using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class UserControl1 : UserControl
{
    public UserControl1()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Shell.OpenSheet();
    }
}