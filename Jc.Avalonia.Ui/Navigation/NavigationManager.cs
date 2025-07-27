using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;


public class NavigationManager : INavigationManager
{
    private static readonly Lazy<NavigationManager> Instance = new Lazy<NavigationManager>(() => new NavigationManager());
    
    public static NavigationManager Current => Instance.Value;
    
    public void NavigateTo<TPage>(TPage page, NavigationMethod method = NavigationMethod.Push) where TPage : UserControl
    {
        NavigationRoot.PushPage(page, method);
    }

    public void GoBack()
    {
        NavigationRoot.PopPage();
    }
}