using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Jc.Avalonia.Ui.Dialogs;

namespace Jc.Avalonia.Ui.Sample.Views.Sheets;

public partial class AddSheet : UserControl
{
    public AddSheet()
    {
        InitializeComponent();
        
        DialogManager.OnSheetOpened += DialogManagerOnOnSheetOpened;
    }
    
    ~AddSheet()
    {
        DialogManager.OnSheetOpened -= DialogManagerOnOnSheetOpened;
    }

    private void DialogManagerOnOnSheetOpened(object? sender, EventArgs e)
    {
        // Invalidate the arrange pass to ensure the layout is updated
        // This is necessary to ensure that the SearchResults control is properly arranged
        // after the sheet is opened, especially if it contains dynamic content.
        SearchResults.InvalidateArrange();
    }

    private void Next_OnClick(object? sender, RoutedEventArgs e)
    {
        AddTreatmentTabControl.SelectedIndex += 2;
    }

    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        AddTreatmentTabControl.SelectedIndex -= 2;
    }
}