namespace Jc.Avalonia.Ui.Navigation;

public interface INavigationManager
{
    event EventHandler<string>? OnNavigated;

    internal void RegisterRoute(string route, ShellContent content, NavigationElementType type);
    internal bool TryGetElement(string route, out NavigationElement? content);
    
    internal void RegisterShell(Shell shell);
    
    Task NavigateAsync(string route, NavigationMethod method = NavigationMethod.Push, CancellationToken cancel = default);
}