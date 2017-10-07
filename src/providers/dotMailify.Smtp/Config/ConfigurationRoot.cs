
using Microsoft.Extensions.Configuration;

namespace dotMailify.Smtp.Config
{
    /// <summary>
    /// Singleton provider for configuration.
    /// </summary>
    public sealed class ConfigurationRootSingleton
    {
        private static readonly ConfigurationRootSingleton instance = new ConfigurationRootSingleton();

        private ConfigurationRootSingleton()
        {
            new ConfigurationBuilder().AddJsonFile("appsettings.json.config", optional: true).Build();
        }

        public static ConfigurationRootSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        public IConfigurationRoot ConfigurationRoot { get; set; }
    }

}
