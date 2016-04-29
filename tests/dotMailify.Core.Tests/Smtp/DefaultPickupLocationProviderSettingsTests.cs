using dotMailify.Smtp.Pickup.Config;
using FluentAssertions;
using Xunit;

namespace dotMailify.Core.Tests.Smtp
{
    [Trait("Category", "Integration")]
    public class DefaultPickupLocationProviderSettingsTests
    {
        [Fact]
        public void ShouldReadMailSettingsConfiguration()
        {
            var settings = new DefaultPickupLocationProviderSettings();

            settings.Location.Should().Be(@"some\directory");
        }
    }
}