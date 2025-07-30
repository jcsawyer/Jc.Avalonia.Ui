using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Jc.Avalonia.Ui;

public class TabContent : ShellContent
{
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    private NavigationOutlet? _navigationOutlet;

    public static readonly StyledProperty<IPageTransition> PageTransitionProperty =
        AvaloniaProperty.Register<TabContent, IPageTransition>(
            nameof(PageTransition), defaultValue: new PageSlide(TimeSpan.FromSeconds(0.25)));

    public IPageTransition PageTransition
    {
        get => GetValue(PageTransitionProperty);
        set => SetValue(PageTransitionProperty, value);
    }

    public static readonly DirectProperty<NavigationRoot, object?> CurrentViewProperty =
        AvaloniaProperty.RegisterDirect<NavigationRoot, object?>(
            nameof(CurrentView),
            o => o.CurrentView);

    public object? CurrentView => _navigationOutlet?.CurrentView;

    internal async Task SwapViewAsync(object view, bool isForward, CancellationToken cancel = default)
    {
        if (view is not TabContent tabContent)
        {
            return;
        }

        var current = CurrentView;
        if (await _navigationOutlet?.AddViewAsync(tabContent.Content, isForward, cancel))
        {
            await _navigationOutlet?.RemoveViewAsync(current, cancel);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _navigationOutlet = e.NameScope.Find<NavigationOutlet>("PART_ContentPresenter");
    }
}