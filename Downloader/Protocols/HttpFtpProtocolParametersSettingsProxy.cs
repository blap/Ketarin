// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using MyDownloader.Core;

namespace MyDownloader.Extension.Protocols
{
    internal class HttpFtpProtocolParametersSettingsProxy : IHttpFtpProtocolParameters
    {
        #region IHttpFtpProtocolParameters Members

        public string ProxyAddress
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyAddress;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyAddress = value;
            }
        }

        public string ProxyUserName
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyUserName;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyUserName = value;
            }
        }

        public string ProxyPassword
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyPassword;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyPassword = value;
            }
        }

        public string ProxyDomain
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyDomain;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyDomain = value;
            }
        }

        public bool UseProxy
        {
            get
            {
                return Ketarin.Downloader.Settings.UseProxy;
            }
            set
            {
                Ketarin.Downloader.Settings.UseProxy = value;
            }
        }

        public bool ProxyByPassOnLocal
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyByPassOnLocal;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyByPassOnLocal = value;
            }
        }

        public int ProxyPort
        {
            get
            {
                return Ketarin.Downloader.Settings.ProxyPort;
            }
            set
            {
                Ketarin.Downloader.Settings.ProxyPort = value;
            }
        }

        #endregion
    }
}