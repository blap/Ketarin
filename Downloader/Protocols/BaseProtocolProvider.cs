// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using System;
using System.Net;
using MyDownloader.Core;

namespace MyDownloader.Extension.Protocols
{
    public class BaseProtocolProvider
    {
        protected virtual WebRequest GetRequest(ResourceLocation location)
        {
            WebRequest request = WebRequest.Create(location.Url);
            request.Timeout = 30000;
            SetProxy(request);
            return request;
        }

        protected void SetProxy(WebRequest request)
        {
            if (request == null) return;
            
            if (HttpFtpProtocolExtension.parameters.UseProxy)
            {
                WebProxy proxy = new WebProxy(HttpFtpProtocolExtension.parameters.ProxyAddress, HttpFtpProtocolExtension.parameters.ProxyPort);
                proxy.BypassProxyOnLocal = HttpFtpProtocolExtension.parameters.ProxyByPassOnLocal;
                request.Proxy = proxy;

                if (!String.IsNullOrEmpty(HttpFtpProtocolExtension.parameters.ProxyUserName))
                {
                    WebProxy? innerProxy = request.Proxy as WebProxy;
                    if (innerProxy != null)
                    {
                        innerProxy.Credentials = new NetworkCredential(
                            HttpFtpProtocolExtension.parameters.ProxyUserName,
                            HttpFtpProtocolExtension.parameters.ProxyPassword,
                            HttpFtpProtocolExtension.parameters.ProxyDomain);
                    }
                }
            }
        }
    }
}