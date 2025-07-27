using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Jc.Avalonia.Ui;

public partial class Sheet : UserControl
{
    private Point _dragStart;
    private double _initialOffsetY;
    private bool _isDragging;

    public static readonly StyledProperty<double> SheetHeightProperty = AvaloniaProperty.Register<Sheet, double>(
        nameof(SheetHeight));

    public double SheetHeight
    {
        get => GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Sheet, bool>(nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set
        {
            SetValue(IsOpenProperty, value);
            _animtationTimer.Start();
        }
    }

    private readonly DispatcherTimer _animtationTimer;

    private static readonly TimeSpan AnimationFramerate = TimeSpan.FromMilliseconds(16);

    private static readonly TimeSpan AnimationDuration = TimeSpan.Parse("0:0:0.25");
    private int AnimationTotalTicks => (int)(AnimationDuration.TotalSeconds / AnimationFramerate.TotalSeconds);


    public Sheet()
    {
        InitializeComponent();

        _animtationTimer = new DispatcherTimer
        {
            Interval = AnimationFramerate,
        };
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        var sheetControl = this.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        if (sheetControl is not { } sheet)
        {
            return;
        }

        ((TranslateTransform)sheet.RenderTransform!).Y = SheetHeight;
        _animtationTimer.Tick += AnimateTick;
    }

    private void AnimateTick(object? sender, EventArgs e)
    {
        var sheetControl = this.GetVisualDescendants().OfType<Border>()
            .FirstOrDefault(x => x.Name == "Sheet");
        if (sheetControl is not { } sheet)
        {
            return;
        }

        if (IsOpen)
        {
            var transform = sheet.RenderTransform as TranslateTransform;
            if (transform is null)
            {
                _animtationTimer.Stop();
                return;
            }
            
            if (transform.Y > SheetHeight / 5)
            {
                transform.Y -= SheetHeight / AnimationTotalTicks;
            }
            else
            {
                _animtationTimer.Stop();
            }
        }
        else if (!IsOpen)
        {
            var transform = (TranslateTransform)sheet.RenderTransform;
            if (transform is null)
            {
                _animtationTimer.Stop();
                return;
            }
            
            if (transform.Y < sheet.Height)
            {
                transform.Y += sheet.Height / AnimationTotalTicks;
            }
            else
            {
                _animtationTimer.Stop();
            }
        }
    }

    private void SheetTab_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var sheet = this.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        _dragStart = e.GetPosition(this);
        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }
        _initialOffsetY = translate.Y;
        _isDragging = true;
    }

    private void SheetTab_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }
        
        var sheet = this.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        var currentPos = e.GetPosition(this);
        var deltaY = currentPos.Y - _dragStart.Y;

        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }
        
        var newPos = _initialOffsetY + deltaY;
        if (newPos < _initialOffsetY)
        {
            newPos = _initialOffsetY;
        }

        translate.Y = newPos;
    }

    private void SheetTab_OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isDragging)
        {
            return;
        }

        var sheet = this.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        _isDragging = false;
        
        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }

        var dismissThreshold = _initialOffsetY * 3;
        var offset = translate.Y;

        Shell.GetShell().IsDialogOpen = !(offset > dismissThreshold);
        IsOpen = !(offset > dismissThreshold);
    }
}