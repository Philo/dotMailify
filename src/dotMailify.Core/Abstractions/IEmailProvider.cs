using System.Threading.Tasks;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Abstractions
{
	public interface IEmailProvider
	{
		void Send(IEmailMessage message);
		Task SendAsync(IEmailMessage message);
	}
}