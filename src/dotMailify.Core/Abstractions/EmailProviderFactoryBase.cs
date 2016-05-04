using System;

namespace dotMailify.Core.Abstractions
{
    public abstract class EmailProviderFactoryBase : IEmailProviderFactory
    {
        public IEmailProvider GetEmailProvider()
        {
            var providerType = GetProviderType();
            if (providerType == null) throw new ArgumentNullException(nameof(providerType), Validation.EmailProviderTypeNotFound);
            if (!typeof(IEmailProvider).IsAssignableFrom(providerType)) throw new InvalidCastException("the specified type is not an email provider");
            return CreateInstance(providerType);
        }

        public IEmailProvider CreateInstance(Type emailProviderType)
        {
            return CreateInstanceCore(emailProviderType);
        }

        public abstract IEmailProvider CreateInstanceCore(Type emailProviderType);

        protected abstract Type GetProviderType();        
    }
}