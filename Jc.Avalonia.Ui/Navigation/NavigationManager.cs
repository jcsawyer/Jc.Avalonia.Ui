using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;


public class NavigationManager : INavigationManager
{
    public void NavigateTo<TPage>(TPage page, NavigationMethod method = NavigationMethod.Push) where TPage : UserControl
    {
        NavigationRoot.PushPage(page, method);
    }

    public void GoBack()
    {
        NavigationRoot.PopPage();
    }
}