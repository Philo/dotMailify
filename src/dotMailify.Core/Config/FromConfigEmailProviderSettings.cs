using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Core.Config
{
    public class FromConfigEmailProviderSettings : IEmailProviderSettings
    {
        private const string Prefix = "dotMailify:";

        public bool EnableDelivery => GetFromAppSettings(false);
        public string BccToDirectory => GetFromAppSettings();
        public string Provider => GetFromAppSettings();

        private static string GetFromAppSettings(string @default = null, string prefix = Prefix, [CallerMemberName] string memberName = "")
        {
            return GetAppSetting(memberName, @default, prefix);
        }

        private static T GetFromAppSettings<T>(T @default = default(T), string prefix = Prefix, 
            [CallerMemberName] string memberName = "")
        {
            return GetAppSetting(memberName, @default, prefix);
        }

        private static T GetAppSetting<T>(string key, T @default = default(T), string prefix = Prefix)
        {
            var keyToRead = string.IsNullOrWhiteSpace(prefix) ? key : $"{prefix}{key}";
            var value = ConfigurationManager.AppSettings.Get(keyToRead);
            if (string.IsNullOrWhiteSpace(value))
            {
                return @default;
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}