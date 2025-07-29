using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;


public class NavigationManager : INavigationManager
{
    internal NavigationManager() {}
    
    private static readonly Lazy<NavigationManager> Instance = new Lazy<NavigationManager>(() => new NavigationManager());
    
    public static NavigationManager Current => Instance.Value;

    public event EventHandler<string>? OnNavigated;

    public string CurrentPage { get; private set; }

    public void NavigateTo(UserControl page, NavigationMethod method = NavigationMethod.Push)
    {
        NavigationRoot.PushPage(page, method);
        var pageName = page.GetType().Name.Replace("Page", string.Empty).Replace("View", string.Empty);
        OnNavigated?.Invoke(this, pageName);
        CurrentPage = pageName;
    }

    public void GoBack()
    {
        var page = NavigationRoot.PopPage();
        if (page is not null)
        {
            var pageName = page.GetType().Name.Replace("Page", string.Empty).Replace("View", string.Empty);
            OnNavigated?.Invoke(this, pageName);
            CurrentPage = pageName;
        }
    }
}