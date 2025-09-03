using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Ketarin
{
    /// <summary>
    /// Provides XML-RPC compatibility methods for .NET 6 migration.
    /// Replaces deprecated .NET Remoting with modern HTTP-based alternatives.
    /// </summary>
    internal static class XmlRpcCompatibility
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static XmlRpcCompatibility()
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Ketarin-XmlRpc/1.0");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        #region XML-RPC Method Calling

        /// <summary>
        /// Calls an XML-RPC method asynchronously.
        /// </summary>
        public static async Task<XElement> CallMethodAsync(string url, string methodName, params object[] parameters)
        {
            var requestXml = CreateXmlRpcRequest(methodName, parameters);
            var responseXml = await SendXmlRpcRequestAsync(url, requestXml);
            return ParseXmlRpcResponse(responseXml);
        }

        /// <summary>
        /// Calls an XML-RPC method with typed return value.
        /// </summary>
        public static async Task<T> CallMethodAsync<T>(string url, string methodName, params object[] parameters)
        {
            var result = await CallMethodAsync(url, methodName, parameters);
            return ConvertXmlRpcValue<T>(result);
        }

        #endregion

        #region Request/Response Handling

        /// <summary>
        /// Creates an XML-RPC request XML document.
        /// </summary>
        public static string CreateXmlRpcRequest(string methodName, params object[] parameters)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("methodCall",
                    new XElement("methodName", methodName),
                    new XElement("params",
                        parameters.Select(p => new XElement("param",
                            new XElement("value", ConvertToXmlRpcValue(p))
                        ))
                    )
                )
            );

            return doc.ToString();
        }

        /// <summary>
        /// Sends an XML-RPC request and returns the response.
        /// </summary>
        public static async Task<string> SendXmlRpcRequestAsync(string url, string xmlRequest)
        {
            using var content = new StringContent(xmlRequest, Encoding.UTF8, "text/xml");

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            // Add XML-RPC specific headers
            request.Headers.Add("Accept", "text/xml");
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Parses an XML-RPC response and returns the result element.
        /// </summary>
        public static XElement ParseXmlRpcResponse(string xmlResponse)
        {
            var doc = XDocument.Parse(xmlResponse);
            var fault = doc.Root.Element("fault");

            if (fault != null)
            {
                throw new XmlRpcFaultException(ParseXmlRpcValue(fault.Element("value")));
            }

            var result = doc.Root.Element("params")?.Element("param")?.Element("value");
            if (result == null)
            {
                throw new XmlRpcException("Invalid XML-RPC response: missing result value");
            }

            return result;
        }

        #endregion

        #region Value Conversion

        /// <summary>
        /// Converts a .NET object to XML-RPC value element.
        /// </summary>
        private static XElement ConvertToXmlRpcValue(object value)
        {
            if (value == null)
            {
                return new XElement("nil");
            }

            switch (value)
            {
                case int i:
                    return new XElement("int", i);
                case long l:
                    return new XElement("i8", l);
                case double d:
                    return new XElement("double", d);
                case bool b:
                    return new XElement("boolean", b ? "1" : "0");
                case string s:
                    return new XElement("string", s);
                case DateTime dt:
                    return new XElement("dateTime.iso8601", dt.ToString("yyyyMMddTHH:mm:ss"));
                case byte[] bytes:
                    return new XElement("base64", Convert.ToBase64String(bytes));
                case IEnumerable<object> array:
                    return new XElement("array",
                        new XElement("data",
                            array.Select(item => new XElement("value", ConvertToXmlRpcValue(item)))
                        )
                    );
                case Dictionary<string, object> dict:
                    return new XElement("struct",
                        dict.Select(kvp => new XElement("member",
                            new XElement("name", kvp.Key),
                            new XElement("value", ConvertToXmlRpcValue(kvp.Value))
                        ))
                    );
                default:
                    return new XElement("string", value.ToString());
            }
        }

        /// <summary>
        /// Parses an XML-RPC value element to .NET object.
        /// </summary>
        private static object ParseXmlRpcValue(XElement valueElement)
        {
            if (valueElement == null) return null;

            var child = valueElement.Elements().FirstOrDefault();
            if (child == null) return valueElement.Value;

            switch (child.Name.LocalName)
            {
                case "int":
                case "i4":
                    return int.Parse(child.Value);
                case "i8":
                    return long.Parse(child.Value);
                case "double":
                    return double.Parse(child.Value);
                case "boolean":
                    return child.Value == "1" || child.Value.ToLower() == "true";
                case "string":
                    return child.Value;
                case "dateTime.iso8601":
                    return DateTime.Parse(child.Value);
                case "base64":
                    return Convert.FromBase64String(child.Value);
                case "array":
                    var data = child.Element("data");
                    return data?.Elements("value").Select(v => ParseXmlRpcValue(v)).ToArray();
                case "struct":
                    var dict = new Dictionary<string, object>();
                    foreach (var member in child.Elements("member"))
                    {
                        var name = member.Element("name")?.Value;
                        var val = member.Element("value");
                        if (name != null && val != null)
                        {
                            dict[name] = ParseXmlRpcValue(val);
                        }
                    }
                    return dict;
                case "nil":
                    return null;
                default:
                    return child.Value;
            }
        }

        /// <summary>
        /// Converts an XML-RPC value to a specific type.
        /// </summary>
        private static T ConvertXmlRpcValue<T>(XElement valueElement)
        {
            var obj = ParseXmlRpcValue(valueElement);
            if (obj == null) return default(T);

            if (obj is T result)
            {
                return result;
            }

            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #region Exception Classes

        /// <summary>
        /// Represents an XML-RPC fault.
        /// </summary>
        public class XmlRpcFaultException : Exception
        {
            public object FaultCode { get; }
            public string FaultString { get; }

            public XmlRpcFaultException(object faultValue)
                : base($"XML-RPC Fault: {faultValue}")
            {
                if (faultValue is Dictionary<string, object> faultDict)
                {
                    FaultCode = faultDict.GetValueOrDefault("faultCode");
                    FaultString = faultDict.GetValueOrDefault("faultString")?.ToString();
                }
            }
        }

        /// <summary>
        /// Represents an XML-RPC related exception.
        /// </summary>
        public class XmlRpcException : Exception
        {
            public XmlRpcException(string message) : base(message) { }
            public XmlRpcException(string message, Exception innerException) : base(message, innerException) { }
        }

        #endregion

        #region Proxy Generation (Compatibility Layer)

        /// <summary>
        /// Creates a dynamic proxy for XML-RPC services (limited compatibility).
        /// </summary>
        public static T CreateProxy<T>(string url) where T : class
        {
            // This is a simplified implementation
            // In a full implementation, this would use dynamic proxy generation
            // For now, return null to indicate this feature is not fully implemented
            return null;
        }

        #endregion
    }

    #region Extension Methods

    internal static class XmlRpcExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out var value) ? value : default(TValue);
        }
    }

    #endregion
}