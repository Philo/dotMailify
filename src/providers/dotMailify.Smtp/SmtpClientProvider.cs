using System.Net;
using System.Net.Mail;
using dotMailify.Core.Message;
using dotMailify.Smtp.Abstractions;
using dotMailify.Smtp.Abstractions.Config;
using dotMailify.Smtp.Config;

namespace dotMailify.Smtp
{
    public class SmtpClientProvider : SmtpClientMailMessageProvider<EmailMessage, ISmtpClientProviderSettings>
	{
        public SmtpClientProvider() : this(new DefaultSmtpClientProviderSettings()) { }

		public SmtpClientProvider(ISmtpClientProviderSettings settings) : base(settings)
		{
		}

		protected override SmtpClient CreateSmtpClient()
		{
			var client = new SmtpClient
			{
				Host = Settings.Host,
				Port = Settings.Port,
				EnableSsl = Settings.EnableSsl
			};

			if (!string.IsNullOrWhiteSpace(Settings.Username))
			{
				client.Credentials = new NetworkCredential(Settings.Username, Settings.Password);
			}

			return client;
		}
	}
}