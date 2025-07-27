using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Jc.Avalonia.Ui.Dialogs;

internal partial class DialogHost : UserControl
{
    private Size? _bounds;

    public Control SheetContent { get; set; }

    public static readonly StyledProperty<bool> IsDialogOpenProperty = AvaloniaProperty.Register<DialogHost, bool>(
        nameof(IsDialogOpen));
    
    public bool IsDialogOpen
    {
        get => GetValue(IsDialogOpenProperty);
        set => SetValue(IsDialogOpenProperty, value);
    }

    internal static Sheet Sheet => GetDialogHost().GetVisualDescendants().OfType<Sheet>().Single();
    
    public DialogHost()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        var topLevel = TopLevel.GetTopLevel(this);
        topLevel!.SizeChanged += TopLevelOnSizeChanged;
    }

    private void TopLevelOnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        if (e is { HeightChanged: false, WidthChanged: false })
        {
            return;
        }
        
        _bounds = e.NewSize;
        Sheet.SheetHeight = _bounds.Value.Height;
    }

    private void ShellMask_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        CloseSheet();
    }

    public static bool CloseAllDialogs()
    {
        return CloseSheet();
    }
    
    public static bool CloseSheet()
    {
        GetDialogHost().IsDialogOpen = false;
        return Sheet.Close();
    }

    internal static void OpenSheet<TContent>(TContent content) where TContent : Control
    {
        GetDialogHost().IsDialogOpen = true;
        Sheet.Open(content);
    }
    
    internal static DialogHost GetDialogHost()
    {
        DialogHost? dialogHost;
        try
        {
            dialogHost = ((ISingleViewApplicationLifetime)Application.Current!.ApplicationLifetime!).MainView!
                .GetVisualDescendants().OfType<DialogHost>().Single();
        }
        catch
        {
            try
            {
                dialogHost = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!).MainWindow!
                    .GetVisualDescendants().OfType<DialogHost>().Single();
            }
            catch
            {
                throw new InvalidOperationException($"A single {nameof(DialogHost)} control must exist in the visual tree.");
            }
        }

        return dialogHost;
    }
}