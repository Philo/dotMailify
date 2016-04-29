using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Net.Mime;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Message
{
    [DebuggerStepThrough]
    public class EmailMessageWithAttachment : EmailMessage, IEmailMessageWithAttachment
	{
		public EmailMessageWithAttachment(EmailAddress @from)
			: base(@from)
		{
		}

		public EmailMessageWithAttachment(EmailAddress @from, params EmailAddress[] toRecipients) : base(@from, toRecipients)
		{
		}

		private readonly ISet<IEmailMessageAttachment> _attachments = new HashSet<IEmailMessageAttachment>();
		public IEnumerable<IEmailMessageAttachment> Attachments => _attachments;

	    public void AddAttachment(IEmailMessageAttachment attachment)
		{
			Validate(attachment);
			_attachments.Add(attachment);
		}

		protected virtual void Validate(IEmailMessageAttachment attachment)
		{
            if(attachment?.Content == null) throw new ArgumentNullException(nameof(attachment), Validation.EmailMessageWithAttachment_NoContentSpecified);
        }

		public override MailMessage ToMimeMessage()
		{
			var mailMessage = base.ToMimeMessage();
			foreach (var attachment in Attachments)
			{
				mailMessage.Attachments.Add(new Attachment(attachment.Content, new ContentType(attachment.MediaType)
				{
					CharSet = attachment.Encoding.WebName,
					Name = attachment.Name
				}));
			}
			return mailMessage;
		}

	}
}