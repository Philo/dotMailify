using System;
using dotMailify.Core.Logging;

namespace dotMailify.Core.Abstractions
{
    public abstract class EmailProviderFactoryBase : IEmailProviderFactory
    {
        public IEmailLoggingProvider EmailLoggingProvider { get; set; }

        protected EmailProviderFactoryBase()
        {
            EmailLoggingProvider = new NullEmailLoggingProvider();
        }

        public IEmailProvider GetEmailProvider()
        {
            var providerType = GetProviderType();
            if (providerType == null) throw new ArgumentNullException(nameof(providerType), Validation.EmailProviderTypeNotFound);
            if (!typeof(IEmailProvider).IsAssignableFrom(providerType)) throw new InvalidCastException("the specified type is not an email provider");

            return CreateInstance(providerType);
        }

        private IEmailProvider CreateInstance(Type emailProviderType)
        {
            return CreateInstanceCore(emailProviderType, EmailLoggingProvider);
        }
        
        protected abstract IEmailProvider CreateInstanceCore(Type emailProviderType, IEmailLoggingProvider emailLoggingProvider);

        protected abstract Type GetProviderType();
    }
}