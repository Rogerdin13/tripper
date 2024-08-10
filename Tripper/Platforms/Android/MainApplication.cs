using Android.App;
using Android.Runtime;
using Tripper.Interfaces.Services;
using Tripper.Platforms.Android.Services;

namespace Tripper;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp(RegisterPlafromTypes);

    private void RegisterPlafromTypes(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDeviceService, DeviceService>();
    }
}
