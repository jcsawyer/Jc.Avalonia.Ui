using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Sample.Views;

public partial class DiaryPage : UserControl
{
    public DiaryPage()
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
        NavigationManager.Current.NavigateTo(new Insights(), NavigationMethod.Push);
    }
    
    private void Button3_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationManager.Current.GoBack();
    }
}