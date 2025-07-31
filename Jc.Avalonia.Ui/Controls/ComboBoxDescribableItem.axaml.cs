using Avalonia;
using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Controls;

public partial class ComboBoxDescribableItem : ComboBoxItem
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
}