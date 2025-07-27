using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;

public interface INavigationManager
{
    void NavigateTo<TPage>(TPage page, NavigationMethod method) where TPage : UserControl;
    void GoBack();
}