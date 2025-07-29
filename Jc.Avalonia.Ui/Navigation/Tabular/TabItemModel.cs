using System.Reactive;
using System.Windows.Input;
using Avalonia;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Navigation.Tabular;

public class TabItemModel : AvaloniaObject
{
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<TabItemModel, string>(
        nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<string?> IconProperty = AvaloniaProperty.Register<TabItemModel, string?>(
        nameof(Icon));

    public string? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<bool> IsCallToActionProperty = AvaloniaProperty.Register<TabItemModel, bool>(
        nameof(IsCallToAction), defaultValue: false);

    public bool IsCallToAction
    {
        get => GetValue(IsCallToActionProperty);
        set => SetValue(IsCallToActionProperty, value);
    }
    
    public static readonly StyledProperty<ReactiveCommand<Unit, Unit>?> CommandProperty = AvaloniaProperty.Register<TabItemModel, ReactiveCommand<Unit, Unit>?>(
        nameof(Command), defaultValue: null);

    public ReactiveCommand<Unit, Unit>? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<TabItemModel, bool>(
        nameof(IsActive));

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
}