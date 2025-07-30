using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DynamicData;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Sample.Controls;
using Jc.Avalonia.Ui.Sample.Views;
using LiveChartsCore;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{
    public static ICommand AddCommand { get; } = ReactiveCommand.Create(Add);

    private ObservableCollection<BloodSugarGraph.DateTimePointReading> _graphReadings = new ObservableCollection<BloodSugarGraph.DateTimePointReading>([]);
    public ObservableCollection<BloodSugarGraph.DateTimePointReading> GraphReadings
    {
        get => _graphReadings;
        set => this.RaiseAndSetIfChanged(ref _graphReadings, value);
    }

    public Axis[] XAxis { get; set; }

    private Axis[] _yAxis = new Axis[] { };
    public Axis[] YAxis
    {
        get => _yAxis;
        set => this.RaiseAndSetIfChanged(ref _yAxis, value);
    }

    private RectangularSection[] _sections = new RectangularSection[] { };
    public RectangularSection[] InRangeSections
    {
        get => _sections;
        set => this.RaiseAndSetIfChanged(ref _sections, value);
    }
    public ISeries[] ReadingSeries { get; set; }
    
    public MainViewModel()
    {
        InitGraph();
        GenerateRandomGraphData();
    }

    private static void Add()
    {
        new DialogManager().OpenSheet(new SheetContent());
    }
    
    private void InitGraph()
    {
        ReadingSeries =
        [
            new LineSeries<BloodSugarGraph.DateTimePointReading>
            {
                Values = GraphReadings,
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

        XAxis =
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
        YAxis =
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
    }

    private void GenerateRandomGraphData()
    {
        GraphReadings.AddRange(Enumerable.Range(0, 50).Select(i =>
        {
            var dateTime = DateTime.Now.AddMinutes(-i * 30);
            var value = new Random().NextDouble() * 12 + 3;
            var direction = i % 2 == 0 ? "Up" : "Down";
            return new BloodSugarGraph.DateTimePointReading(dateTime, value, direction);
        }));

        InRangeSections =
        [
            new RectangularSection
            {
                Yi = 4,
                Yj = 10,
                Fill = new SolidColorPaint(SKColor.Parse("#11000000"))
            }
        ];
        YAxis =
        [
            new Axis
            {
                CustomSeparators = [3, 9, 15, 21],
                TextSize = 10,
                MinLimit = 0,
                MaxLimit = 22,
                SeparatorsPaint = new SolidColorPaint(SKColor.Empty),
                LabelsPaint = new SolidColorPaint(SKColor.Parse("#01000000")),
                Padding = new Padding(0, 0, 0, 0),
            }
        ];
    }
    
    
}