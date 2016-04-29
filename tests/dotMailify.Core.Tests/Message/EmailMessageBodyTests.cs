using System.IO;
using System.Net.Mime;
using System.Text;
using dotMailify.Core.Message;
using FluentAssertions;
using Xunit;

namespace dotMailify.Core.Tests.Message
{
    public class EmailMessageBodyTests
    {
        public class WhenCreatingPlainEmailMessageBody : EmailMessageBodyTests
        {
            [Theory]
            [InlineData("")]
            [InlineData("Hello world")]
            [InlineData("Aenean conubia a aenean amet tellus nunc pulvinar nunc. Aenean dolor euismod. Phasellus nulla voluptate bibendum consectetuer repudiandae a donec arcu. Quisque ac amet. Eleifend massa ipsum eleifend aliquam pede.")]
            public void ShouldCreatePlainTextMessageBody(string bodyText)
            {
                var expectedStream = new MemoryStream(Encoding.Default.GetBytes(bodyText));
                var body = EmailMessageBody.FromText(bodyText);
                body.Should().NotBeNull();
                body.Content.Length.Should().Be(expectedStream.Length);
                body.MediaType.Should().Be(MediaTypeNames.Text.Plain);
            }

            [Theory]
            [InlineData("")]
            [InlineData("<span>Hello world</span>")]
            [InlineData("<b>Aenean</b> conubia a aenean amet tellus nunc pulvinar nunc. Aenean dolor euismod. Phasellus nulla voluptate bibendum consectetuer repudiandae a donec arcu. Quisque ac amet. Eleifend massa ipsum eleifend aliquam pede.")]
            public void ShouldCreateHtmlMessageBody(string bodyText)
            {
                var expectedStream = new MemoryStream(Encoding.Default.GetBytes(bodyText));
                var body = EmailMessageBody.FromHtml(bodyText);
                body.Should().NotBeNull();
                body.Content.Length.Should().Be(expectedStream.Length);
                body.MediaType.Should().Be(MediaTypeNames.Text.Html);
            }
        }
    }
}
