using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Smtp.Pickup
{
	public interface IPickupLocationProviderSettings : IEmailProviderSettings
	{
		string Location { get; }
	}
}