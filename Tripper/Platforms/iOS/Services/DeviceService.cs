using CoreLocation;
using Tripper.Interfaces.Services;

namespace Tripper.Platforms.iOS.Services;

public class DeviceService : IDeviceService
{
    public bool GpsServicesEnabled()
    {
        return CLLocationManager.LocationServicesEnabled;
    }
}
