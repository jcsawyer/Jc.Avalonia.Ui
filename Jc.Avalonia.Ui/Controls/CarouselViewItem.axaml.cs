using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Jc.Avalonia.Ui.Controls;

public partial class CarouselViewItem : ListBoxItem
{
    private CarouselView? _carouselView;

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);

        _carouselView = e.Parent as CarouselView;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        AddHandler(RequestBringIntoViewEvent, BroughtIntoView);
    }

    private void BroughtIntoView(object? sender, RequestBringIntoViewEventArgs e)
    {
        if (_carouselView is { } parent)
        {
            Height = parent.GetDesiredItemHeight();
            Width = parent.GetDesiredItemWidth();
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        RemoveHandler(RequestBringIntoViewEvent, BroughtIntoView);
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        _carouselView = null;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if(_carouselView is { } parent)
        {
            Height = parent.GetDesiredItemHeight();
            Width = parent.GetDesiredItemWidth();
        }
        return base.MeasureOverride(availableSize);
    }
}
