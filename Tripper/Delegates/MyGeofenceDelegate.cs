using Shiny.Locations;

namespace Tripper.Delegates
{
    public partial class MyGeofenceDelegate : IGeofenceDelegate
    {
        public MyGeofenceDelegate()
        {
        }


        public Task OnStatusChanged(GeofenceState newStatus, GeofenceRegion region)
        {
            throw new NotImplementedException();
        }
    }


#if ANDROID
    public partial class MyGeofenceDelegate : IAndroidForegroundServiceDelegate
    {
        public void Configure(AndroidX.Core.App.NotificationCompat.Builder builder)
        {
        
        }
    }
#endif
}
