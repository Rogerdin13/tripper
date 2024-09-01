using Shiny.Locations;
using Tripper.Interfaces.Services;

namespace Tripper
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var logger = ContainerLocator.Container.Resolve<ILoggingService>();
                logger.Log($"  UNHANDLED  ::  {((Exception)e.ExceptionObject).Message}");
            };

            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                var logger = ContainerLocator.Container.Resolve<ILoggingService>();
                logger.Log($"  UNOBSERVED  ::  {e.Exception.Message}");

                // Prevent the exception from being re-thrown
                e.SetObserved();
            };
        }
    }
}
