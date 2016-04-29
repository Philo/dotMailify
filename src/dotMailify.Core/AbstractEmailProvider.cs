using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotMailify.Core.Abstractions;
using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Abstractions.Message;
using dotMailify.Core.Config;
using dotMailify.Core.Message;

namespace dotMailify.Core
{
    public abstract class AbstractEmailProvider : AbstractEmailProvider<EmailMessage, IEmailProviderSettings> {
		protected AbstractEmailProvider(IEmailProviderSettings settings) : base(settings)
		{
		}

        protected AbstractEmailProvider() : this(new FromConfigEmailProviderSettings()) { }
	}

	public abstract class AbstractEmailProvider<TEmailMessage, TEmailProcessorSettings> : IEmailProvider 
		where TEmailMessage : class, IEmailMessage
		where TEmailProcessorSettings : IEmailProviderSettings
	{
		protected TEmailProcessorSettings Settings { get; set; }
        
        protected virtual void Log(string logMessage, Exception exception = null)
        {
            Trace.WriteLine(logMessage);
        }

        protected AbstractEmailProvider(TEmailProcessorSettings settings)
		{
            if(settings == null) throw new ArgumentNullException(nameof(settings), Validation.EmailProvider_SettingsNotSpecified);
			Settings = settings;
		}

		public void Send(IEmailMessage message)
		{
			try
			{
			    var task = Task.Run(() => SendAsync(message));
			    task.Wait();
			}
			catch (AggregateException exception)
			{
				throw exception.GetBaseException();
			}
		}

		public async Task SendAsync(IEmailMessage message)
		{
			Validate(message);
			if (Settings.EnableDelivery)
			{
				await SendCore(message as TEmailMessage);
				// await AuditOutboundMessageCore(message);
			}
			else
			{
                Log($"Email delivery via [{GetType().Name}] of message {message.Subject} did not occur due to configuration [EnableDelivery = false]");
			}
		}

        /*
		protected async virtual Task AuditOutboundMessage(TEmailMessage message, DirectoryInfo bccDirectory)
		{
			using (var client = new SmtpClient
			{
				DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
				EnableSsl = false,
				PickupDirectoryLocation = bccDirectory.FullName
			})
			{
				await client.SendMailAsync(message.ToMimeMessage());
			}
		}

		protected virtual DirectoryInfo GetAuditDirectory(TEmailMessage message, string bccDirectoryPath)
		{
			var bccDirectoryInfo = new DirectoryInfo(bccDirectoryPath);
			var timestamp = DateTime.UtcNow;
			bccDirectoryInfo.Create();
			return bccDirectoryInfo.CreateSubdirectory($@"{timestamp.Year}\{timestamp.Month}\{timestamp.Day}");
		}

		private async Task AuditOutboundMessageCore(IEmailMessage message)
		{
			if (!string.IsNullOrWhiteSpace(Settings.BccToDirectory))
			{
				try
				{
					var emailMessage = message as TEmailMessage;
					var bccDirectory = GetAuditDirectory(emailMessage, Settings.BccToDirectory);
					await AuditOutboundMessage(emailMessage, bccDirectory);
					Log($"Outbound Message logged to {bccDirectory.FullName}");
				}
				catch (Exception ex)
				{
					Log($"Failed to audit email message to {Settings.BccToDirectory}", ex);
				}
			}
		} */

		private void Validate(IEmailMessage message)
		{
            if(message.From == null) throw new ArgumentNullException(nameof(message.From), Validation.FromValueNotSpecified);
            if (message.Recipients == null || !message.Recipients.Any()) throw new ArgumentNullException(nameof(message.Recipients), Validation.NoEmailRecipientsInMessage);
            if(string.IsNullOrWhiteSpace(message.Subject)) throw new ArgumentNullException(nameof(message.Subject), Validation.NoSubjectLineInMessage);
            ValidateCore(message as TEmailMessage);
		}

		protected bool HasAttachments(TEmailMessage message)
		{
			return GetAttachments(message).Any();
		}

		protected IEnumerable<IEmailMessageAttachment> GetAttachments(TEmailMessage message)
		{
			var messageWithAttachments = message as IEmailMessageWithAttachment;
			if (messageWithAttachments != null && messageWithAttachments.Attachments.Any())
			{
				return messageWithAttachments.Attachments;
			}
			return Enumerable.Empty<IEmailMessageAttachment>();
		}

		protected virtual void ValidateCore(TEmailMessage message)
		{
		}

		protected abstract Task SendCore(TEmailMessage message);

		protected string GetBodyText(TEmailMessage message, string mediaType)
		{
			using (var reader = new StreamReader(message.GetBody(mediaType).Content))
			{
				return reader.ReadToEnd();
			}
		}

	}
}