using System.Configuration;
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
            var smtpSection = ConfigurationRootSingleton.Instance.ConfigurationRoot.GetSection(
                                "system.net:mailSettings:smtp:");
            if (smtpSection != null)
            {
                Location = smtpSection["SpecifiedPickupDirectory:PickupDirectoryLocation"];
            }
        }

        public string Location { get; private set; }
    }
}