using System.Net;
using System.Net.Mail;
using dotMailify.Core.Abstractions.Message;
using dotMailify.Core.Config;
using dotMailify.Core.Message;
using dotMailify.Smtp.Abstractions;
using dotMailify.Smtp.Abstractions.Config;
using dotMailify.Smtp.Config;

namespace dotMailify.Smtp
{
    public abstract class SmtpClientProvider<TEmailMessage, TSmtpClientProviderSettings> : SmtpClientMailMessageProvider<TEmailMessage, TSmtpClientProviderSettings>
        where TEmailMessage : class, IEmailMessage
        where TSmtpClientProviderSettings : ISmtpClientProviderSettings
    {
        protected SmtpClientProvider(TSmtpClientProviderSettings settings) : base(settings)
        {
        }

        protected sealed override SmtpClient CreateSmtpClient(TSmtpClientProviderSettings settings)
        {
            var client = new SmtpClient
            {
                Host = settings.Host,
                Port = settings.Port,
                EnableSsl = settings.EnableSsl
            };

            if (!string.IsNullOrWhiteSpace(settings.Username))
            {
                client.Credentials = new NetworkCredential(settings.Username, settings.Password);
            }

            return client;
        }
    }

    public class SmtpClientProvider : SmtpClientProvider<EmailMessage, ISmtpClientProviderSettings>
	{
        public SmtpClientProvider() : this(new DefaultSmtpClientProviderSettings()) { }

		public SmtpClientProvider(ISmtpClientProviderSettings settings) : base(settings)
		{
		}
	}
}