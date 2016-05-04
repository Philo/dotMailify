using System;
using System.Configuration;
using System.Reflection;
using dotMailify.Core.Abstractions;
using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Logging;

namespace dotMailify.Core
{
    public class DefaultEmailProviderFactory : EmailProviderFactoryBase
    {
        private readonly string _providerTypeString;

        public DefaultEmailProviderFactory(string providerTypeString)
        {
            _providerTypeString = providerTypeString;
        }

        public DefaultEmailProviderFactory()
        {
        }

        protected override IEmailProvider CreateInstanceCore(Type emailProviderType, IEmailLoggingProvider emailLoggingProvider)
        {
            var provider = (IEmailProvider)Activator.CreateInstance(emailProviderType);
            provider.EmailLoggingProvider = emailLoggingProvider;
            return provider;
        }

        protected override Type GetProviderType()
        {
            var providerTypeString = _providerTypeString;
            if (string.IsNullOrWhiteSpace(providerTypeString))
            {
                providerTypeString = ConfigurationManager.AppSettings.Get(Constants.Settings.EmailProviderTypeKey);
                if (string.IsNullOrWhiteSpace(providerTypeString)) return null;
            }
            var providerType = Type.GetType(providerTypeString, false, true);
            return providerType;
        }
    }
}