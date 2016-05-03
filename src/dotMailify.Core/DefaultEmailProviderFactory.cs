using System;
using System.Configuration;
using dotMailify.Core.Abstractions;

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

        public override IEmailProvider CreateInstanceCore(Type emailProviderType)
        {
            return (IEmailProvider)Activator.CreateInstance(emailProviderType);
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