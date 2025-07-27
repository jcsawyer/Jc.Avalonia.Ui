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

    protected override void OnLoaded(RoutedEventArgs e)
    {
        var nav = new NavigationManager();
        nav.NavigateTo(new UserControl1(), NavigationMethod.Clear);
        base.OnLoaded(e);
    }
}