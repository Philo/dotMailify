using System.Configuration;
using System.Net.Configuration;
using dotMailify.Core.Config;
using dotMailify.Smtp.Abstractions.Config;

namespace dotMailify.Smtp.Pickup.Config
{
    public sealed class DefaultPickupLocationProviderSettings : DefaultEmailProviderSettings, IPickupLocationProviderSettings
    {
        public DefaultPickupLocationProviderSettings()
        {
            Configure();
        }

        private void Configure()
        {
            var settings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (settings != null)
            {
                Location = settings.SpecifiedPickupDirectory?.PickupDirectoryLocation;
            }
        }

        public string Location { get; private set; }
    }
}