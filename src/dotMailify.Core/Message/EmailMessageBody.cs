using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace dotMailify.Core.Message
{
    [DebuggerStepThrough]
    public class EmailMessageBody
	{
		public string MediaType { get; private set; }
		public Stream Content { get; private set; }
		public Encoding Encoding { get; private set; }

		private EmailMessageBody(Stream content, Encoding encoding = null, string mediaType = MediaTypeNames.Text.Plain)
		{
			Content = content;
			Encoding = encoding;
			MediaType = mediaType;
		}

		public static EmailMessageBody FromText(string text, Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.Default;

			var stream = new MemoryStream(encoding.GetBytes(text));
			return new EmailMessageBody(stream, encoding, MediaTypeNames.Text.Plain);
		}

		public static EmailMessageBody FromHtml(string html, Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.Default;

			var stream = new MemoryStream(encoding.GetBytes(html));

			return new EmailMessageBody(stream, encoding, MediaTypeNames.Text.Html);
		}
	}
}