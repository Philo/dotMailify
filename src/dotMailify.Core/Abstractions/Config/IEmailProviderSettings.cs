namespace dotMailify.Core.Abstractions.Config
{
	public interface IEmailProviderSettings
	{
		bool EnableDelivery { get; }

		// string BccToDirectory { get; }
	}
}