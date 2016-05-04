using dotMailify.Core.Abstractions.Config;

namespace dotMailify.SendGrid.Abstractions.Config
{
	public interface ISendGridEmailProviderSettings : IEmailProviderSettings
	{
		string ApiKey { get; }
	}
}