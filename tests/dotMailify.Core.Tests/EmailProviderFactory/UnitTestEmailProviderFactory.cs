using dotMailify.Core.Abstractions;
using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Core.Tests.EmailProviderFactory
{
    public class UnitTestEmailProviderFactory : IEmailProviderFactory
    {
        private readonly IEmailProvider _provider;
        public UnitTestEmailProviderFactory(IEmailProviderSettings settings)
        {
            _provider = new UnitTestTrackingEmailProvider(settings);
        }

        public IEmailProvider GetEmailProvider()
        {
            return _provider;
        }
    }
}