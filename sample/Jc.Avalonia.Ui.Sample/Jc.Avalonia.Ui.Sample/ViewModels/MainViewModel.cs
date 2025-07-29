using System.Windows.Input;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Sample.Views;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{
    public static ICommand AddCommand { get; } = ReactiveCommand.Create(Add);
    
    public MainViewModel()
    {
    }

    private static void Add()
    {
        new DialogManager().OpenSheet(new SheetContent());
    }
}