using System.Configuration;
using System.Net.Configuration;
using dotMailify.Smtp.Abstractions.Config;

namespace dotMailify.Smtp.Pickup.Config
{
    public sealed class DefaultPickupLocationProviderSettings : IPickupLocationProviderSettings
    {
        private void ConfigureFromMailAppSettings()
        {
            var settings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (settings != null)
            {
                Location = settings.SpecifiedPickupDirectory?.PickupDirectoryLocation;
            }
        }

        public DefaultPickupLocationProviderSettings()
        {
            ConfigureFromMailAppSettings();
        }

        public bool EnableDelivery { get; } = false;
        public string Location { get; private set; }
    }
}