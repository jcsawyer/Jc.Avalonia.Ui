using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Dialogs;
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

        DialogManager.OnSheetOpening += (sender, args) =>
        {
            Console.WriteLine("SHEET OPENING");
        };
        
        DialogManager.OnSheetOpened += (sender, args) =>
        {
            Console.WriteLine("SHEET OPENED");
        };
        
        DialogManager.OnSheetClosing += (sender, args) =>
        {
            Console.WriteLine("SHEET CLOSING");
        };
        
        DialogManager.OnSheetClosed += (sender, args) =>
        {
            Console.WriteLine("SHEET CLOSED");
        };

    }
}