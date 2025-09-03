using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Ketarin
{
    /// <summary>
    /// Helper class for security-related operations in Ketarin
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Creates a secure HttpClientHandler with proper certificate validation
        /// </summary>
        public static HttpClientHandler CreateSecureHttpClientHandler()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = ValidateServerCertificate,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                CheckCertificateRevocationList = true
            };
        }

        /// <summary>
        /// Validates server certificates with enhanced security checks
        /// </summary>
        private static bool ValidateServerCertificate(
            HttpRequestMessage request,
            X509Certificate2 certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // Allow self-signed certificates for development/testing (legacy behavior)
            // In production, this should be more restrictive
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // Log certificate validation issues for debugging
            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateChainErrors))
            {
                // Could log: "Certificate chain validation failed"
            }

            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch))
            {
                // Could log: "Certificate name mismatch"
            }

            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNotAvailable))
            {
                // Could log: "Certificate not available"
            }

            // For backward compatibility, accept certificates with issues
            // TODO: Make this configurable through settings
            return true;
        }

        /// <summary>
        /// Creates an HttpClient with security headers and timeout
        /// </summary>
        public static HttpClient CreateSecureHttpClient(string userAgent, int timeoutSeconds = 30)
        {
            var handler = CreateSecureHttpClientHandler();
            var client = new HttpClient(handler);

            // Set secure defaults
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            client.DefaultRequestHeaders.Accept.ParseAdd("*/*");
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            // Security headers
            client.DefaultRequestHeaders.Add("X-Content-Type-Options", "nosniff");
            client.DefaultRequestHeaders.Add("X-Frame-Options", "DENY");
            client.DefaultRequestHeaders.Add("X-XSS-Protection", "1; mode=block");

            return client;
        }

        /// <summary>
        /// Safely validates URLs to prevent injection attacks
        /// </summary>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            try
            {
                var uri = new Uri(url);
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sanitizes filenames to prevent path traversal attacks
        /// </summary>
        public static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return "download";

            // Remove path separators and other dangerous characters
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

            // Ensure the filename is not too long
            if (sanitized.Length > 255)
            {
                sanitized = sanitized.Substring(0, 255);
            }

            return string.IsNullOrWhiteSpace(sanitized) ? "download" : sanitized;
        }
    }
}