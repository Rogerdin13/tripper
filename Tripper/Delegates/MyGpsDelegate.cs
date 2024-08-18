using Shiny.Locations;
using Tripper.Services;

namespace Tripper.Delegates
{
    public partial class MyGpsDelegate : GpsDelegate
    {
        public MyGpsDelegate(ILogger<MyGpsDelegate> logger) : base(logger)
        {
            this.MinimumDistance = Distance.FromMeters(9);
            this.MinimumTime = TimeSpan.FromSeconds(11);
        }


        protected override async Task OnGpsReading(GpsReading reading)
        {
        }
    }

#if ANDROID
    public partial class MyGpsDelegate : IAndroidForegroundServiceDelegate
    {
        public void Configure(AndroidX.Core.App.NotificationCompat.Builder builder)
        {
            builder.SetContentTitle("Tripper")
                   .SetContentText("Listening to locationchanges");
                   //.SetSmallIcon(Resource.Mipmap.youricon);
            
        }
    }
#endif
}
