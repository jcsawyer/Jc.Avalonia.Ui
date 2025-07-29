using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        await NavigationManager.Current.NavigateAsync("/Home/Diary");
        base.OnLoaded(e);
    }
}