using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;


public class NavigationManager : INavigationManager
{
    private static readonly Lazy<NavigationManager> Instance = new Lazy<NavigationManager>(() => new NavigationManager());
    
    public static NavigationManager Current => Instance.Value;
    
    public void NavigateTo(UserControl page, NavigationMethod method = NavigationMethod.Push)
    {
        NavigationRoot.PushPage(page, method);
    }

    public void GoBack()
    {
        NavigationRoot.PopPage();
    }
}