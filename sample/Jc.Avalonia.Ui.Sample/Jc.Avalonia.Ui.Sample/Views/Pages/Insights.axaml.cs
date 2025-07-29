using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Sample.Views.Pages;

public partial class Insights : UserControl
{
    public Insights()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        //NavigationManager.Current.GoBack();
    }

    private void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
       // NavigationManager.Current.NavigateTo(new DiaryPage(), NavigationMethod.Push);
    }
}