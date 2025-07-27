using System;
using System.Windows.Input;
using Jc.Avalonia.Ui.Navigation;
using Jc.Avalonia.Ui.Sample.Views;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationManager _navigationManager;

    public ICommand NavigateCommand { get; set; }
    
    public MainViewModel(INavigationManager navigationManager)
    {
        _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        
        NavigateCommand = ReactiveCommand.Create(Navigate);
    }

    private void Navigate()
    {
        _navigationManager.NavigateTo(new UserControl1(), NavigationMethod.Clear);
    }
}