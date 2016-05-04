using System;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Logging
{
    public class NullEmailLoggingProvider : IEmailLoggingProvider
    {
        public void Log(string message, Exception exception = null)
        {
        }

        public void Sent(IEmailMessage emailMessage, string message = null)
        {
        }

        public void Failed(IEmailMessage emailMessage, Exception exception, string message = null)
        {
        }

        public void Blocked(IEmailMessage emailMessage, string message = null)
        {
        }
    }
}