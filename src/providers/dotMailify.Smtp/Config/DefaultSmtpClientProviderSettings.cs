using System.Configuration;
using dotMailify.Core.Config;
using dotMailify.Smtp.Abstractions.Config;

namespace dotMailify.Smtp.Config
{
    public sealed class DefaultSmtpClientProviderSettings : DefaultEmailProviderSettings, ISmtpClientProviderSettings
    {
        public string Host { get; private set; } = "localhost";
        public int Port { get; private set; } = 25;
        public bool EnableSsl { get; private set; } = false;
        public string Username { get; private set; }
        public string Password { get; private set; }

        private void Configure()
        {
            var networkSettings = ConfigurationRootSingleton.Instance.ConfigurationRoot.GetSection("system.net:mailSettings:smtp:Network");
            if (networkSettings != null)
            {
                var enableSSLString = networkSettings["EnableSsl"];
                if (bool.TryParse(enableSSLString, out bool enableSll))
                {
                    EnableSsl = enableSll;
                }
                Host = networkSettings["Host"];
                Username = networkSettings["UserName"];
                Password = networkSettings["Password"];
                var portString = networkSettings["Port"];
                if (int.TryParse(portString, out int port))
                {
                    Port = port;
                }
            }
        }

        public DefaultSmtpClientProviderSettings()
        {
            Configure();
        }
    }
}