using System.Net.Mail;
using dotMailify.Core.Logging;
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

		protected sealed override SmtpClient CreateSmtpClient(IPickupLocationProviderSettings settings)
		{
			var client = new SmtpClient
			{
                EnableSsl = false,
				DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
				PickupDirectoryLocation = settings.Location
			};

			return client;
		}
	}
}