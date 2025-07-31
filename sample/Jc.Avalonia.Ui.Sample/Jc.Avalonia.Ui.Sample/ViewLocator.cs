using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Sample.ViewModels;

namespace Jc.Avalonia.Ui.Sample;

public static class ViewLocator
{
    public static IDialogManager DialogManager { get; } = new DialogManager();
    public static MainViewModel MainViewModel => new MainViewModel();
}