using System.IO;
using System.Text;

namespace dotMailify.Core.Abstractions.Message
{
	public interface IEmailMessageAttachment
	{
		Stream Content { get; }
		Encoding Encoding { get; }
		string MediaType { get; }
		string Name { get; }
	}
}