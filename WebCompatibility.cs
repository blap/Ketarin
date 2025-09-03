using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Ketarin
{
    /// <summary>
    /// Provides web utility compatibility methods for .NET 6 migration.
    /// Replaces deprecated System.Web utilities with modern alternatives.
    /// </summary>
    internal static class WebCompatibility
    {
        #region URL Encoding/Decoding

        /// <summary>
        /// URL-encodes a string (equivalent to HttpUtility.UrlEncode).
        /// </summary>
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// URL-decodes a string (equivalent to HttpUtility.UrlDecode).
        /// </summary>
        public static string UrlDecode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return WebUtility.UrlDecode(value);
        }

        /// <summary>
        /// HTML-encodes a string (equivalent to HttpUtility.HtmlEncode).
        /// </summary>
        public static string HtmlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return WebUtility.HtmlEncode(value);
        }

        /// <summary>
        /// HTML-decodes a string (equivalent to HttpUtility.HtmlDecode).
        /// </summary>
        public static string HtmlDecode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return WebUtility.HtmlDecode(value);
        }

        #endregion

        #region Query String Parsing

        /// <summary>
        /// Parses a query string into a NameValueCollection.
        /// </summary>
        public static NameValueCollection ParseQueryString(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
                return new NameValueCollection();

            var collection = new NameValueCollection();

            if (queryString.StartsWith("?"))
                queryString = queryString.Substring(1);

            var pairs = queryString.Split('&');
            foreach (var pair in pairs)
            {
                if (string.IsNullOrEmpty(pair)) continue;

                var parts = pair.Split('=', 2);
                var key = UrlDecode(parts[0]);
                var value = parts.Length > 1 ? UrlDecode(parts[1]) : string.Empty;

                collection.Add(key, value);
            }

            return collection;
        }

        /// <summary>
        /// Converts a NameValueCollection to a query string.
        /// </summary>
        public static string ToQueryString(NameValueCollection collection)
        {
            if (collection == null || collection.Count == 0)
                return string.Empty;

            var parts = new List<string>();
            foreach (string key in collection.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                var values = collection.GetValues(key);
                if (values != null)
                {
                    foreach (var value in values)
                    {
                        parts.Add($"{UrlEncode(key)}={UrlEncode(value ?? string.Empty)}");
                    }
                }
                else
                {
                    parts.Add($"{UrlEncode(key)}=");
                }
            }

            return string.Join("&", parts);
        }

        #endregion

        #region Form Data Handling

        /// <summary>
        /// Parses form data from a POST request body.
        /// </summary>
        public static Dictionary<string, string> ParseFormData(string formData)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(formData))
                return result;

            var pairs = formData.Split('&');
            foreach (var pair in pairs)
            {
                if (string.IsNullOrEmpty(pair)) continue;

                var parts = pair.Split('=', 2);
                var key = UrlDecode(parts[0]);
                var value = parts.Length > 1 ? UrlDecode(parts[1]) : string.Empty;

                result[key] = value;
            }

            return result;
        }

        /// <summary>
        /// Converts a dictionary to form-encoded data.
        /// </summary>
        public static string ToFormData(Dictionary<string, string> data)
        {
            if (data == null || data.Count == 0)
                return string.Empty;

            var parts = data.Select(kvp =>
                $"{UrlEncode(kvp.Key)}={UrlEncode(kvp.Value ?? string.Empty)}");

            return string.Join("&", parts);
        }

        /// <summary>
        /// Converts a NameValueCollection to form-encoded data.
        /// </summary>
        public static string ToFormData(NameValueCollection data)
        {
            if (data == null || data.Count == 0)
                return string.Empty;

            var parts = new List<string>();
            foreach (string key in data.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                var values = data.GetValues(key);
                if (values != null)
                {
                    foreach (var value in values)
                    {
                        parts.Add($"{UrlEncode(key)}={UrlEncode(value ?? string.Empty)}");
                    }
                }
            }

            return string.Join("&", parts);
        }

        #endregion

        #region Path Utilities

        /// <summary>
        /// Maps a virtual path to a physical path (equivalent to HttpContext.Current.Server.MapPath).
        /// </summary>
        public static string MapPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
                return virtualPath;

            // For .NET 6, we need to handle this differently since HttpContext is not available
            // This is a simplified implementation
            if (virtualPath.StartsWith("~"))
            {
                virtualPath = virtualPath.Substring(1);
            }

            if (virtualPath.StartsWith("/"))
            {
                virtualPath = virtualPath.Substring(1);
            }

            return virtualPath.Replace('/', System.IO.Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Combines URL paths safely.
        /// </summary>
        public static string CombineUrls(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrEmpty(baseUrl)) return relativeUrl;
            if (string.IsNullOrEmpty(relativeUrl)) return baseUrl;

            baseUrl = baseUrl.TrimEnd('/');
            relativeUrl = relativeUrl.TrimStart('/');

            return $"{baseUrl}/{relativeUrl}";
        }

        #endregion

        #region Cookie Handling

        /// <summary>
        /// Parses a cookie header value.
        /// </summary>
        public static Dictionary<string, string> ParseCookies(string cookieHeader)
        {
            var cookies = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(cookieHeader))
                return cookies;

            var cookiePairs = cookieHeader.Split(';');
            foreach (var pair in cookiePairs)
            {
                var trimmedPair = pair.Trim();
                if (string.IsNullOrEmpty(trimmedPair)) continue;

                var parts = trimmedPair.Split('=', 2);
                var name = parts[0].Trim();
                var value = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                cookies[name] = value;
            }

            return cookies;
        }

        /// <summary>
        /// Creates a cookie header value from a dictionary.
        /// </summary>
        public static string ToCookieHeader(Dictionary<string, string> cookies)
        {
            if (cookies == null || cookies.Count == 0)
                return string.Empty;

            var parts = cookies.Select(kvp => $"{kvp.Key}={kvp.Value}");
            return string.Join("; ", parts);
        }

        #endregion
    }
}