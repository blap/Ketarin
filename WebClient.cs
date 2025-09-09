using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using CDBurnerXP;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ketarin
{
    /// <summary>
    /// The modified WebClient used by Ketarin for 
    /// downloads. Submits a valid user agent by default.
    /// </summary>
    internal class WebClient : System.Net.WebClient
    {
        private static string? defaultUserAgent;
        private string replacementString = string.Empty;
        private HttpClient httpClient = new HttpClient();
        
        #region Properties

        /// <summary>
        /// If the WebClient has been redirected after a request,
        /// this specifies the new URL.
        /// </summary>
        public Uri? ResponseUri { get; private set; }

        /// <summary>
        /// Gets the plain POST data which is being ursed for a request.
        /// </summary>
        public string PostData { get; private set; } = string.Empty;

        /// <summary>
        /// Default user agent for all requests.
        /// </summary>
        public static string DefaultUserAgent
        {
            get { return defaultUserAgent ?? (defaultUserAgent = Settings.GetValue("DefaultUserAgent", "Mozilla/4.0 (compatible; Ketarin; +https://ketarin.org/)") as string ?? string.Empty); }
            set { defaultUserAgent = value; }
        }

        #endregion

        public WebClient()
            : this(null)
        {
            // Configure HttpClient with default settings
            httpClient.Timeout = TimeSpan.FromSeconds(Convert.ToInt32(Settings.GetValue("ConnectionTimeout", 10)));
        }

        public WebClient(string? userAgent)
        {
            // Configure HttpClient with user agent
            string userAgentValue = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent ?? string.Empty;
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgentValue);
            
            // Configure other default headers
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

            // MS Bugfix - https://connect.microsoft.com/VisualStudio/feedback/details/386695/system-uri-incorrectly-strips-trailing-dots?wa=wsignin1.0#
            MethodInfo? getSyntax = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
            FieldInfo? flagsField = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (string scheme in new[] { "http", "https" })
                {
                    object? parserObj = getSyntax.Invoke(null, new object[] { scheme });
                    UriParser? parser = parserObj as UriParser;
                    if (parser != null)
                    {
                        object? flagsValueObj = flagsField.GetValue(parser);
                        int flagsValue = flagsValueObj != null ? (int)flagsValueObj : 0;
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads a string from the specified URI using HttpClient.
        /// </summary>
        /// <param name="address">The URI to download from.</param>
        /// <returns>The downloaded string.</returns>
        public new async Task<string> DownloadStringTaskAsync(string address)
        {
            try
            {
                return await DownloadStringTaskAsync(new Uri(address));
            }
            catch (UriFormatException)
            {
                throw new UriFormatException("The format of the URI \"" + address + "\" cannot be determined.");
            }
        }

        /// <summary>
        /// Downloads a string from the specified URI using HttpClient.
        /// </summary>
        /// <param name="address">The URI to download from.</param>
        /// <returns>The downloaded string.</returns>
        public new async Task<string> DownloadStringTaskAsync(Uri address)
        {
            this.replacementString = string.Empty;

            try
            {
                // Handle POST data if present
                if (!string.IsNullOrEmpty(this.PostData))
                {
                    var content = new StringContent(this.PostData, Encoding.UTF8, "application/x-www-form-urlencoded");
                    var response = await httpClient.PostAsync(address, content);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Simple GET request
                    return await httpClient.GetStringAsync(address);
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle SSL/TLS issues
                if (ex.InnerException is System.Security.Authentication.AuthenticationException)
                {
                    // Try with different security protocols
                    var oldHandler = httpClient;
                    httpClient = new HttpClient(new HttpClientHandler()
                    {
                        SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    });
                    httpClient.Timeout = oldHandler.Timeout;
                    
                    try
                    {
                        if (!string.IsNullOrEmpty(this.PostData))
                        {
                            var content = new StringContent(this.PostData, Encoding.UTF8, "application/x-www-form-urlencoded");
                            var response = await httpClient.PostAsync(address, content);
                            response.EnsureSuccessStatusCode();
                            return await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            return await httpClient.GetStringAsync(address);
                        }
                    }
                    finally
                    {
                        // Restore default protocols
                        httpClient = oldHandler;
                    }
                }

                if (!string.IsNullOrEmpty(this.replacementString))
                {
                    return this.replacementString;
                }

                throw;
            }
        }

        /// <summary>
        /// Downloads a string from the specified URI using HttpClient (synchronous wrapper).
        /// </summary>
        /// <param name="address">The URI to download from.</param>
        /// <returns>The downloaded string.</returns>
        public new string DownloadString(string address)
        {
            return DownloadStringTaskAsync(address).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Downloads a string from the specified URI using HttpClient (synchronous wrapper).
        /// </summary>
        /// <param name="address">The URI to download from.</param>
        /// <returns>The downloaded string.</returns>
        public new string DownloadString(Uri address)
        {
            return DownloadStringTaskAsync(address).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the response for the specified request using HttpClient.
        /// </summary>
        /// <param name="request">The request to get response for.</param>
        /// <returns>The response for the request.</returns>
        public static async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request)
        {
            using (var client = new HttpClient())
            {
                return await client.SendAsync(request);
            }
        }

        /// <summary>
        /// Sets the POST data for this WebClient based on a UrlVariable.
        /// </summary>
        /// <param name="variable">The UrlVariable containing POST data.</param>
        public void SetPostData(UrlVariable variable)
        {
            this.PostData = variable?.PostData ?? string.Empty;
        }

        /// <summary>
        /// Parses POST data string into key-value pairs.
        /// </summary>
        /// <param name="postData">The POST data string to parse.</param>
        /// <returns>An array of string arrays, each containing a key-value pair.</returns>
        public static string[][] GetKeyValuePairs(string postData)
        {
            if (string.IsNullOrEmpty(postData))
                return new string[0][];

            var pairs = new List<string[]>();
            string[] keyValuePairs = postData.Split('&');
            
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length >= 2)
                {
                    pairs.Add(new[] { 
                        System.Web.HttpUtility.UrlDecode(keyValue[0]), 
                        System.Web.HttpUtility.UrlDecode(keyValue[1]) 
                    });
                }
                else if (keyValue.Length == 1)
                {
                    pairs.Add(new[] { 
                        System.Web.HttpUtility.UrlDecode(keyValue[0]), 
                        string.Empty 
                    });
                }
            }
            
            return pairs.ToArray();
        }

        /// <summary>
        /// Gets the response for the specified WebRequest.
        /// </summary>
        /// <param name="request">The WebRequest to get response for.</param>
        /// <returns>The WebResponse for the request.</returns>
        public static WebResponse GetResponse(WebRequest request)
        {
            // This is a synchronous wrapper around the async method
            return request.GetResponse();
        }

        /// <summary>
        /// Disposes of the HttpClient resources.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if called from finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                httpClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}