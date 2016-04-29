using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Message
{
	[DebuggerStepThrough]
	public class EmailMessage : IEmailMessage
	{
		public EmailAddress Sender { get; set; }
		public EmailAddress From { get; set; }

		private readonly ISet<EmailAddress> _replyTo = new HashSet<EmailAddress>();
		public IEnumerable<EmailAddress> ReplyTo => _replyTo;

	    private readonly ISet<EmailAddress> _recipients = new HashSet<EmailAddress>();
		public IEnumerable<EmailAddress> Recipients => _recipients;

	    private readonly ISet<EmailAddress> _ccRecipients = new HashSet<EmailAddress>();
		public IEnumerable<EmailAddress> CcRecipients => _ccRecipients;

	    private readonly ISet<EmailAddress> _bccRecipients = new HashSet<EmailAddress>();
		public IEnumerable<EmailAddress> BccRecipients => _bccRecipients;

	    private readonly ISet<EmailMessageBody> _emailBody = new HashSet<EmailMessageBody>();
		public IEnumerable<EmailMessageBody> Body => _emailBody;

	    public string Subject { get; set; }

		public EmailMessageBody GetBody(string mediaType = null)
		{
			return Body.FirstOrDefault(x => x.MediaType == mediaType) ?? Body.FirstOrDefault();
		}

		public virtual MailMessage ToMimeMessage()
		{
			var mailMessage = new MailMessage();
			if (Sender != null) mailMessage.Sender = Sender.ToMailAddress();

			mailMessage.From = From.ToMailAddress();

			foreach (var recipient in Recipients)
			{
				mailMessage.To.Add(recipient.ToMailAddress());
			}
			mailMessage.Subject = Subject;
			AddAlternateViews(mailMessage);

			return mailMessage;
		}

		protected void AddAlternateViews(MailMessage mailMessage)
		{
			foreach (var body in Body)
			{
				if (body.Content != null && body.Content.CanRead)
				{
					mailMessage.AlternateViews.Add(new AlternateView(body.Content, body.MediaType)
					{
						ContentType =
						{
							CharSet = body.Encoding.WebName
						}
					});
				}
			}
		}

		public EmailMessage(EmailAddress @from)
		{
			From = @from;
		}

		public EmailMessage(EmailAddress @from, params EmailAddress[] toRecipients) : this(@from)
		{
			foreach (var recipient in toRecipients)
			{
				AddRecipient(recipient);
			}
		}

		public void AddBody(EmailMessageBody body)
		{
			_emailBody.Add(body);
		}

		public void AddRecipient(EmailAddress recipient, EmailMessageRecipientType recipientType = EmailMessageRecipientType.To)
		{
			ValidateRecipient(recipient, recipientType);
			switch (recipientType)
			{
				case EmailMessageRecipientType.Cc:
					_ccRecipients.Add(recipient);
					break;
				case EmailMessageRecipientType.Bcc:
					_bccRecipients.Add(recipient);
					break;
				case EmailMessageRecipientType.To:
					_recipients.Add(recipient);
					break;
					case EmailMessageRecipientType.ReplyTo:
					_replyTo.Add(recipient);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(recipientType), Validation.EmailMessage_UnknownRecipientType);
			}
		}

		protected virtual void ValidateRecipient(EmailAddress recipient, EmailMessageRecipientType recipientType)
		{
			recipient.Validate();
		}

		public void AddRecipient(string emailAddress, string displayAs = null, EmailMessageRecipientType recipientType = EmailMessageRecipientType.To)
		{
			AddRecipient(new EmailAddress(emailAddress, displayAs), recipientType);
		}
	}
}