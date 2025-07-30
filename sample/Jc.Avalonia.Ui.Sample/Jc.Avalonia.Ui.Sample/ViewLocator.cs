using Jc.Avalonia.Ui.Sample.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Jc.Avalonia.Ui.Sample;

public static class ViewLocator
{
    public static MainViewModel MainViewModel => App.Services.GetService<MainViewModel>();
}