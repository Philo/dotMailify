using dotMailify.Core.Config;
using dotMailify.Smtp.Abstractions.Config;

namespace dotMailify.Smtp.Config
{
    public class FromConfigSmtpClientProviderSettings : DefaultEmailProviderSettings, ISmtpClientProviderSettings
    {
        public string Host { get; } = GetFromAppSettings("localhost");
        public int Port { get; } = GetFromAppSettings(25);
        public bool EnableSsl { get; } = GetFromAppSettings(false);
        public string Username { get; } = GetFromAppSettings(string.Empty);
        public string Password { get; } = GetFromAppSettings(string.Empty);
    }
}