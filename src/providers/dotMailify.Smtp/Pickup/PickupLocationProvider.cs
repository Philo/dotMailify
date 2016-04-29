using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using dotMailify.Core.Message;
using dotMailify.Smtp.Abstractions;

namespace dotMailify.Smtp.Pickup
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

    public class PickupLocationProvider : SmtpClientMailMessageProvider<EmailMessage, IPickupLocationProviderSettings>
	{
		public PickupLocationProvider(IPickupLocationProviderSettings settings)
			: base(settings)
		{
		}

        public PickupLocationProvider() : this(new DefaultPickupLocationProviderSettings()) { }

		protected override SmtpClient CreateSmtpClient()
		{
			var client = new SmtpClient
			{
				EnableSsl = false,
				DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
				PickupDirectoryLocation = Settings.Location
			};

			return client;
		}
	}
}