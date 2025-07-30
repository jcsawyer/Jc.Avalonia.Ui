using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Jc.Avalonia.Ui.Dialogs;
using Jc.Avalonia.Ui.Navigation;
using LiveChartsCore.SkiaSharpView.Avalonia;

namespace Jc.Avalonia.Ui.Sample.Views.Pages;

public partial class Diary : UserControl
{
    private double _currentOffset;
    private ScrollViewer _scrollViewer;
    private StackPanel _largeCenter;
    private CartesianChart _bloodSugarChart;
    private Panel _smallSplit;
    private StackPanel _smallSplitBackground;
    private StackPanel _smallSplitLeft;
    private StackPanel _smallSplitRight;

    public Diary()
    {
        InitializeComponent();

        _scrollViewer = this.FindControl<ScrollViewer>("scrollView");
        _largeCenter = this.FindControl<StackPanel>("largeCenter");
        _bloodSugarChart = this.FindControl<CartesianChart>("bloodSugarChart");
        _smallSplit = this.FindControl<Panel>("smallSplit");
        _smallSplitBackground = this.FindControl<StackPanel>("smallSplitBackground");
        _smallSplitLeft = this.FindControl<StackPanel>("smallSplitLeft");
        _smallSplitRight = this.FindControl<StackPanel>("smallSplitRight");
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialogManager = new DialogManager();
        dialogManager.OpenSheet(new SheetContent());
    }

    private async void Button2_OnClick(object? sender, RoutedEventArgs e)
    {
        //NavigationManager.Current.NavigateTo(new Insights(), NavigationMethod.Push);
    }

    private void Button3_OnClick(object? sender, RoutedEventArgs e)
    {
        //NavigationManager.Current.GoBack();
    }

    private void ScrollView_OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var offset = Math.Clamp(_scrollViewer.Offset.Y - 50, 0, 100);
        var progress = offset / 100;

        _largeCenter.Opacity = 1 - progress;
        (_largeCenter.RenderTransform as ScaleTransform).ScaleX = 1 - progress;
        (_largeCenter.RenderTransform as ScaleTransform).ScaleY = 1 - progress;

        var chartProgress = Math.Clamp(_scrollViewer.Offset.Y - 100, 0, 200);
        _bloodSugarChart.Height = 200 - chartProgress;

        if (_scrollViewer.Offset.Y > 125)
        {
            var smallOffset = Math.Clamp((_scrollViewer.Offset.Y - 125) * 2, 0, 200);
            var smallProgress = smallOffset / 200;
            
            _smallSplitBackground.Opacity = smallProgress;
            _smallSplit.Opacity = smallProgress;
            
            _smallSplitLeft.Margin = new Thickness(200 - smallOffset, 0, 0, 0);
            _smallSplitRight.Margin = new Thickness(0, 0, 200 - smallOffset, 0);
        }
        else
        {
            _smallSplitBackground.Opacity = 0;
            _smallSplit.Opacity = 0;
        }
    }
}