using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ketarin
{
    /// <summary>
    /// Provides HTTP compatibility methods for .NET 6 migration.
    /// Handles deprecated WebClient and HttpWebRequest usage.
    /// </summary>
    internal static class HttpCompatibility
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        static HttpCompatibility()
        {
            // Configure default headers
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/4.0 (compatible; Ketarin; +https://ketarin.org/)");
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            _httpClient.Timeout = TimeSpan.FromSeconds(30); // Default timeout

            // Ignore SSL certificate validation errors (similar to original behavior)
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
        }

        /// <summary>
        /// Downloads a string from the specified URL asynchronously.
        /// </summary>
        public static async Task<string> DownloadStringAsync(string url, string userAgent = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.ParseAdd(userAgent);
            }

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Downloads data from the specified URL asynchronously.
        /// </summary>
        public static async Task<byte[]> DownloadDataAsync(string url, string userAgent = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.ParseAdd(userAgent);
            }

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        /// <summary>
        /// Downloads a file from the specified URL to the specified path asynchronously.
        /// </summary>
        public static async Task DownloadFileAsync(string url, string filePath, string userAgent = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.ParseAdd(userAgent);
            }

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var fileStream = File.Create(filePath);
            await response.Content.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Sends a POST request with form data asynchronously.
        /// </summary>
        public static async Task<string> PostFormDataAsync(string url, Dictionary<string, string> formData, string userAgent = null)
        {
            using var content = new FormUrlEncodedContent(formData);

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.ParseAdd(userAgent);
            }

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Gets the response URI after following redirects.
        /// </summary>
        public static async Task<Uri> GetResponseUriAsync(string url, string userAgent = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, url);

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.Headers.UserAgent.ParseAdd(userAgent);
            }

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            return response.RequestMessage.RequestUri;
        }

        /// <summary>
        /// Sets the default timeout for HTTP operations.
        /// </summary>
        public static void SetTimeout(int seconds)
        {
            _httpClient.Timeout = TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Gets the current timeout setting.
        /// </summary>
        public static int GetTimeout()
        {
            return (int)_httpClient.Timeout.TotalSeconds;
        }

        /// <summary>
        /// Adds a request to the cancellation list (for compatibility).
        /// </summary>
        public static void AddRequestToCancel(HttpRequestMessage request)
        {
            // Implementation would depend on the Updater class
            // This is a placeholder for compatibility
        }

        /// <summary>
        /// Checks if the given URI needs protocol fixing.
        /// </summary>
        public static Uri FixNoProtocolUri(Uri uri)
        {
            if (uri == null) return null;

            string uriString = uri.ToString();

            // If no protocol is specified, assume http
            if (!uriString.Contains("://"))
            {
                uriString = "http://" + uriString;
                return new Uri(uriString);
            }

            return uri;
        }
    }
}