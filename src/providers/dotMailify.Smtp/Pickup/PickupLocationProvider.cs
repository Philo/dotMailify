using System.Net.Mail;
using dotMailify.Core.Message;
using dotMailify.Smtp.Abstractions;
using dotMailify.Smtp.Abstractions.Config;
using dotMailify.Smtp.Pickup.Config;

namespace dotMailify.Smtp.Pickup
{
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
				DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
				PickupDirectoryLocation = Settings.Location
			};

			return client;
		}
	}
}