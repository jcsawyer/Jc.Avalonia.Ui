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