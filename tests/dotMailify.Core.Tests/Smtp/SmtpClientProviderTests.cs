using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dotMailify.Core.Message;
using dotMailify.Smtp;
using dotMailify.Smtp.Abstractions.Config;
using dotMailify.Smtp.Pickup;
using FluentAssertions;
using Moq;
using nDumbster.Smtp;
using Xunit;

namespace dotMailify.Core.Tests.Smtp
{
    public class SmtpPickupLocationProviderTests : IDisposable
    {
        private readonly DirectoryInfo _tempDirectory;

        public SmtpPickupLocationProviderTests()
        {
            _tempDirectory = new DirectoryInfo($"{Path.GetTempPath()}/{Path.GetRandomFileName()}");
            _tempDirectory.Create();
        }

        [Fact]
        public async Task ShouldDeliverToPickupLocation()
        {
            var settings = new Mock<IPickupLocationProviderSettings>();
            settings.SetupGet(s => s.DisableDelivery).Returns(false);
            settings.SetupGet(s => s.Location).Returns(_tempDirectory.FullName);

            var provider = new PickupLocationProvider(settings.Object);
            var message = new EmailMessage("from@localtest.me", "to@localtest.me")
            {
                Subject = "Unit test"
            };
            message.AddBody(EmailMessageBody.FromText("Hello world"));
            await provider.SendAsync(message);
            _tempDirectory.EnumerateFiles("*.eml", SearchOption.TopDirectoryOnly).Count().Should().Be(1);
            var emlFile = _tempDirectory.EnumerateFiles("*.eml", SearchOption.TopDirectoryOnly).First();
            var emlContent = File.ReadAllText(emlFile.FullName);
            emlContent.Should().Contain("from@localtest.me");
            emlContent.Should().Contain("to@localtest.me");
            emlContent.Should().Contain("Unit test");
        }

        public void Dispose()
        {
            _tempDirectory.Delete(true);
        }
    }

    public class SmtpClientProviderTests : IDisposable
    {
        private readonly SimpleSmtpServer _server;
        private readonly Mock<ISmtpClientProviderSettings> _settings;

        public SmtpClientProviderTests()
        {
            _server = SimpleSmtpServer.Start(64666);
            _settings = new Mock<ISmtpClientProviderSettings>();
            _settings.SetupGet(s => s.DisableDelivery).Returns(false);
            _settings.SetupGet(s => s.Host).Returns("localhost");
            _settings.SetupGet(s => s.Port).Returns(_server.Port);
        }

        [Fact]
        public async Task ShouldSendEmailUsingSmtp()
        {
            var provider = new SmtpClientProvider(_settings.Object);
            var message = new EmailMessage("from@localtest.me", "to@localtest.me")
            {
                Subject = "Unit test"
            };
            message.AddBody(EmailMessageBody.FromText("Hello world"));
            await provider.SendAsync(message);

            _server.ReceivedEmailCount.Should().Be(1);
            var receivedMessage = _server.ReceivedEmail.First();
            receivedMessage.Subject.Should().Be("Unit test");
            receivedMessage.Body.Should().Be("Hello world");
            receivedMessage.IsBodyHtml.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldSendEmailWithAttachmentUsingSmtp()
        {
            var provider = new SmtpClientProvider(_settings.Object);
            var message = new EmailMessageWithAttachment("from@localtest.me", "to@localtest.me")
            {
                Subject = "Unit test"
            };

            message.AddBody(EmailMessageBody.FromText("Hello world"));
            var textStream = new MemoryStream();
            using (var wtr = new StreamWriter(textStream))
            {
                wtr.WriteLine("Hello world attachment");
                wtr.Flush();
                message.AddAttachment(EmailMessageAttachment.Create(textStream, Encoding.UTF8, "test.txt", "plain/text"));
                await provider.SendAsync(message);
            }

            _server.ReceivedEmailCount.Should().Be(1);
            var receivedMessage = _server.ReceivedEmail.First();
            receivedMessage.Subject.Should().Be("Unit test");
            receivedMessage.Body.Should().Be("Hello world");
            receivedMessage.IsBodyHtml.Should().BeFalse();

            receivedMessage.Attachments.Count.Should().Be(1);
            receivedMessage.Attachments.First().Name.Should().Be("test.txt");
            receivedMessage.Attachments.First().ContentType.MediaType.Should().Be("plain/text");
        }

        public void Dispose()
        {
            _server.Stop();
        }
    }
}