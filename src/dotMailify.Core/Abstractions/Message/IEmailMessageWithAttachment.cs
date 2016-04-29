using System.Collections.Generic;

namespace dotMailify.Core.Abstractions.Message
{
	public interface IEmailMessageWithAttachment : IEmailMessage
	{
		IEnumerable<IEmailMessageAttachment> Attachments { get; }

		void AddAttachment(IEmailMessageAttachment attachment);
	}
}