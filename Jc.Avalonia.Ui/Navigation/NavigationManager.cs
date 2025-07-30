using Avalonia;
using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Navigation;

public class NavigationManager : INavigationManager
{
    internal NavigationManager()
    {
    }

    private static readonly Lazy<NavigationManager> Instance =
        new Lazy<NavigationManager>(() => new NavigationManager());

    public static INavigationManager Current => Instance.Value;

    private object? _currentView;

    private static readonly Stack<NavigationElement> NavigationStack = new Stack<NavigationElement>();

    public static readonly Uri RootUri = new Uri(
        $"app://{(Application.Current?.Name is not { Length: > 0 } appName ? "JcApp" : appName.Replace(" ", "-").ToLowerInvariant())}");

    private Dictionary<string, NavigationElement> _routes { get; } =
        new Dictionary<string, NavigationElement>(StringComparer.InvariantCultureIgnoreCase);

    private Shell _shell;

    public event EventHandler<string>? OnNavigated;

    public Uri CurrentUri { get; private set; } = RootUri;

    void INavigationManager.RegisterRoute(string route, ShellContent content, NavigationElementType type)
    {
        route = route.ToLowerInvariant();
        var newUri = new Uri(RootUri, route);
        if (newUri.AbsolutePath == RootUri.AbsolutePath)
        {
            throw new InvalidOperationException("Cannot re-register root navigation element.");
        }

        if (_routes.ContainsKey(newUri.AbsolutePath))
        {
            throw new InvalidOperationException("Navigation element already registered.");
        }

        var element = new NavigationElement
        {
            Route = route,
            Page = content.ContentTemplate ?? content.GetType(),
            Type = type,
            Title = content.Title,
            Icon = content.Icon,
        };

        if (content is TabBarButton tabBarButton)
        {
            element.Command = tabBarButton.Command;
        }

        var parentPath = newUri.AbsolutePath.EndsWith("/")
            ? (new Uri(newUri, "..")).AbsolutePath.TrimEnd('/')
            : (new Uri(newUri, ".")).AbsolutePath.TrimEnd('/');
        parentPath = parentPath.Length > 0 ? parentPath : "/";

        if (parentPath != "/")
        {
            if (!_routes.TryGetValue(parentPath, out var parentElement))
            {
                throw new InvalidOperationException($"Parent route '{parentPath}' not found for '{route}'.");
            }

            parentElement.AddElement(element);
        }

        _routes[newUri.AbsolutePath] = element;
    }

    bool INavigationManager.TryGetElement(string route, out NavigationElement? content)
    {
        return _routes.TryGetValue(route.ToLowerInvariant(), out content);
    }

    void INavigationManager.RegisterShell(Shell shell)
    {
        _shell = shell ?? throw new ArgumentNullException(nameof(shell));
    }

    public async Task NavigateAsync(string route, NavigationMethod method = NavigationMethod.Push,
        CancellationToken cancel = default)
    {
        var originalUri = new Uri(CurrentUri, route);
        if (!_routes.TryGetValue(route.ToLowerInvariant(), out var element))
        {
            throw new InvalidOperationException($"Route '{originalUri.AbsolutePath}' not found.");
        }

        var isHostable = element.Type == NavigationElementType.Page &&
                         element.Parent is { Type: NavigationElementType.Host };

        // TODO make this DI friendly
        var viewLocator = new DefaultNavigationViewLocator();
        var view = viewLocator.LocateView(element);

        var oldView = _currentView;
        if (isHostable)
        {
            var host = new TabContent { Content = view, DataContext = element.Parent.Children };
            await _shell.AddViewAsync(host, cancel);

            if (_currentView is not TabContent)
            {
                _currentView = host;
            }
        }
        else
        {
            await _shell.AddViewAsync(view, cancel);
            _currentView = view;
        }

        if (oldView is not null && !(oldView is TabContent && isHostable))
        {
            await _shell.RemoveViewAsync(oldView, cancel);
        }
    }
}