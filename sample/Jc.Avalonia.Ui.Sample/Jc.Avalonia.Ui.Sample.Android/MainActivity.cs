using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Views;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.Android;

[Activity(
    Label = "Jc.Avalonia.Ui.Sample.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        Window!.ClearFlags(WindowManagerFlags.TranslucentStatus);
        Window!.SetStatusBarColor(Color.Transparent);
        
        return base.CustomizeAppBuilder(builder)
            .WithInterFont()
            .UseReactiveUI();
    }
}