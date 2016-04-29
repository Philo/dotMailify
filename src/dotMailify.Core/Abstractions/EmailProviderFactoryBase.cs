using System;

namespace dotMailify.Core.Abstractions
{
    public abstract class EmailProviderFactoryBase : IEmailProviderFactory
    {
        public IEmailProvider GetEmailProvider()
        {
            var providerType = GetProviderType();
            if (providerType == null) throw new ArgumentNullException(nameof(providerType), "the specified email provider type can be found");
            if (!typeof(IEmailProvider).IsAssignableFrom(providerType)) throw new InvalidCastException("the specified type is not an email provider");
            return CreateInstance(providerType);
        }

        public abstract IEmailProvider CreateInstance(Type emailProviderType);

        protected abstract Type GetProviderType();
    }
}