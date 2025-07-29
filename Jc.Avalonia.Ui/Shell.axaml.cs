using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Platform;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Jc.Avalonia.Ui.Converters;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;
using Jc.Avalonia.Ui.Navigation.Tabular;
using ReactiveUI;

namespace Jc.Avalonia.Ui;

public partial class Shell : UserControl
{
    public static readonly StyledProperty<NavigationMode> NavigationModeProperty =
        AvaloniaProperty.Register<Shell, NavigationMode>(
            nameof(NavigationMode));

    public NavigationMode NavigationMode
    {
        get => GetValue(NavigationModeProperty);
        set => SetValue(NavigationModeProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<TabItemModel>> TabItemsProperty = AvaloniaProperty.Register<Shell, AvaloniaList<TabItemModel>>(
        nameof(TabItems), defaultValue: new AvaloniaList<TabItemModel>());

    public AvaloniaList<TabItemModel> TabItems
    {
        get => GetValue(TabItemsProperty);
        set => SetValue(TabItemsProperty, value);
    }

    public static readonly StyledProperty<Thickness> SafeAreaPaddingProperty =
        AvaloniaProperty.Register<Shell, Thickness>(
            nameof(SafeAreaPadding));

    public Thickness SafeAreaPadding
    {
        get => GetValue(SafeAreaPaddingProperty);
        set => SetValue(SafeAreaPaddingProperty, value);
    }

    public static readonly StyledProperty<Thickness> BottomPaddingProperty =
        AvaloniaProperty.Register<Shell, Thickness>(
            nameof(BottomPadding));

    public Thickness BottomPadding
    {
        get => GetValue(BottomPaddingProperty);
        set => SetValue(BottomPaddingProperty, value);
    }

    public ICommand PageSelectCommand { get; }

    public Shell()
    {
        InitializeComponent();

        PageSelectCommand = ReactiveCommand.Create<Page>(NavigateToPage);

        if (!Resources.ContainsKey("NavigationModeToTemplateConverter"))
        {
            Resources.Add("NavigationModeToTemplateConverter", new NavigationModeToTemplateConverter
            {
                NoneTemplate = (IDataTemplate)Resources["NoneTemplate"]!,
                TabularTemplate = (IDataTemplate)Resources["TabularTemplate"]!,
                HamburgerTemplate = (IDataTemplate)Resources["HamburgerTemplate"]!
            });
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel is { })
        {
            topLevel.BackRequested += TopLevelOnBackRequested;
            if (topLevel.InsetsManager is { } insets)
            {
                insets.DisplayEdgeToEdgePreference = true;
                SafeAreaPadding = insets.SafeAreaPadding;
                BottomPadding = new Thickness(0, 0, 0, insets.SafeAreaPadding.Bottom);
                insets.SafeAreaChanged += InsetsOnSafeAreaChanged;
            }
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.InsetsManager is { } insets)
        {
            insets.SafeAreaChanged -= InsetsOnSafeAreaChanged;
        }

        base.OnUnloaded(e);
    }

    private void InsetsOnSafeAreaChanged(object? sender, SafeAreaChangedArgs e)
    {
        SafeAreaPadding = e.SafeAreaPadding;
        BottomPadding = new Thickness(0, 0, 0, e.SafeAreaPadding.Bottom);
    }

    private void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        if (DialogHost.CloseAllDialogs())
        {
            e.Handled = true;
            return;
        }

        NavigationRoot.PopPage();
        e.Handled = true;
    }

    internal static Shell GetShell()
    {
        Shell? shell;
        try
        {
            shell = ((ISingleViewApplicationLifetime)Application.Current!.ApplicationLifetime!).MainView!
                .GetVisualDescendants().OfType<Shell>().Single();
        }
        catch
        {
            try
            {
                shell = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow!
                    .GetVisualDescendants().OfType<Shell>().Single();
            }
            catch
            {
                throw new InvalidOperationException($"A single {nameof(Shell)} control must exist in the visual tree.");
            }
        }

        return shell;
    }

    public void NavigateToPage(Page page)
    {
        var pageInstance = Activator.CreateInstance(page.PageType) as UserControl;
        NavigationManager.Current.NavigateTo(pageInstance, NavigationMethod.Replace);
    }
}