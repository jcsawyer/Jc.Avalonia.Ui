using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Jc.Avalonia.Ui.Controls;

public partial class ListBoxDescribableItem : ListBoxItem
{
    public static readonly StyledProperty<string> ItemProperty = AvaloniaProperty.Register<ListBoxDescribableItem, string>(
        nameof(Item));

    public string Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    public static readonly StyledProperty<string> DescriptionProperty = AvaloniaProperty.Register<ListBoxDescribableItem, string>(
        nameof(Description));

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly StyledProperty<string?> SearchTermProperty = AvaloniaProperty.Register<ListBoxDescribableItem, string?>(
        nameof(SearchTerm));

    public string? SearchTerm
    {
        get => GetValue(SearchTermProperty);
        set => SetValue(SearchTermProperty, value);
    }
}