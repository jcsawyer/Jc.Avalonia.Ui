using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Sample.ViewModels;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class MainView : UserControl
{
    
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.NavigateCommand.Execute(null);
        }
        base.OnLoaded(e);
    }
}