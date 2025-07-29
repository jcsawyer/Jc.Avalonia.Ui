using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;

public interface INavigationManager
{
    event EventHandler<string>? OnNavigated;
    
    string CurrentPage { get; }
    void NavigateTo(UserControl page, NavigationMethod method);
    void GoBack();
}