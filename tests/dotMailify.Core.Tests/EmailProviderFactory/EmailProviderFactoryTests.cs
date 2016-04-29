using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using dotMailify.Core.Message;
using FluentAssertions;
using Xunit;

namespace dotMailify.Core.Tests.EmailProviderFactory
{
    public class EmailProviderFactoryTests
    {
        public class StubEmailProvider : AbstractEmailProvider
        {
            protected override Task SendCore(EmailMessage message)
            {
                return null;
            }
        }

        public class WhenUsingTheDefaultEmailProviderFactory
        {
            [Theory]
            [InlineData("dotMailify.Core.Tests.EmailProviderFactory.EmailProviderFactoryTests+StubEmailProvider, dotMailify.Core.Tests", typeof(StubEmailProvider))]
            public void ShouldCreateInstanceOfConfiguredEmailProvider(string providerType, Type providerTypeType)
            {
                var factory = new DefaultEmailProviderFactory(providerType);
                var provider = factory.GetEmailProvider();
                provider.Should().BeOfType(providerTypeType);
            }

            [Theory]
            [InlineData("dotMailify.Core.Tests.EmailProviderFactory.EmailProviderFactoryTests+StubEmailProvider")]
            [InlineData("")]
            [InlineData(null)]
            public void ShouldNotCreateInstanceOfConfiguredEmailProviderWithInvalidTypeName(string providerType)
            {
                var factory = new DefaultEmailProviderFactory(providerType);
                Assert.Throws<ArgumentNullException>(() => factory.GetEmailProvider());
            }

            [Theory]
            [InlineData("dotMailify.Core.Tests.EmailProviderFactory.EmailProviderFactoryTests, dotMailify.Core.Tests")]
            public void ShouldNotCreateInstanceOfConfiguredEmailProviderWithTypeNotEmailProvider(string providerType)
            {
                var factory = new DefaultEmailProviderFactory(providerType);
                Assert.Throws<InvalidCastException>(() => factory.GetEmailProvider());
            }
        }
    }
}