using Avalonia;

namespace Jc.Avalonia.Ui.Navigation;

public class Page : AvaloniaObject
{
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<Page, string>(nameof(Title));

    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<Page, string>(nameof(Icon));

    public static readonly StyledProperty<Type> PageTypeProperty =
        AvaloniaProperty.Register<Page, Type>(nameof(PageType));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Type PageType
    {
        get => GetValue(PageTypeProperty);
        set => SetValue(PageTypeProperty, value);
    }
}