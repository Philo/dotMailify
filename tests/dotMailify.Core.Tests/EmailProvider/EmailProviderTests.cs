using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Message;
using dotMailify.Core.Tests.EmailProviderFactory;
using FluentAssertions;
using Moq;
using Xunit;

namespace dotMailify.Core.Tests.EmailProvider
{
    public class EmailProviderTests
    {
        private readonly UnitTestEmailProviderFactory _factory;
        private readonly Mock<IEmailProviderSettings> _settings;
        private readonly UnitTestTrackingEmailProvider _provider;
        public EmailProviderTests()
        {
            _settings = new Mock<IEmailProviderSettings>();
            _settings.SetupGet(g => g.DisableDelivery).Returns(false);

            _factory = new UnitTestEmailProviderFactory(_settings.Object);
            _provider = (UnitTestTrackingEmailProvider)_factory.GetEmailProvider();
        }

        [Fact]
        public void ShouldSendEmailToTestProvider()
        {
            var message = new EmailMessage("from@localtest.me", "to@localtest.me")
            {
                Subject = "Hello world"
            };
            message.AddBody(EmailMessageBody.FromText("Hello world body"));
            message.AddBody(EmailMessageBody.FromHtml("Hello world body"));
            _provider.Send(message);

            _provider.HasMessageFrom("from@localtest.me").Should().BeTrue();
        }
    }
}
