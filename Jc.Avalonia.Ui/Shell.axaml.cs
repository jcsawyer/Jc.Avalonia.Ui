using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui;

public partial class Shell : UserControl
{
    public Shell()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel.BackRequested += TopLevelOnBackRequested;
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
}