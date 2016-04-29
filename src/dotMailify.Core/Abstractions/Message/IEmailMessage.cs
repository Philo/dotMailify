using System.Collections.Generic;
using System.Net.Mail;
using dotMailify.Core.Message;

namespace dotMailify.Core.Abstractions.Message
{
	public interface IEmailMessage
	{
		EmailAddress Sender { get; set; }
		EmailAddress From { get; set; }
		IEnumerable<EmailAddress> ReplyTo { get; }
		IEnumerable<EmailAddress> Recipients { get; }
		IEnumerable<EmailAddress> CcRecipients { get; }
		IEnumerable<EmailAddress> BccRecipients { get; }
		IEnumerable<EmailMessageBody> Body { get; }
		string Subject { get; set; }

		EmailMessageBody GetBody(string mediaType = null);
		MailMessage ToMimeMessage();
	}
}