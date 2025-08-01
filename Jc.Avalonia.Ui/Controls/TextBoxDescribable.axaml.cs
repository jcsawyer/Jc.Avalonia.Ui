using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Jc.Avalonia.Ui.Controls;

public partial class TextBoxDescribable : TextBox
{
    public static readonly StyledProperty<string?> LabelProperty = AvaloniaProperty.Register<TextBoxDescribable, string?>(
        nameof(Label));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
}