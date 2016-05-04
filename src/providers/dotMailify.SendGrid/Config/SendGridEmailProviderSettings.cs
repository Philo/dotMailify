using dotMailify.Core.Config;
using dotMailify.SendGrid.Abstractions.Config;

namespace dotMailify.SendGrid.Config
{
	public class SendGridEmailProviderSettings : DefaultEmailProviderSettings, ISendGridEmailProviderSettings
	{
	    public string ApiKey { get; } = GetFromAppSettings(string.Empty);
	}
}