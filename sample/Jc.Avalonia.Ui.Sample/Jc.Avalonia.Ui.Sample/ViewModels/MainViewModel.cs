using System;
using System.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Sample.Controls;
using Jc.Avalonia.Ui.Sample.Views;
using Jc.Avalonia.Ui.Sample.Views.Sheets;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public class MainViewModel : ViewModelBase
{
    public static ICommand AddCommand { get; } = ReactiveCommand.Create(Add);

    private AvaloniaList<BloodSugarGraph.DateTimePointReading> _graphReadings = new AvaloniaList<BloodSugarGraph.DateTimePointReading>([]);
    public AvaloniaList<BloodSugarGraph.DateTimePointReading> GraphReadings
    {
        get => _graphReadings;
        set => this.RaiseAndSetIfChanged(ref _graphReadings, value);
    }
    
    public MainViewModel()
    {
        GraphReadings.AddRange(Enumerable.Range(0, 50).Select(i =>
        {
            var dateTime = DateTime.Now.AddMinutes(-i * 30);
            var value = new Random().NextDouble() * 12 + 3;
            var direction = i % 2 == 0 ? "Up" : "Down";
            return new BloodSugarGraph.DateTimePointReading(dateTime, value, direction);
        }));
    }

    private static void Add()
    {
        new DialogManager().OpenSheet(new AddSheet());
    }
}