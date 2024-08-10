using Foundation;
using Tripper.Interfaces.Services;
using Tripper.Platforms.iOS.Services;
using UIKit;

namespace Tripper;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
        => MauiProgram.CreateMauiApp(RegisterPlafromTypes);

    private void RegisterPlafromTypes(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDeviceService, DeviceService>();
    }
}
