using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace Jc.Avalonia.Ui.Controls;

public partial class CarouselView : SelectingItemsControl
{
    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new FuncTemplate<Panel?>(() => new StackPanel()
        {
            Orientation = Orientation.Horizontal
        });

    private bool _isApplied;
    private bool _isHorizontal;

    public static readonly StyledProperty<TimeSpan?> TransitionDurationProperty =
        CarouselViewScrollViewer.TransitionDurationProperty.AddOwner<CarouselView>();

    private bool _arranged;

    internal ItemsPresenter? ItemsPresenterPart { get; private set; }
    internal CarouselViewScrollViewer? ScrollViewerPart { get; private set; }

    public static readonly StyledProperty<IBrush> IndicatorColorProperty =
        AvaloniaProperty.Register<CarouselView, IBrush>(nameof(IndicatorColor), Brushes.Gray);

    public static readonly StyledProperty<IBrush> IndicatorSelectedColorProperty =
        AvaloniaProperty.Register<CarouselView, IBrush>(nameof(IndicatorSelectedColor), Brushes.Black);

    public static readonly StyledProperty<double> IndicatorSizeProperty =
        AvaloniaProperty.Register<CarouselView, double>(nameof(IndicatorSize), 10);

    private Panel? _indicatorPanel;

    public IBrush IndicatorColor
    {
        get => GetValue(IndicatorColorProperty);
        set => SetValue(IndicatorColorProperty, value);
    }

    public IBrush IndicatorSelectedColor
    {
        get => GetValue(IndicatorSelectedColorProperty);
        set => SetValue(IndicatorSelectedColorProperty, value);
    }

    public double IndicatorSize
    {
        get => GetValue(IndicatorSizeProperty);
        set => SetValue(IndicatorSizeProperty, value);
    }

    public TimeSpan? TransitionDuration
    {
        get => GetValue(TransitionDurationProperty);
        set => SetValue(TransitionDurationProperty, value);
    }

    static CarouselView()
    {
        SelectionModeProperty.OverrideDefaultValue<CarouselView>(SelectionMode.AlwaysSelected);
        ItemsPanelProperty.OverrideDefaultValue<CarouselView>(DefaultPanel);
        AutoScrollToSelectedItemProperty.OverrideDefaultValue<CarouselView>(false);
    }

