using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Jc.Avalonia.Ui.Extensions.DependencyInjection;
using Jc.Avalonia.Ui.Sample.ViewModels;
using Jc.Avalonia.Ui.Sample.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Jc.Avalonia.Ui.Sample;

public partial class App : Application
{
    internal static IServiceProvider Services { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Services = BuildServiceProvider();
        
        var vm = Services.GetRequiredService<MainViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private IServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddJcUi()
            .AddTransient<MainViewModel>()
            .BuildServiceProvider();
    }
}