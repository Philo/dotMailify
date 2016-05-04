using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Abstractions.Message;
using dotMailify.Core.Logging;
using dotMailify.Core.Message;
using FluentAssertions;
using Xunit;

namespace dotMailify.Core.Tests.EmailProviderFactory
{
    public class EmailProviderFactoryTests
    {
        public class StubEmailProvider : AbstractEmailProvider
        {
            protected override Task SendCore(EmailMessage message, IEmailProviderSettings settings)
            {
                Log("fake message");
                return null;
            }
        }

        public class StubEmailLoggingProvider : IEmailLoggingProvider
        {
            public int LogCallCount { get; set; }
            public int SentCallCount { get; set; }
            public int FailedCallCount { get; set; }
            public int BlockedCallCount { get; set; }

            public void Reset()
            {
                LogCallCount = 0;
                SentCallCount = 0;
                FailedCallCount = 0;
                BlockedCallCount = 0;
            }

            public void Log(string message, Exception exception = null)
            {
                LogCallCount++;
            }

            public void Sent(IEmailMessage emailMessage, string message = null)
            {
                SentCallCount++;
            }

            public void Failed(IEmailMessage emailMessage, Exception exception, string message = null)
            {
                FailedCallCount++;
            }

            public void Blocked(IEmailMessage emailMessage, string message = null)
            {
                BlockedCallCount++;
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

            [Fact]
            public void ShouldConfigureNullLoggingProviderIfNotSpecified()
            {
                var factory = new DefaultEmailProviderFactory(typeof(StubEmailProvider).FullName);
                factory.EmailLoggingProvider.Should().BeOfType<NullEmailLoggingProvider>();
            }

            [Fact]
            public async Task ShouldConfigureSpecifiedEmailLoggingProviderOnEmailProvider()
            {
                var factory = new DefaultEmailProviderFactory(typeof(StubEmailProvider).AssemblyQualifiedName);
                var loggingProvider = new StubEmailLoggingProvider();
                factory.EmailLoggingProvider = loggingProvider;

                var provider = factory.GetEmailProvider();

                var emailMessage = new EmailMessage("test@localtest.me", "test@localtest.me")
                {
                    Subject = "Test"
                };
                emailMessage.AddBody(EmailMessageBody.FromText("test"));
                await provider.SendAsync(emailMessage);
                loggingProvider.LogCallCount.Should().Be(1);
            }
        }
    }
}