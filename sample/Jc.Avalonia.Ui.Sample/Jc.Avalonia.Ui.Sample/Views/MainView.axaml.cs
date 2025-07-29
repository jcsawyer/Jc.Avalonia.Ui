using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Navigation;
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
        NavigationManager.Current.NavigateTo(new DiaryPage());
        base.OnLoaded(e);
    }
}