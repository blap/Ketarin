using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CDBurnerXP;

namespace Ketarin
{
    /// <summary>
    /// Custom WebRequest for HTTP/HTTPS with extended functionality.
    /// This implementation uses HttpClient instead of the obsolete WebRequest.
    /// </summary>
    internal class HttpxRequest : WebRequest
    {
        private readonly Uri m_RequestUri;
        private readonly HttpClient m_HttpClient;
        private string? m_Method = "GET";
        private WebHeaderCollection m_Headers = new WebHeaderCollection();
        private string? m_ContentType;
        private long m_ContentLength;
        private Stream? m_ContentStream;
        private int m_Timeout = 100000; // Default timeout in milliseconds

        public HttpxRequest(Uri uri)
        {
            m_RequestUri = uri;
            
            // Create HttpClient with default settings
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false // We'll handle cookies manually if needed
            };
            
            m_HttpClient = new HttpClient(handler);
            m_HttpClient.Timeout = TimeSpan.FromMilliseconds(m_Timeout);
        }

        /// <summary>
        /// Gets the URI of the request.
        /// </summary>
        public override Uri RequestUri => m_RequestUri;

        /// <summary>
        /// Gets or sets the protocol method to use in this request.
        /// </summary>
        public override string Method
        {
            get => m_Method ?? string.Empty;
            set => m_Method = value;
        }

        /// <summary>
        /// Gets or sets the contents of the header that is sent with the request.
        /// </summary>
        public override WebHeaderCollection Headers
        {
            get => m_Headers;
            set => m_Headers = value ?? new WebHeaderCollection();
        }

        /// <summary>
        /// Gets or sets the content type of the request data being sent.
        /// </summary>
        public override string? ContentType
        {
            get => m_ContentType;
            set => m_ContentType = value;
        }

