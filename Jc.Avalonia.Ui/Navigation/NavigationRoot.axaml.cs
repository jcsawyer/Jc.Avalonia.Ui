using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

namespace Jc.Avalonia.Ui.Navigation;

internal partial class NavigationRoot : UserControl
{
    /// <summary>
    /// The currently displayed page.
    /// This is the page that is currently visible in the shell.
    /// </summary>
    public Control? CurrentPage;
    
    /// <summary>
    /// Page navigation stack.
    /// </summary>
    public Stack<Control> Pages = new Stack<Control>();
    
    public static void PopPage()
    {
        var navigationRoot = GetNavigationRoot();
        
        if (navigationRoot.Pages.Count == 0)
        {
            return;
        }
        
        var page = navigationRoot.Pages.Pop();
     
        navigationRoot.CurrentPage = page;
        navigationRoot.Content = page;
    }
    
    public static void PushPage(UserControl content, NavigationMethod method)
    {
        var navigationRoot = GetNavigationRoot();
        if (navigationRoot.CurrentPage is null && content.Content is not null)
        {
            navigationRoot.CurrentPage = (Control)content.Content;
        }

        switch (method)
        {
            case NavigationMethod.Push:
                navigationRoot.Pages.Push(navigationRoot.CurrentPage);
                break;
            case NavigationMethod.Pop:
                if (navigationRoot.Pages.Count > 0)
                {
                    navigationRoot.Pages.Pop();
                }
                break;
            case NavigationMethod.Replace:
                if (navigationRoot.Pages.Count > 0)
                {
                    navigationRoot.Pages.Pop();
                }
                navigationRoot.Pages.Push(content);
                break;
            case NavigationMethod.Clear:
                navigationRoot.Pages.Clear();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
        navigationRoot.CurrentPage = content;
        navigationRoot.Content = content;
    }
    
    /// <summary>
    /// Gets the root <see cref="NavigationRoot"/> control from the visual tree.
    /// </summary>
    /// <returns>The root <see cref="NavigationRoot"/> control.</returns>
    /// <exception cref="InvalidOperationException">None or more than one <see cref="NavigationRoot"/> exists in the visual tree.</exception>
    internal static NavigationRoot GetNavigationRoot()
    {
        NavigationRoot? shell;
        try
        {
            shell = ((ISingleViewApplicationLifetime)Application.Current!.ApplicationLifetime!).MainView!.GetVisualDescendants().OfType<NavigationRoot>().Single();
        }
        catch
        {
            try
            {
                shell = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow!.GetVisualDescendants().OfType<NavigationRoot>().Single();
            }
            catch
            {
                throw new InvalidOperationException($"A single {nameof(NavigationRoot)} control must exist in the visual tree.");
            }
        }

        return shell;
    }
}