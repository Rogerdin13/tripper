using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.ApplicationModel;

namespace Tripper
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        MainLauncher = true,
        ConfigurationChanges =
            ConfigChanges.ScreenSize |
            ConfigChanges.Orientation |
            ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout |
            ConfigChanges.SmallestScreenSize |
            ConfigChanges.Density
    )]
    [IntentFilter(
        new[] {
            Platform.Intent.ActionAppAction,
            global::Android.Content.Intent.ActionView
        },
        Categories = new[] {
            global::Android.Content.Intent.CategoryDefault,
            global::Android.Content.Intent.CategoryBrowsable
        }
    )]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
