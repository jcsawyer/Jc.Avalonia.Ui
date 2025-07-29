using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Jc.Avalonia.Ui.Dialogs;

namespace Jc.Avalonia.Ui.Behaviors;

internal class SnapBackDragBehavior : Behavior<Border>
{
    private Point? _dragStart;
    private double _initialOffsetY;
    private bool _isDragging;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is { } sheetTab)
        {
            sheetTab.PointerPressed += OnPointerPressed;
            sheetTab.PointerMoved += OnPointerMoved;
            sheetTab.PointerReleased += OnPointerReleased;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is { } sheetTab)
        {
            sheetTab.PointerPressed -= OnPointerPressed;
            sheetTab.PointerMoved -= OnPointerMoved;
            sheetTab.PointerReleased -= OnPointerReleased;
        }

        base.OnDetaching();
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isDragging || !((sender as Control)?.GetVisualAncestors().OfType<Sheet>().SingleOrDefault() is { } sheetControl))
        {
            return;
        }

        var sheet = sheetControl.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        _isDragging = false;

        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }

        var dismissThreshold = _initialOffsetY * 3;
        var offset = translate.Y;

        var isOpen = !(offset > dismissThreshold);
        if (isOpen)
        {
            DialogHost.Sheet.IsOpen = true;
        }
        else
        {
            DialogHost.Sheet.Close();
            DialogHost.GetDialogHost().IsDialogOpen = isOpen;
        }
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDragging || !((sender as Control)?.GetVisualAncestors().OfType<Sheet>().SingleOrDefault() is { } sheetControl))
        {
            return;
        }

        var sheet = sheetControl.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        var currentPos = e.GetPosition(sheetControl);
        var deltaY = currentPos.Y - _dragStart.GetValueOrDefault().Y;

        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }

        if (deltaY < 0)
        {
            return;
        }

        var newPos = _initialOffsetY + deltaY;
        if (newPos < _initialOffsetY)
        {
            newPos = _initialOffsetY;
        }

        translate.Y = newPos;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!((sender as Control)?.GetVisualAncestors().OfType<Sheet>().SingleOrDefault() is { } sheetControl))
        {
            return;
        }

        var sheet = sheetControl.GetVisualDescendants().OfType<Border>().FirstOrDefault(x => x.Name == "Sheet");
        _dragStart = e.GetPosition(sheetControl);
        if (sheet?.RenderTransform is not TranslateTransform translate)
        {
            return;
        }

        _initialOffsetY = translate.Y;
        _isDragging = true;
    }
}