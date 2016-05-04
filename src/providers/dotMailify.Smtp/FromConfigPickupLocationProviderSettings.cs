using dotMailify.Core.Config;
using dotMailify.Smtp.Abstractions.Config;

namespace dotMailify.Smtp
{
    public class FromConfigPickupLocationProviderSettings : DefaultEmailProviderSettings, IPickupLocationProviderSettings
    {
        public string Location { get; } = GetFromAppSettings(string.Empty);
    }
}