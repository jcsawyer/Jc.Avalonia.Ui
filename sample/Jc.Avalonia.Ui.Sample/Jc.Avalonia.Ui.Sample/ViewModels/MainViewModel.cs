using System;
using System.Windows.Input;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;
using Jc.Avalonia.Ui.Sample.Views;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationManager _navigationManager;
    private readonly IDialogManager _dialogManager;
    
    public ICommand AddCommand { get; }
    public ICommand NavigateCommand { get; set; }
    
    public MainViewModel(INavigationManager navigationManager, IDialogManager dialogManager)
    {
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        _dialogManager = dialogManager ?? throw new ArgumentNullException(nameof(dialogManager));

        AddCommand = ReactiveCommand.Create(Add);
        NavigateCommand = ReactiveCommand.Create(Navigate);
    }

    private void Add()
    {
        _dialogManager.OpenSheet(new SheetContent());
    }

    private void Navigate()
    {
        _navigationManager.NavigateTo(new UserControl1(), NavigationMethod.Clear);
    }
}