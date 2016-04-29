using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Smtp
{
	public interface ISmtpClientProviderSettings : IEmailProviderSettings
	{
		string Host { get; }
		int Port { get; }
		bool EnableSsl { get; }
		string Username { get; }
		string Password { get; }
	}
}