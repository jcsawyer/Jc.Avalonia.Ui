using System;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Metadata;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Jc.Avalonia.Ui.Sample.Controls;

public class BloodSugarGraph : TemplatedControl
{
    public static readonly StyledProperty<object?> ContentProperty = AvaloniaProperty.Register<BloodSugarGraph, object?>(
        nameof(Content));

    [Content] public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    
    private CartesianChart _cartesianChart;
    private ScrollViewer _scrollViewer;
    private StackPanel _largeCenter;
    private CartesianChart _bloodSugarChart;
    private Panel _smallSplit;
    private StackPanel _smallSplitBackground;
    private StackPanel _smallSplitLeft;
    private StackPanel _smallSplitRight;
    
    public static readonly StyledProperty<AvaloniaList<DateTimePointReading>> ReadingsProperty = AvaloniaProperty.Register<BloodSugarGraph, AvaloniaList<DateTimePointReading>>(
        nameof(Readings));

    public AvaloniaList<DateTimePointReading> Readings
    {
        get => GetValue(ReadingsProperty);
        set => SetValue(ReadingsProperty, value);
    }
    
    public static readonly StyledProperty<double> CurrentReadingProperty = AvaloniaProperty.Register<BloodSugarGraph, double>(
        nameof(CurrentReading));

    public double CurrentReading
    {
        get => GetValue(CurrentReadingProperty);
        set => SetValue(CurrentReadingProperty, value);
    }

    public static readonly StyledProperty<double> InsulinOnBoardProperty = AvaloniaProperty.Register<BloodSugarGraph, double>(
        nameof(InsulinOnBoard));

    public double InsulinOnBoard
    {
        get => GetValue(InsulinOnBoardProperty);
        set => SetValue(InsulinOnBoardProperty, value);
    }

    public static readonly StyledProperty<string> ReadingDirectionProperty = AvaloniaProperty.Register<BloodSugarGraph, string>(
        nameof(ReadingDirection));

    public string ReadingDirection
    {
        get => GetValue(ReadingDirectionProperty);
        set => SetValue(ReadingDirectionProperty, value);
    }

    public static readonly StyledProperty<double?> TargetRangeLowerProperty = AvaloniaProperty.Register<BloodSugarGraph, double?>(
        nameof(TargetRangeLower));

    public double? TargetRangeLower
    {
        get => GetValue(TargetRangeLowerProperty);
        set => SetValue(TargetRangeLowerProperty, value);
    }

    public static readonly StyledProperty<double?> TargetRangeUpperProperty = AvaloniaProperty.Register<BloodSugarGraph, double?>(
        nameof(TargetRangeUpper));

    public double? TargetRangeUpper
    {
        get => GetValue(TargetRangeUpperProperty);
        set => SetValue(TargetRangeUpperProperty, value);
    }
    
    private Axis[] XAxes =
    [
        new DateTimeAxis(TimeSpan.FromHours(4), date => date.ToString("h tt"))
        {
            Padding = new Padding(0),
            SeparatorsPaint = new SolidColorPaint(SKColor.Empty),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#f5f5f5")),
            TextSize = 8,
            MaxLimit = DateTime.Now.Ticks,
            MinLimit = DateTime.Now.AddHours(-24).Ticks,
        }
    ];
    
    private Axis[] YAxes =
    [
        new Axis
        {
            CustomSeparators =
            [
                3, 9, 15, 21
            ],
            TextSize = 10,
            MinLimit = 0,
            MaxLimit = 22,
            SeparatorsPaint = new SolidColorPaint(SKColor.Empty),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#000000")),
            Padding = new Padding(0, 0, 0, 0),
        }
    ];
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _cartesianChart = e.NameScope.Find<CartesianChart>("bloodSugarChart");
        _scrollViewer = e.NameScope.Find<ScrollViewer>("scrollView");
        _largeCenter = e.NameScope.Find<StackPanel>("largeCenter");
        _bloodSugarChart = e.NameScope.Find<CartesianChart>("bloodSugarChart");
        _smallSplit = e.NameScope.Find<Panel>("smallSplit");
        _smallSplitBackground = e.NameScope.Find<StackPanel>("smallSplitBackground");
        _smallSplitLeft = e.NameScope.Find<StackPanel>("smallSplitLeft");
        _smallSplitRight = e.NameScope.Find<StackPanel>("smallSplitRight");
        
        if (_cartesianChart is null || _scrollViewer is null)
        {
            return;
        }
        
        _cartesianChart.XAxes = XAxes;
        _cartesianChart.YAxes = YAxes;

        _cartesianChart.Sections =
        [
            new RectangularSection
            {
                Yi = TargetRangeLower ?? 4,
                Yj = TargetRangeUpper ?? 10,
                Fill = new SolidColorPaint(SKColor.Parse("#11000000"))
            }
        ];
        
        _cartesianChart.Series =
        [
            new LineSeries<BloodSugarGraph.DateTimePointReading>
            {
                Values = Readings,
                Stroke = new SolidColorPaint(SKColor.Parse("#f5f5f5"))
                {
                    StrokeThickness = 3,
                },
                Fill = null,
                GeometryFill = null,
                GeometryStroke = null,
                DataPadding = new LvcPoint(0, 0),
                AnimationsSpeed = TimeSpan.FromMilliseconds(500),
                EasingFunction = EasingFunctions.Lineal,
                IsHoverable = true,
                LineSmoothness = 0,
                Mapping = (point, i) => new Coordinate(point.DateTime.Ticks, point.Value),
                XToolTipLabelFormatter = x => x.Model.DateTime.ToString("h:mm tt"),
                YToolTipLabelFormatter = y => y.Model.Direction + " " + y.Model.Value.ToString("N2"),
            }
        ];
        
        _scrollViewer.ScrollChanged += ScrollViewerOnScrollChanged;
    }

    private void ScrollViewerOnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var offset = Math.Clamp(_scrollViewer.Offset.Y - 50, 0, 100);
        var progress = offset / 100;

        _largeCenter.Opacity = 1 - progress;
        (_largeCenter.RenderTransform as ScaleTransform).ScaleX = 1 - progress;
        (_largeCenter.RenderTransform as ScaleTransform).ScaleY = 1 - progress;

        var chartProgress = Math.Clamp(_scrollViewer.Offset.Y - 100, 0, 200);
        _bloodSugarChart.Height = 200 - chartProgress;

        if (_scrollViewer.Offset.Y > 125)
        {
            var smallOffset = Math.Clamp((_scrollViewer.Offset.Y - 125) * 2, 0, 200);
            var smallProgress = smallOffset / 200;
            _smallSplitBackground.Opacity = smallProgress;
            _smallSplit.Opacity = smallProgress;
            
            _smallSplitLeft.Margin = new Thickness(200 - smallOffset, 0, 0, 0);
            _smallSplitRight.Margin = new Thickness(0, 0, 200 - smallOffset, 0);
        }
        else
        {
            _smallSplitBackground.Opacity = 0;
            _smallSplit.Opacity = 0;
        }
    }
    
    public sealed class DateTimePointReading : DateTimePoint
    {
        private string _direction;
        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                OnPropertyChanged();
            }
        }

        public new double Value
        {
            get => base.Value ?? 0;
            set
            {
                base.Value = value;
                OnPropertyChanged();
            }
        }

        public DateTimePointReading(DateTime dateTime, double value, string direction) : base(dateTime, value)
        {
            _direction = direction ?? throw new ArgumentNullException(nameof(direction));
        }
    }
}