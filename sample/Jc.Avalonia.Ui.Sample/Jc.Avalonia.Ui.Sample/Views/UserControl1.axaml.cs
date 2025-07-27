using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class UserControl1 : UserControl
{
    public UserControl1()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialogManager = new DialogManager();
        dialogManager.OpenSheet(new SheetContent());
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        var nav = new NavigationManager();
        nav.NavigateTo(new UserControl2(), NavigationMethod.Push);
    }
    
    private void Button3_OnClick(object? sender, RoutedEventArgs e)
    {
        var nav = new NavigationManager();
        nav.GoBack();
    }
}