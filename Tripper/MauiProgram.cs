using Prism;
using Tripper.Interfaces.Services;
using Tripper.Services;
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
                    prism => prism.OnInitialized(container =>
                    {
                        var eventAggregator = container.Resolve<IEventAggregator>();
                        eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(context => {
                            var type = context.Type;
                            var wasSuccess = context.Result.Success;

                            if(type == NavigationRequestType.GoBack && wasSuccess)
                            {
                                var ctvm = Application.Current!.Handler.MauiContext!.Services.GetServices<CustomTitleViewModel>().First();
                                ctvm.isLogPage = false;
                            }
                        });
                    })
                    .CreateWindow("NavigationPage/Home")
                )
                .UseShiny()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .RegisterTypes();

            builder.Configuration.AddJsonPlatformBundle();

            builder.Services.AddConnectivity();
            builder.Services.AddBattery();
            builder.Services.AddGps<Tripper.Delegates.MyGpsDelegate>();
            builder.Services.AddGeofencing<Tripper.Delegates.MyGeofenceDelegate>();


            var app = builder.Build();

            return app;
        }

        /// <summary>
        ///     app internals get registered here
        ///     (MVVM bindings, self build services, ...)
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static MauiAppBuilder RegisterTypes(this MauiAppBuilder builder)
        {
            // Services
            builder.Services.AddSingleton<ILoggingService, LoggingService>();

            // MVVM Pages
            builder.Services.RegisterForNavigation<Home, HomeViewModel>();
            builder.Services.RegisterForNavigation<LogPage, LogPageViewModel>();
            builder.Services.RegisterForNavigation<InitializationPage, InitializationPageViewModel>();

            // View VMs
            builder.Services.AddSingleton<CustomTitleViewModel>();

            return builder;
        }
    }
}