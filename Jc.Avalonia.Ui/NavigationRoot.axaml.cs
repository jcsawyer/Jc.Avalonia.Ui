using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;

namespace Jc.Avalonia.Ui;

public partial class NavigationRoot : UserControl
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
    
    public NavigationRoot()
    {
    }
    
    public static void PushPage(UserControl content, bool clearStack = false)
    {
        var navigationRoot = GetNavigationRoot();

        if (navigationRoot.CurrentPage is null && content.Content is not null)
        {
            navigationRoot.CurrentPage = (Control)content.Content;
        }
        
        navigationRoot.Pages.Push(navigationRoot.CurrentPage);
        if (clearStack)
        {
            navigationRoot.Pages.Clear();
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