using dotMailify.Smtp.Config;
using FluentAssertions;
using Xunit;

namespace dotMailify.Core.Tests.Smtp
{
    public class DefaultSmtpClientProviderSettingsTests
    {
        [Fact]
        public void ShouldReadMailSettingsConfiguration()
        {
            var settings = new DefaultSmtpClientProviderSettings();

            settings.EnableSsl.Should().BeTrue();
            settings.Host.Should().Be("localtest.me");
            settings.Port.Should().Be(2525);
            settings.Username.Should().Be("user");
            settings.Password.Should().Be("pass");
        } 
    }
}