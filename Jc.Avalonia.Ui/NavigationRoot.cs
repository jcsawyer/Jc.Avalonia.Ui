using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;

namespace Jc.Avalonia.Ui;

public sealed class NavigationRoot : Panel
{
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public static readonly StyledProperty<IPageTransition> TransitionProperty =
        AvaloniaProperty.Register<NavigationRoot, IPageTransition>(
            nameof(Transition));

    public IPageTransition Transition
    {
        get => GetValue(TransitionProperty);
        set => SetValue(TransitionProperty, value);
    }

    public static readonly DirectProperty<NavigationRoot, object?> CurrentViewProperty =
        AvaloniaProperty.RegisterDirect<NavigationRoot, object?>(
            nameof(CurrentView),
            o => o.Children.LastOrDefault());

    public object? CurrentView => Children.LastOrDefault();

    public async Task AddViewAsync(object view, bool isForward, CancellationToken cancel = default)
    {
        await _semaphoreSlim.WaitAsync(cancel);
        try
        {
            var current = CurrentView;
            if ((current is not null && current == view) || view is not Control control)
            {
                return;
            }

            if (Children.Contains(control))
            {
                var index = Children.IndexOf(control);
                if (index != Children.Count - 1)
                {
                    Children.Move(index, Children.Count - 1);
                }
            }
            else
            {
                if (current is TabContent currentTabContent && view is TabContent)
                {
                    await currentTabContent.SwapViewAsync(view, isForward, cancel);
                    return;
                }
                
                {
                    Children.Add(control);
                }
            }

            await RunAnimationAsync(current, control, false, cancel);
            RaisePropertyChanged(CurrentViewProperty, current, CurrentView);
            
            if (current is null && view is TabContent tabContent)
            {
                await tabContent.SwapViewAsync(view, isForward, cancel);
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task<bool> RemoveViewAsync(object view, CancellationToken cancel = default)
    {
        await _semaphoreSlim.WaitAsync(cancel);
        try
        {
            if (!Children.Contains(CurrentView))
            {
                return false;
            }

            if (CurrentView == view)
            {
                await RunAnimationAsync(CurrentView, Children.Count > 1 ? Children[^2] : null, false, cancel);
            }

            if (view is not Control control)
            {
                return false;
            }

            var current = CurrentView;
            Children.Remove(control);
            RaisePropertyChanged(CurrentViewProperty, current, CurrentView);

            return true;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private Task RunAnimationAsync(object? from, object? to, bool removed, CancellationToken cancel)
    {
        if (from is TabContent tab && to is TabContent)
        {
            // If both are TabContent, we don't run the transition and let the TabControl handle the transition.
            return tab.SwapViewAsync(to, removed, cancel);
        }

        return Transition?.Start(from as Visual, to as Visual, !removed, cancel) ?? Task.CompletedTask;
    }
}