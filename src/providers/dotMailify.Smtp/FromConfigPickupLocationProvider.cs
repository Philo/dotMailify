using dotMailify.Smtp.Pickup;

namespace dotMailify.Smtp
{
    public class FromConfigPickupLocationProvider : PickupLocationProvider
    {
        public FromConfigPickupLocationProvider() : base(new FromConfigPickupLocationProviderSettings()) { }
    }
}