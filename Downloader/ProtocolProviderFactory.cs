// <copyright>
// The Code Project Open License (CPOL) 1.02
// </copyright>
// <author>Guilherme Labigalini</author>

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace MyDownloader.Core
{
    public static class ProtocolProviderFactory
    {
        private static Hashtable protocolHandlers = new Hashtable();

        public static event EventHandler<ResolvingProtocolProviderEventArgs>? ResolvingProtocolProvider;

        public static void RegisterProtocolHandler(string prefix, Type protocolProvider)
        {
            protocolHandlers[prefix] = protocolProvider;
        }

        public static IProtocolProvider CreateProvider(string uri, Downloader downloader)
        {
            IProtocolProvider provider = InternalGetProvider(uri);

            // Removed call to Initialize as it was not used for any meaningful purpose

            return provider;
        }

        public static IProtocolProvider GetProvider(string uri)
        {
            return InternalGetProvider(uri);
        }

        public static Type? GetProviderType(string uri)
        {
            int index = uri.IndexOf("://");

            if (index > 0)
            {
                string prefix = uri.Substring(0, index);
                Type? type = protocolHandlers[prefix] as Type;
                return type;
            }
            else
            {
                return null;
            }
        }

        public static IProtocolProvider CreateProvider(Type providerType, Downloader downloader)
        {
            IProtocolProvider provider = CreateFromType(providerType);

            if (ResolvingProtocolProvider != null)
            {
                ResolvingProtocolProviderEventArgs e = new ResolvingProtocolProviderEventArgs(provider, null!);
                ResolvingProtocolProvider(null, e);
                provider = e.ProtocolProvider;
            }

            // Removed call to Initialize as it was not used for any meaningful purpose

            return provider;
        }

        private static IProtocolProvider InternalGetProvider(string uri)
        {
            Type? type = GetProviderType(uri);

            if (type == null)
            {
                throw new ArgumentException("No protocol handler registered for the given URI", nameof(uri));
            }

            IProtocolProvider provider = CreateFromType(type);

            if (ResolvingProtocolProvider != null)
            {
                ResolvingProtocolProviderEventArgs e = new ResolvingProtocolProviderEventArgs(provider, uri);
                ResolvingProtocolProvider(null, e);
                provider = e.ProtocolProvider;
            }

            return provider;
        }

        private static IProtocolProvider CreateFromType(Type type)
        {
            IProtocolProvider? provider = Activator.CreateInstance(type) as IProtocolProvider;
            return provider ?? throw new InvalidOperationException($"Failed to create instance of {type.FullName}");
        }
    }
}