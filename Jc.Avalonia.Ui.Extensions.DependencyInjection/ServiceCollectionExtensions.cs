using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace Jc.Avalonia.Ui.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJcUi(this IServiceCollection services)
    {
        services.AddSingleton<INavigationManager, NavigationManager>(_ => NavigationManager.Current)
            .AddSingleton<IDialogManager, DialogManager>();
        return services;
    }
}