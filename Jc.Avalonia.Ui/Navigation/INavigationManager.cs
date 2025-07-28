using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;

public interface INavigationManager
{
    void NavigateTo(UserControl page, NavigationMethod method);
    void GoBack();
}