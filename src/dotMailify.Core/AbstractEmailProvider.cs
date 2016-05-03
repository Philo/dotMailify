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
    internal class DisabledEmailProvider : AbstractEmailProvider
    {
        protected override Task SendCore(EmailMessage message)
        {
            return Task.Run(() =>
            {
                Trace.WriteLine("Email delivery has been disabled, your email has not been sent");
            });
        }
    }

    public abstract class AbstractEmailProvider : AbstractEmailProvider<EmailMessage, IEmailProviderSettings> {
		protected AbstractEmailProvider(IEmailProviderSettings settings) : base(settings)
		{
		}

        protected AbstractEmailProvider() : this(new DefaultEmailProviderSettings()) { }
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
			if (!Settings.DisableDelivery)
			{
				await SendCore(message as TEmailMessage);
			}
			else
			{
                Log($"Email delivery via [{GetType().Name}] of message {message.Subject} did not occur due to configuration [EnableDelivery = false]");
			}
		}

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