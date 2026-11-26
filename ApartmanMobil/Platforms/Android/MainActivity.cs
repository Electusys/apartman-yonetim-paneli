using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ApartmanMobil;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, WindowSoftInputMode = SoftInput.AdjustResize)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (Window is null)
        {
            return;
        }

        Window.SetStatusBarColor(Android.Graphics.Color.Rgb(31, 34, 43));
        Window.SetNavigationBarColor(Android.Graphics.Color.Rgb(31, 34, 43));
    }
}
