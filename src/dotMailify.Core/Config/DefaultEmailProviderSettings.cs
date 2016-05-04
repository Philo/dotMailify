using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using dotMailify.Core.Abstractions;
using dotMailify.Core.Abstractions.Config;

namespace dotMailify.Core.Config
{
    public class DefaultEmailProviderSettings : IEmailProviderSettings
    {
        public virtual bool DisableDelivery { get; } = GetFromAppSettings(true);

        protected static string GetFromAppSettings(string @default = null, string prefix = Constants.Settings.SettingsPrefix, [CallerMemberName] string memberName = "")
        {
            return GetAppSetting(memberName, @default, prefix);
        }

        protected static T GetFromAppSettings<T>(T @default = default(T), string prefix = Constants.Settings.SettingsPrefix, 
            [CallerMemberName] string memberName = "")
        {
            return GetAppSetting(memberName, @default, prefix);
        }

        protected static T GetAppSetting<T>(string key, T @default = default(T), string prefix = Constants.Settings.SettingsPrefix)
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