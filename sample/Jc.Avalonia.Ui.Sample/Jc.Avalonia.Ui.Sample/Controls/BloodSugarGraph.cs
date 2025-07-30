using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Avalonia;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Jc.Avalonia.Ui.Sample.Controls;

public class BloodSugarGraph : TemplatedControl
{
    private CartesianChart _cartesianChart;
    
    public static readonly StyledProperty<DateTimePointReading> ReadingsProperty = AvaloniaProperty.Register<BloodSugarGraph, DateTimePointReading>(
        nameof(Readings));

    public DateTimePointReading Readings
    {
        get => GetValue(ReadingsProperty);
        set => SetValue(ReadingsProperty, value);
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
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#000000")),
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
            SeparatorsPaint = new SolidColorPaint(SKColor.Parse("#000000").WithAlpha(20)),
            LabelsPaint = new SolidColorPaint(SKColor.Parse("#000000")),
            Padding = new Padding(0, 0, 0, 0),
        }
    ];
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _cartesianChart = e.NameScope.Find<CartesianChart>("CartesianChart");
        if (_cartesianChart is null)
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