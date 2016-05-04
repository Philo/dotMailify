using System.Net.Mail;
using System.Threading.Tasks;
using dotMailify.Core;
using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Abstractions.Message;
using dotMailify.Core.Logging;
using dotMailify.Core.Message;

namespace dotMailify.Smtp.Abstractions
{
	public abstract class SmtpClientMailMessageProvider<TEmailMessage, TSettings> : AbstractEmailProvider<TEmailMessage, TSettings>
		where TEmailMessage : class, IEmailMessage
		where TSettings : IEmailProviderSettings
	{
		protected SmtpClientMailMessageProvider(TSettings settings)
			: base(settings)
		{
		}

        protected virtual MailMessage CreateMailMessage(TEmailMessage message)
		{
			var mailMessage = message.ToMimeMessage();
			return CreateMailMessageCore(mailMessage, message);
		}

		protected virtual MailMessage CreateMailMessageCore(MailMessage mailMessage, TEmailMessage message)
		{
			return mailMessage;
		}

		protected virtual MailAddress CreateMailAddress(EmailAddress recipient)
		{
			return recipient.ToMailAddress();
		}

		protected abstract SmtpClient CreateSmtpClient(TSettings settings);

		protected async override Task SendCore(TEmailMessage message, TSettings settings)
		{
			using (var client = CreateSmtpClient(settings))
			{
				var mailMessage = CreateMailMessage(message);
				await client.SendMailAsync(mailMessage);
			}
		}
	}
}