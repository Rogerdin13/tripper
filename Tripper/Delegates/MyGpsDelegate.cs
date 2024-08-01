using Shiny.Locations;

namespace Tripper.Delegates
{
    public partial class MyGpsDelegate : GpsDelegate
    {
        public MyGpsDelegate(ILogger<MyGpsDelegate> logger) : base(logger)
        {
            this.MinimumDistance = Distance.FromMeters(5);
            this.MinimumTime = TimeSpan.FromSeconds(3);
        }


        protected override async Task OnGpsReading(GpsReading reading)
        {
            var logstring1 = $"{reading.Position.Latitude} / {reading.Position.Longitude} - H: {reading.Heading}";
            var logstring2 = $"Accuracy: {reading.PositionAccuracy} - SP: {reading.Speed}";
            var timestamp = reading.Timestamp;
        }
    }

#if ANDROID
    public partial class MyGpsDelegate : IAndroidForegroundServiceDelegate
    {
        public void Configure(AndroidX.Core.App.NotificationCompat.Builder builder)
        {
        
        }
    }
#endif
}
