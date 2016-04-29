using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;
using dotMailify.Core.Abstractions.Message;

namespace dotMailify.Core.Message
{
    [DebuggerStepThrough]
    public class EmailMessageAttachment : IEmailMessageAttachment
	{
		public Stream Content { get; private set; }
		public Encoding Encoding { get; private set; }
		public string MediaType { get; private set; }
		public string Name { get; private set; }

        /// <summary>
        /// Create email attachment
        /// </summary>
        /// <param name="attachment">attachment stream</param>
        /// <param name="encoding">encoding used for the <paramref name="attachment"/></param>
        /// <param name="name">name of the <paramref name="attachment"/></param>
        /// <param name="mediaType">media type taken from <see cref="MediaTypeNames.Application"/></param>
        /// <returns></returns>
		public static IEmailMessageAttachment Create(Stream attachment, Encoding encoding, string name, string mediaType = MediaTypeNames.Application.Octet)
		{
			return new EmailMessageAttachment
			{
				Content = attachment,
				MediaType = mediaType,
				Name = name,
				Encoding = encoding
			};
		}
	}
}