using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class UserControl2 : UserControl
{
    public UserControl2()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var nav = new NavigationManager();
        nav.GoBack();
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        var nav = new NavigationManager();
        nav.NavigateTo(new UserControl1(), NavigationMethod.Push);
    }
}