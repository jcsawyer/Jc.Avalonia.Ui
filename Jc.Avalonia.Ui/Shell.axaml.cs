using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui;

public partial class Shell : UserControl
{
    public static readonly StyledProperty<bool> IsDialogOpenProperty = AvaloniaProperty.Register<Shell, bool>(
        nameof(IsDialogOpen));

    public bool IsDialogOpen
    {
        get => GetValue(IsDialogOpenProperty);
        set => SetValue(IsDialogOpenProperty, value);
    }

    public Shell()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel.BackRequested += TopLevelOnBackRequested;
        topLevel.SizeChanged += TopLevelOnSizeChanged;
        Width = topLevel.Bounds.Width;
        Height = topLevel.Bounds.Height;
    }

    private void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        if (IsDialogOpen)
        {
            CloseSheet();
        }

        NavigationRoot.PopPage();
    }

    private void TopLevelOnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        Width = topLevel.Bounds.Width;
        Height = topLevel.Bounds.Height;
        var shell = GetShell();
        var sheet = shell.GetVisualDescendants().OfType<Sheet>().Single();
        sheet.IsOpen = sheet.IsOpen;
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel.SizeChanged -= TopLevelOnSizeChanged;
        base.OnUnloaded(e);
    }
    
    internal static void OpenSheet()
    {
        var shell = GetShell();
        shell.IsDialogOpen = true;
        var sheet = shell.GetVisualDescendants().OfType<Sheet>().Single();
        sheet.IsOpen = true;
    }
    
    internal static void CloseSheet()
    {
        var shell = GetShell();
        shell.IsDialogOpen = false;
        var sheet = shell.GetVisualDescendants().OfType<Sheet>().Single();
        sheet.IsOpen = false;
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

    private void ShellMask_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        CloseSheet();
    }
}