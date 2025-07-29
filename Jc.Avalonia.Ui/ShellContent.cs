using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Metadata;

namespace Jc.Avalonia.Ui;

public class ShellContent : UserControl, ITabBarItem
{
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<ShellContent, string>(
        nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly StyledProperty<string> RouteProperty = AvaloniaProperty.Register<ShellContent, string>(
        nameof(Route));

    public string Route
    {
        get => GetValue(RouteProperty);
        set => SetValue(RouteProperty, value);
    }

    public static readonly StyledProperty<string?> IconProperty = AvaloniaProperty.Register<ShellContent, string?>(
        nameof(Icon));

    public string? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    
    public static readonly StyledProperty<Type> ContentTemplateProperty = AvaloniaProperty.Register<ShellContent, Type>(
        nameof(ContentTemplate));

    public Type ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }
    
    [Content] public AvaloniaList<ShellContent> Items { get; set; } = new AvaloniaList<ShellContent>();
}