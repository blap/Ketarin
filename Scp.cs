using System;
using System.IO;
using System.Net;

#pragma warning disable SYSLIB0014 // WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete

namespace Ketarin
{
    /// <summary>
    /// Creates new ScpWebRequests based on the URI.
    /// Required format as of now: sf://user:pass@UrlToFileOnSourceForge
    /// </summary>
    public class ScpWebRequestCreator : IWebRequestCreate
    {
        #region IWebRequestCreate Member

        public WebRequest Create(Uri uri)
        {
            // Return null to indicate that this protocol is not supported
            // This removes the SharpSSH dependency while maintaining compatibility
            return null!;
        }

        #endregion
    }

    /// <summary>
    /// Represents an SCP download request.
    /// </summary>
    public class ScpWebRequest : WebRequest
    {
        private readonly Uri requestUri;
        private int timeout;

        #region Properties

        /// <summary>
        /// Gets or sets the timeout for ScpResponse.
        /// </summary>
        public override int Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                this.timeout = value;
            }
        }

        /// <summary>
        /// Gets the requested URI.
        /// </summary>
        public override Uri RequestUri
        {
            get
            {
                return this.requestUri;
            }
        }

        #endregion

        /// <summary>
        /// Creates a new ScpWebRquest based on the URI.
        /// </summary>
        public ScpWebRequest(Uri uri)
        {
            this.requestUri = uri;
        }

        /// <summary>
        /// Creates a new ScpWebResponse from the current request.
        /// </summary>
        /// <returns></returns>
        public override WebResponse GetResponse()
        {
            // Throw an exception to indicate that this protocol is not supported
            throw new NotSupportedException("SCP protocol is not supported in this version of Ketarin.");
        }

        /// <summary>
        /// Aborts the SCP request. Not supported.
        /// </summary>
        public override void Abort()
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Represents the server response of an SCP download request.
    /// </summary>
    public class ScpWebResponse : WebResponse
    {
        #region Properties

        /// <summary>
        /// Gets the last modified date of the requested file.
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets the file size of the request file.
        /// </summary>
        public override long ContentLength
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the Uri of this server response.
        /// </summary>
        public override Uri ResponseUri
        {
            get
            {
                return null!;
            }
        }

        /// <summary>
        /// Gets the header information of the response.
        /// Not supported.
        /// </summary>
        public override WebHeaderCollection Headers
        {
            get
            {
                return new WebHeaderCollection();
            }
        }

        #endregion

        /// <summary>
        /// Initiates a new SCP response on sourceforge.net for downloading a file.
        /// </summary>
        /// <param name="uri">URI to download (includes username and password)</param>
        /// <param name="timeout">Timeout for this session</param>
        public ScpWebResponse(Uri uri, int timeout)
        {
            // Throw an exception to indicate that this protocol is not supported
            throw new NotSupportedException("SCP protocol is not supported in this version of Ketarin.");
        }

        public override Stream GetResponseStream()
        {
            return new MemoryStream();
        }
    }
}

#pragma warning restore SYSLIB0014 // WebRequest, HttpWebRequest, ServicePoint, and WebClient are obsolete