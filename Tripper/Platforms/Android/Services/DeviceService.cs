using Android.Content;
using Android.Locations;
using Tripper.Interfaces.Services;

namespace Tripper.Platforms.Android.Services;

public class DeviceService : IDeviceService
{
    public bool GpsServicesEnabled()
    {
        LocationManager? locManager = Platform.AppContext.GetSystemService(Context.LocationService) as LocationManager;
        return locManager.IsProviderEnabled(LocationManager.GpsProvider);
    }
}
