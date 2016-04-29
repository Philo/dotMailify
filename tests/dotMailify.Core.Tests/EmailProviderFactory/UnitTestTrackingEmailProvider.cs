using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotMailify.Core.Abstractions.Config;
using dotMailify.Core.Message;

namespace dotMailify.Core.Tests.EmailProviderFactory
{
    public class UnitTestTrackingEmailProvider : AbstractEmailProvider
    {
        private readonly IList<EmailMessage> _emailMessageList = new List<EmailMessage>();

        public UnitTestTrackingEmailProvider(IEmailProviderSettings settings) : base(settings)
        {
        }

        protected override Task SendCore(EmailMessage message)
        {
            return Task.Run(() => _emailMessageList.Add(message));
        }

        public IEnumerable<EmailMessage> GetSentMessages()
        {
            return _emailMessageList;
        }

        public bool HasMessageWithSubjectLine(string subject)
        {
            return _emailMessageList.Any(x => x.Subject.Equals(subject));
        }

        public bool HasMessageFrom(string address)
        {
            return _emailMessageList.Any(x => x.From.Address.Equals(address));
        }
    }
}