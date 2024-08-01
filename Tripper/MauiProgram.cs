
using Tripper.ViewModels;
using Tripper.Views;

namespace Tripper
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp
                .CreateBuilder()
                .UseMauiApp<App>()
                .UsePrism(
                    new DryIocContainerExtension(),
                    prism => prism.CreateWindow("NavigationPage/Home")
                )
                .UseShiny()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Configuration.AddJsonPlatformBundle();

            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Logging.AddDebug();

            builder.Services.AddConnectivity();
            builder.Services.AddBattery();
            builder.Services.AddGps<Tripper.Delegates.MyGpsDelegate>();
            builder.Services.AddGeofencing<Tripper.Delegates.MyGeofenceDelegate>();

            // MVVM Pages
            builder.Services.RegisterForNavigation<Home, HomeViewModel>();


            var app = builder.Build();

            return app;
        }
    }
}