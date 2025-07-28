using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Jc.Avalonia.Ui.Converters;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;
using ReactiveUI;

namespace Jc.Avalonia.Ui;

public partial class Shell : UserControl
{
    public static readonly StyledProperty<NavigationMode> NavigationModeProperty = AvaloniaProperty.Register<Shell, NavigationMode>(
        nameof(NavigationMode));

    public NavigationMode NavigationMode
    {
        get => GetValue(NavigationModeProperty);
        set => SetValue(NavigationModeProperty, value);
    }

    public static readonly StyledProperty<AvaloniaList<Page>> PagesProperty =
        AvaloniaProperty.Register<Shell, AvaloniaList<Page>>(nameof(Pages), new AvaloniaList<Page>());

    public AvaloniaList<Page> Pages
    {
        get => GetValue(PagesProperty);
        set => SetValue(PagesProperty, value);
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
        topLevel.BackRequested += TopLevelOnBackRequested;
        
        if (Pages.Count > 0)
        {
            NavigationManager.Current.NavigateTo(Activator.CreateInstance(Pages[0].PageType) as UserControl);
        }
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