        /// <summary>
        /// Gets or sets the number of milliseconds before the request times out.
        /// </summary>
        public override int Timeout
        {
            get => m_Timeout;
            set
            {
                m_Timeout = value;
                m_HttpClient.Timeout = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// Gets or sets authentication information for the request.
        /// </summary>
        public override ICredentials? Credentials { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to send an authenticate header with the request.
        /// </summary>
        public override bool PreAuthenticate { get; set; }

        /// <summary>
        /// Gets or sets proxy information for the request.
        /// </summary>
        public override IWebProxy? Proxy { get; set; }

        /// <summary>
        /// Creates a stream for writing data to the Internet resource.
        /// </summary>
        /// <returns>A Stream for writing data to the Internet resource.</returns>
        public override Stream GetRequestStream()
        {
            m_ContentStream = new MemoryStream();
            return m_ContentStream ?? throw new InvalidOperationException("Failed to create request stream");
        }

        /// <summary>
        /// Returns a response to an Internet request.
        /// </summary>
        /// <returns>A WebResponse containing the response to the Internet request.</returns>
        public override WebResponse GetResponse()
        {
            // This is a synchronous wrapper around the async method
            return GetResponseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns a response to an Internet request asynchronously.
        /// </summary>
        /// <returns>A Task that represents the asynchronous operation and contains the WebResponse.</returns>
        public override async Task<WebResponse> GetResponseAsync()
        {
            try
            {
                // Create HttpRequestMessage
                var request = new HttpRequestMessage(new HttpMethod(m_Method ?? "GET"), m_RequestUri);

                // Add headers
                foreach (string key in m_Headers.Keys)
                {
                    // Skip headers that are set automatically by HttpClient
                    if (!key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) &&
                        !key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase) &&
                        !key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                    {
                        request.Headers.TryAddWithoutValidation(key, m_Headers[key]);
                    }
                }

                // Add content if needed
                if (m_ContentStream != null && m_ContentStream.Length > 0)
                {
                    m_ContentStream.Position = 0;
                    var contentBytes = new byte[m_ContentStream.Length];
                    // Fix for CA2022 warning - use ReadAsync with CancellationToken
                    await m_ContentStream.ReadAsync(contentBytes, 0, contentBytes.Length, CancellationToken.None);
                    
                    var content = new ByteArrayContent(contentBytes);
                    
                    // Set content type if specified
                    if (!string.IsNullOrEmpty(m_ContentType))
                    {
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(m_ContentType);
                    }
                    
                    request.Content = content;
                }

                // Send request
                var response = await m_HttpClient.SendAsync(request);

                // Create and return HttpxResponse
                return new HttpxResponse(response, m_RequestUri);
            }
            catch (HttpRequestException ex)
            {
                throw new WebException("An error occurred while sending the request", ex, WebExceptionStatus.UnknownError, null);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                throw new WebException("The request timed out", ex, WebExceptionStatus.Timeout, null);
            }
            catch (Exception ex)
            {
                throw new WebException("An error occurred while sending the request", ex, WebExceptionStatus.UnknownError, null);
            }
        }

        /// <summary>
        /// Aborts the request.
        /// </summary>
        public override void Abort()
        {
            m_HttpClient?.CancelPendingRequests();
        }
    }

    /// <summary>
    /// Custom WebResponse for HTTP/HTTPS with extended functionality.
    /// </summary>
    internal class HttpxResponse : WebResponse
    {
        private readonly HttpResponseMessage m_HttpResponse;
        private readonly Uri m_ResponseUri;

        public HttpxResponse(HttpResponseMessage httpResponse, Uri requestUri)
        {
            m_HttpResponse = httpResponse;
            m_ResponseUri = httpResponse.RequestMessage?.RequestUri ?? requestUri;
        }

        /// <summary>
        /// Gets the URI of the Internet resource that actually responded to the request.
        /// </summary>
        public override Uri ResponseUri => m_ResponseUri;

        /// <summary>
        /// Gets the headers that are associated with this response from the server.
        /// </summary>
        public override WebHeaderCollection Headers
        {
            get
            {
                var headers = new WebHeaderCollection();
                foreach (var header in m_HttpResponse.Headers)
                {
                    headers[header.Key] = string.Join(", ", header.Value);
                }
                
                if (m_HttpResponse.Content != null)
                {
                    foreach (var header in m_HttpResponse.Content.Headers)
                    {
                        headers[header.Key] = string.Join(", ", header.Value);
                    }
                }
                
                return headers;
            }
        }

        /// <summary>
        /// Gets the content length of data being received.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                if (m_HttpResponse.Content?.Headers.ContentLength.HasValue == true)
                {
                    return m_HttpResponse.Content.Headers.ContentLength.Value;
                }
                return -1;
            }
            set { /* Not implemented for responses */ }
        }

        /// <summary>
        /// Gets the content type of the data being received.
        /// </summary>
        public override string ContentType
        {
            get
            {
                if (m_HttpResponse.Content?.Headers.ContentType?.MediaType != null)
                {
                    return m_HttpResponse.Content.Headers.ContentType.MediaType;
                }
                return string.Empty;
            }
            set { /* Not implemented for responses */ }
        }

        /// <summary>
        /// Gets the stream that is used to read the body of the response from the server.
        /// </summary>
        /// <returns>A Stream containing the body of the response.</returns>
        public override Stream GetResponseStream()
        {
            if (m_HttpResponse.Content == null)
            {
                return new MemoryStream();
            }
            
            // This is a synchronous wrapper around the async method
            return m_HttpResponse.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Closes the response stream.
        /// </summary>
        public override void Close()
        {
            m_HttpResponse?.Dispose();
        }
    }

    /// <summary>
    /// WebRequest creator for HttpxRequest.
    /// </summary>
    internal class HttpxRequestCreator : IWebRequestCreate
    {
        /// <summary>
        /// Creates a WebRequest instance.
        /// </summary>
        /// <param name="uri">The URI of the resource to request.</param>
        /// <returns>A WebRequest instance.</returns>
        public WebRequest Create(Uri uri)
        {
            return new HttpxRequest(uri);
        }
    }
}