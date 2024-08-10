using Prism;
using Tripper.Interfaces.Services;
using Tripper.Services;
using Tripper.ViewModels;
using Tripper.Views;

namespace Tripper
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp(Action<MauiAppBuilder>? registerPflatformServices = null)
        {
            var builder = MauiApp
                .CreateBuilder()
                .UseMauiApp<App>()
                .UsePrism(
                    new DryIocContainerExtension(),
                    prism => prism.OnInitialized(container =>
                    {
                        // listen to navigation
                        var eventAggregator = container.Resolve<IEventAggregator>();
                        eventAggregator.GetEvent<NavigationRequestEvent>().Subscribe(context => {
                            var type = context.Type;
                            var wasSuccess = context.Result.Success;

                            // handle titleView tripwire manually since this navigation stuff works too weirdly -.-
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
                .RegisterTypes()
                .RegisterPlatformServices(registerPflatformServices);

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

            // View VMs
            builder.Services.AddSingleton<CustomTitleViewModel>();

            return builder;
        }

        private static MauiAppBuilder RegisterPlatformServices(this MauiAppBuilder builder, Action<MauiAppBuilder>? registerPflatformServices)
        {
            registerPflatformServices?.Invoke(builder);

            return builder;
        }
    }
}