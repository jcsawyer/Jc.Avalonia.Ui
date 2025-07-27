using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Jc.Avalonia.Ui.Dialogs;

internal partial class Sheet : UserControl
{
    public static readonly StyledProperty<double> SheetHeightProperty = AvaloniaProperty.Register<Sheet, double>(
        nameof(SheetHeight), coerce: (element, value) =>
        {
            if (element is Sheet sheet)
            {
                sheet.ContentHeight = value - (value / 5) - 35;
            }

            return value;
        });

    public double SheetHeight
    {
        get => GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    public static readonly StyledProperty<double> ContentHeightProperty = AvaloniaProperty.Register<Sheet, double>(
        nameof(ContentHeight));

    public double ContentHeight
    {
        get => GetValue(ContentHeightProperty);
        set => SetValue(ContentHeightProperty, value);
    }

    public static readonly StyledProperty<bool> IsOpenProperty = AvaloniaProperty.Register<Sheet, bool>(
        nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set
        {
            SetValue(IsOpenProperty, value);
            _animtationTimer.Start();
        }
    }

    private bool IsOpening { get; set; }
    private bool IsClosing { get; set; }

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

        ContentHeight = SheetHeight - (SheetHeight / 5) - 35;
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
                if (transform.Y < SheetHeight / 5)
                {
                    transform.Y = SheetHeight / 5;
                    _animtationTimer.Stop();
                    if (IsOpening)
                    {
                        DialogManager.OnSheetOpened?.Invoke(this, EventArgs.Empty);
                        IsOpening = false;
                    }
                }
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
                if (transform.Y > sheet.Height)
                {
                    transform.Y = sheet.Height;
                    _animtationTimer.Stop();
                    if (IsClosing)
                    {
                        DialogManager.OnSheetClosed?.Invoke(this, EventArgs.Empty);
                        IsClosing = false;
                    }
                }
            }
            else
            {
                _animtationTimer.Stop();
            }
        }
    }

    public void Open<TContent>(TContent content) where TContent : Control
    {
        DialogManager.OnSheetOpening?.Invoke(this, EventArgs.Empty);
        Content = content;
        IsOpening = true;
        IsOpen = true;
    }

    public bool Close()
    {
        if (IsOpen || IsOpening)
        {
            DialogManager.OnSheetClosing?.Invoke(this, EventArgs.Empty);
            IsOpen = false;
            IsClosing = true;
            IsOpening = false;
            return true;
        }

        return false;
    }
}