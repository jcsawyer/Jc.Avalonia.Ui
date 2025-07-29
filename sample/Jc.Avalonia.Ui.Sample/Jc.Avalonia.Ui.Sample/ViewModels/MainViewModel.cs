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
    public ICommand NavigateDiary { get; set; }
    public ICommand NavigateInsights { get; set; }
    
    public MainViewModel(INavigationManager navigationManager, IDialogManager dialogManager)
    {
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        _dialogManager = dialogManager ?? throw new ArgumentNullException(nameof(dialogManager));

        AddCommand = ReactiveCommand.Create(Add);
        NavigateDiary = ReactiveCommand.Create(() => Navigate("Diary"));
        NavigateInsights = ReactiveCommand.Create(() => Navigate("Insights"));
    }

    private void Add()
    {
        _dialogManager.OpenSheet(new SheetContent());
    }

    private void Navigate(string pageName)
    {
        switch (pageName)
        {
            case "Diary":
                _navigationManager.NavigateTo(new DiaryPage(), NavigationMethod.Push);
                break;
            case "Insights":
                _navigationManager.NavigateTo(new Insights(), NavigationMethod.Push);
                break;
        }
    }
}