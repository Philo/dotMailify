using System;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Logging
{
    public interface IEmailLoggingProvider
    {
        void Log(string message, Exception exception = null);
        void Sent(IEmailMessage emailMessage, string message = null);
        void Failed(IEmailMessage emailMessage, Exception exception, string message = null);
        void Blocked(IEmailMessage emailMessage, string message = null);
    }
}
