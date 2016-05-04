using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using dotMailify.Core;
using dotMailify.Core.Message;
using dotMailify.SendGrid.Abstractions.Config;
using SendGrid;

namespace dotMailify.SendGrid
{
	public class SendGridEmailProvider : AbstractEmailProvider<EmailMessage, ISendGridEmailProviderSettings>
    {
		public SendGridEmailProvider(ISendGridEmailProviderSettings settings) : base(settings)
		{
		}
        
	    protected sealed override async Task SendCore(EmailMessage message, ISendGridEmailProviderSettings settings)
	    {
	        var sendGridMessage = new SendGridMessage
	        {
	            From = message.From,
	            Subject = message.Subject,
	            To = message.Recipients.Select<EmailAddress, MailAddress>(x => x).ToArray(),
	            Cc = message.CcRecipients.Select<EmailAddress, MailAddress>(x => x).ToArray(),
                Bcc = message.BccRecipients.Select<EmailAddress, MailAddress>(x => x).ToArray(),
                Html = GetBodyText(message, MediaTypeNames.Text.Html),
	            Text = GetBodyText(message, MediaTypeNames.Text.Plain)
	        };


	        foreach (var attachment in GetAttachments(message))
		    {
			    sendGridMessage.AddAttachment(attachment.Content, attachment.Name);
		    }

		    var web = new Web(settings.ApiKey);
		    await web.DeliverAsync(sendGridMessage);
	    }
    }
}
