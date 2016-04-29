using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Smtp.Abstractions.Config
{
	public interface IPickupLocationProviderSettings : IEmailProviderSettings
	{
		string Location { get; }
	}
}