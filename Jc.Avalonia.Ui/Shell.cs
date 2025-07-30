using System.Collections.Specialized;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Platform;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui;

[TemplatePart("PART_ContentPresenter", typeof(NavigationRoot))]
public class Shell : TemplatedControl
{
    NavigationRoot? _navigationRoot;

    public static readonly StyledProperty<Thickness> SafeAreaPaddingProperty =
        AvaloniaProperty.Register<Shell, Thickness>(
            nameof(SafeAreaPadding));

    public Thickness SafeAreaPadding
    {
        get => GetValue(SafeAreaPaddingProperty);
        set => SetValue(SafeAreaPaddingProperty, value);
    }

    public static readonly StyledProperty<IPageTransition> PageTransitionProperty =
        AvaloniaProperty.Register<Shell, IPageTransition>(
            nameof(PageTransition), defaultValue: new CrossFade(TimeSpan.FromSeconds(0.25)));

    public static readonly StyledProperty<Thickness> BottomPaddingProperty = AvaloniaProperty.Register<Shell, Thickness>(
        nameof(BottomPadding));

    public Thickness BottomPadding
    {
        get => GetValue(BottomPaddingProperty);
        set => SetValue(BottomPaddingProperty, value);
    }
    
    public IPageTransition PageTransition
    {
        get => GetValue(PageTransitionProperty);
        set => SetValue(PageTransitionProperty, value);
    }

    [Content] public AvaloniaList<IShellItem> Items { get; private set; } = new AvaloniaList<IShellItem>();

    public Shell()
    {
        Items.CollectionChanged += ItemsOnCollectionChanged;

        NavigationManager.Current.RegisterShell(this);
    }

    private void ItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        foreach (var item in e.NewItems.Cast<IShellItem>())
        {
            switch (item)
            {
                case TabBar tabBar:
                    AddTabBar(tabBar, string.Empty);
                    break;
                case TabContent tab:
                    AddShellContent(tab, string.Empty);
                    break;
                case ShellContent shellContent:
                    AddShellContent(shellContent, string.Empty);
                    break;
            }
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        if (TopLevel.GetTopLevel(this) is { } topLevel)
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
            if (topLevel.InsetsManager is { } insets)
            {
                insets.SafeAreaChanged += InsetsOnSafeAreaChanged;
            }
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);

        if (TopLevel.GetTopLevel(this) is { } topLevel)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;
            if (topLevel.InsetsManager is { } insets)
            {
                insets.SafeAreaChanged -= InsetsOnSafeAreaChanged;
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _navigationRoot = e.NameScope.Find<NavigationRoot>("PART_ContentPresenter");

        if (TopLevel.GetTopLevel(this) is { InsetsManager: { } insetsManager })
        {
            TopLevel.SetAutoSafeAreaPadding(this, false);
            SafeAreaPadding = insetsManager.SafeAreaPadding;
            BottomPadding = new  Thickness(0, 3, 0, SafeAreaPadding.Bottom);
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
    }

    internal async Task AddViewAsync(object view, bool isForward, CancellationToken cancel = default)
    {
        if (_navigationRoot is null)
        {
            return;
        }

        await _navigationRoot.AddViewAsync(view, isForward, cancel);
    }

    internal async Task<bool> RemoveViewAsync(object view, CancellationToken cancel = default)
    {
        if (_navigationRoot is null)
        {
            return false;
        }
        
        return await _navigationRoot.RemoveViewAsync(view, cancel);
    }

    private void AddTabBar(TabBar item, string basePath)
    {
        var route = $"{item.Route}";

        foreach (var child in item.Items.OfType<TabContent>())
        {
            AddShellContent(child, route);
        }
    }

    private void AddShellContent(ShellContent item, string basePath)
    {
        var route = $"{basePath}/{item.Route}";
        var tabHost = item as TabContent;
        if (item is not null && (item.ContentTemplate?.IsSubclassOf(typeof(ItemsControl)) ?? false))
        {
            throw new InvalidOperationException("ShellContent must inherit from ItemsControl");
        }

        NavigationManager.Current.RegisterRoute(route, item,
            tabHost is null ? NavigationElementType.Page : NavigationElementType.Host);


        foreach (var child in item.Items)
        {
            AddShellContent(child, route);
        }
    }

    private void InsetsOnSafeAreaChanged(object? sender, SafeAreaChangedArgs e)
    {
        SafeAreaPadding = e.SafeAreaPadding;
    }

    private void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}