    public CarouselView()
    {
        AddHandler(PointerWheelChangedEvent, FlipPointerWheelChanged, handledEventsToo: true);
        AddHandler(KeyDownEvent, FlipKeyDown, handledEventsToo: true);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) =>
        new CarouselViewItem();

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<CarouselViewItem>(item, out recycleKey);
    }

    protected override void PrepareContainerForItemOverride(Control element, object? item, int index)
    {
        if (element is CarouselViewItem viewItem)
        {
            viewItem.Content = item;
            element.Width = GetDesiredItemWidth();
            element.Height = GetDesiredItemHeight();
        }

        base.PrepareContainerForItemOverride(element, item, index);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        ItemsPresenterPart = e.NameScope.Get<ItemsPresenter>("PART_ItemsPresenter");

        _indicatorPanel = e.NameScope.Get<Panel>("PART_IndicatorPanel");
        UpdateIndicators();

        if (ScrollViewerPart != null)
        {
            ScrollViewerPart.RemoveHandler(Gestures.ScrollGestureEndedEvent, ScrollEndedEventHandler);
            ScrollViewerPart.SizeChanged -= ScrollViewerPart_SizeChanged;
        }

        ScrollViewerPart = e.NameScope.Find<CarouselViewScrollViewer>("PART_ScrollViewer");

        if (ScrollViewerPart != null)
        {
            ScrollViewerPart.AddHandler(Gestures.ScrollGestureEndedEvent, ScrollEndedEventHandler,
                handledEventsToo: true);
            ScrollViewerPart.SizeChanged += ScrollViewerPart_SizeChanged;
        }

        _isApplied = true;

        SetOrientation();
    }

    private void ScrollViewerPart_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        SetItemSize();

        if (ScrollViewerPart != null)
        {
            var enableTransition = ScrollViewerPart.EnableTransition;
            ScrollViewerPart.EnableTransition = false;
            this.ScrollIntoView(SelectedIndex);
            ScrollViewerPart.EnableTransition = enableTransition;
        }
    }

    private void SetItemSize()
    {
        var width = GetDesiredItemWidth();
        var height = GetDesiredItemHeight();

        var item = ContainerFromIndex(SelectedIndex);
        if (item is CarouselViewItem CarouselViewItem)
        {
            CarouselViewItem.Width = width;
            CarouselViewItem.Height = height;
        }
    }

    private void ScrollEndedEventHandler(object? sender, ScrollGestureEndedEventArgs e)
    {
        UpdateSelectedIndex();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var arrange = base.ArrangeOverride(finalSize);

        if (!_arranged)
        {
            var width = GetDesiredItemWidth();
            var height = GetDesiredItemHeight();

            for (var i = 0; i < ItemCount; i++)
            {
                var item = ContainerFromIndex(i);
                if (item is CarouselViewItem CarouselViewItem)
                {
                    CarouselViewItem.Width = width;
                    CarouselViewItem.Height = height;
                }
            }
        }

        _arranged = true;

        return arrange;
    }

    private void UpdateSelectedIndex()
    {
        if (ItemsPresenterPart != null && ScrollViewerPart != null && ItemCount > 0)
        {
            var offset = _isHorizontal ? ScrollViewerPart.Offset.X : ScrollViewerPart.Offset.Y;
            var viewport = _isHorizontal ? ScrollViewerPart.Viewport.Width : ScrollViewerPart.Viewport.Height;
            var viewPortIndex = (long)(offset / viewport);
            var lowerBounds = viewPortIndex * viewport;
            var midPoint = lowerBounds + (viewport * 0.5);

            var index = offset > midPoint ? viewPortIndex + 1 : viewPortIndex;

            SetScrollViewerOffset((int)Math.Max(0, Math.Min(index, ItemCount)));
        }
    }

    protected void FlipPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (e.Delta.Y < 0)
        {
            MoveNext();
        }
        else
        {
            MovePrevious();
        }
    }

    private void MoveNext()
    {
        if (ItemCount > 0)
        {
            SetScrollViewerOffset(Math.Min(ItemCount - 1, SelectedIndex + 1));
        }
    }

    private void MovePrevious()
    {
        if (ItemCount > 0)
        {
            SetScrollViewerOffset(Math.Max(0, SelectedIndex - 1));
        }
    }

    private void MoveStart()
    {
        if (ItemCount > 0)
        {
            SetScrollViewerOffset(0);
        }
    }

    private void MoveEnd()
    {
        if (ItemCount > 0)
        {
            SetScrollViewerOffset(Math.Max(0, ItemCount - 1));
        }
    }

    private void FlipKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
            case Key.Left:
            case Key.PageUp:
                MovePrevious();
                break;

            case Key.Down:
            case Key.Right:
            case Key.PageDown:
                MoveNext();
                break;
            case Key.Home:
                MoveStart();
                break;
            case Key.End:
                MoveEnd();
                break;
        }
    }

    internal double GetDesiredItemWidth()
    {
        double width = 0;
        if (ItemsPresenterPart is { } presenter)
        {
            if (presenter.Panel is VirtualizingPanel virtualizingPanel)
            {
                width = virtualizingPanel.Bounds.Width;
            }

            if (width == 0)
            {
                width = ScrollViewerPart != null ? ScrollViewerPart.Bounds.Width : Bounds.Width;
            }
        }

        if (width == 0)
        {
            width = Width;
        }

        return width;
    }

    internal double GetDesiredItemHeight()
    {
        double height = 0;
        if (ItemsPresenterPart is { } presenter)
        {
            if (presenter.Panel is VirtualizingPanel virtualizingPanel)
            {
                height = virtualizingPanel.Bounds.Height;
            }

            if (height == 0)
            {
                height = ScrollViewerPart != null ? ScrollViewerPart.Bounds.Height : Bounds.Height;
            }
        }

        if (height == 0)
        {
            height = Height;
        }

        return height;
    }

    private void SetOrientation()
    {
        if (!_isApplied)
        {
            return;
        }

        var panel = ItemsPanel.Build();

        switch (panel)
        {
            case StackPanel stackPanel:
                _isHorizontal = stackPanel.Orientation switch
                {
                    Orientation.Horizontal => true,
                    Orientation.Vertical => false,
                    _ => _isHorizontal
                };
                break;
            case VirtualizingStackPanel virtualizingStackPanel:
                _isHorizontal = virtualizingStackPanel.Orientation switch
                {
                    Orientation.Horizontal => true,
                    Orientation.Vertical => false,
                    _ => _isHorizontal
                };

                break;
        }
    }

    private void UpdateIndicators()
    {
        if (_indicatorPanel == null || Items == null)
            return;

        _indicatorPanel.Children.Clear();
        var count = ItemCount;

        for (int i = 0; i < count; i++)
        {
            var dot = new Ellipse
            {
                Width = IndicatorSize,
                Height = IndicatorSize,
                Margin = new Thickness(4),
                Fill = i == SelectedIndex ? IndicatorSelectedColor : IndicatorColor,
                Cursor = new Cursor(StandardCursorType.Hand)
            };

            int targetIndex = i;
            dot.PointerPressed += (_, __) => SelectedIndex = targetIndex;

            _indicatorPanel.Children.Add(dot);
        }
    }

    protected Vector IndexToOffset(int index)
    {
        var container = ContainerFromIndex(index);
        var panel = ItemsPanelRoot;
        var scrollViewer = ScrollViewerPart;
        if (container == null || panel == null || scrollViewer == null)
            return default;

        var bounds = container.Bounds;
        var offset = scrollViewer.Offset;

        if (bounds.Bottom > offset.Y + scrollViewer.Viewport.Height)
        {
            offset = offset.WithY((bounds.Bottom - scrollViewer.Viewport.Height) + panel.Margin.Top);
        }

        if (bounds.Y < offset.Y)
        {
            offset = offset.WithY(bounds.Y);
        }

        if (bounds.Right > offset.X + scrollViewer.Viewport.Width)
        {
            offset = offset.WithX((bounds.Right - scrollViewer.Viewport.Width) + panel.Margin.Left);
        }

        if (bounds.X < offset.X)
        {
            offset = offset.WithX(bounds.X);
        }

        return offset;
    }

    private void SetScrollViewerOffset(int index)
    {
        var offset = IndexToOffset(index);
        SetCurrentValue(SelectedIndexProperty, index);

        if (ScrollViewerPart is { } scrollViewer)
        {
            scrollViewer.SetCurrentValue(CarouselViewScrollViewer.OffsetProperty, offset);
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedIndexProperty)
        {
            SetScrollViewerOffset(change.GetNewValue<int>());
            UpdateIndicators();
        }

        if (change.Property == ItemsPanelProperty)
        {
            SetOrientation();
        }
    }
}