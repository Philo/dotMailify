using System.Threading.Tasks;
using dotMailify.Core.Abstractions.Message;
using dotMailify.Core.Logging;

namespace dotMailify.Core.Abstractions
{
	public interface IEmailProvider
	{
        IEmailLoggingProvider EmailLoggingProvider { get; set; }

		void Send(IEmailMessage message);
		Task SendAsync(IEmailMessage message);
	}
}