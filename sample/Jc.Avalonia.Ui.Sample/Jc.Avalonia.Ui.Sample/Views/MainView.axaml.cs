using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        NavigationRoot.PushPage(new UserControl1());
        base.OnLoaded(e);
    }
    
    // private void OnShowSheetClicked(object? sender, RoutedEventArgs e)
    // {
    //     MySheet.IsOpen = true;
    // }
    //
    // private void OnCloseSheetClicked(object? sender, RoutedEventArgs e)
    // {
    //     MySheet.IsOpen = false;
    // }
}