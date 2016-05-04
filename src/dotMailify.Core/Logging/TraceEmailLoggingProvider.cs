using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Logging
{
    public class TraceEmailLoggingProvider : IEmailLoggingProvider
    {
        private StringBuilder AsString(IEmailMessage emailMessage, string message = null, Exception exception = null)
        {
            var sb = new StringBuilder();
            if(string.IsNullOrWhiteSpace(message)) sb.AppendLine($"Message: {message}");
            if(exception != null) sb.AppendLine($"Exception: {exception}");
            sb.AppendLine($"----------------------");
            sb.AppendLine($"{nameof(emailMessage.Sender)}: {emailMessage.Sender}");
            sb.AppendLine($"{nameof(emailMessage.From)}: {emailMessage.From}");
            sb.AppendLine($"{nameof(emailMessage.ReplyTo)}: {emailMessage.ReplyTo}");
            sb.AppendLine($"{nameof(emailMessage.Recipients)}: {string.Join(", ", emailMessage.Recipients)}");
            sb.AppendLine($"{nameof(emailMessage.CcRecipients)}: {string.Join(", ", emailMessage.CcRecipients)}");
            sb.AppendLine($"{nameof(emailMessage.BccRecipients)}: {string.Join(", ", emailMessage.BccRecipients)}");
            sb.AppendLine($"{nameof(emailMessage.Subject)}: {emailMessage.Subject}");
            sb.AppendLine($"{nameof(emailMessage.Body)} Segments: {emailMessage.Body.Count()} ({string.Join(", ", emailMessage.Body.Select(x => x.MediaType))})");
            sb.AppendLine($"----------------------");
            return sb;
        }

        public void Log(string message, Exception exception = null)
        {
            if (exception != null)
            {
                Trace.TraceError($"{message} : {exception}");
            }
            else
            {
                Trace.TraceInformation(message);
            }
        }

        public void Sent(IEmailMessage emailMessage, string message = null)
        {
            Trace.TraceInformation(AsString(emailMessage, message).ToString());
        }

        public void Failed(IEmailMessage emailMessage, Exception exception, string message = null)
        {
            Trace.TraceWarning(AsString(emailMessage, message, exception).ToString());
        }

        public void Blocked(IEmailMessage emailMessage, string message = null)
        {
            Trace.TraceWarning(AsString(emailMessage, message).ToString());
        }
    }
}