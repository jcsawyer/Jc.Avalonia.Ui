using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;

namespace Jc.Avalonia.Ui.Behaviors;

public static class InlineTextBehavior
{
    public static readonly AttachedProperty<IEnumerable<Inline>> InlineSourceProperty =
        AvaloniaProperty.RegisterAttached<TextBlock, IEnumerable<Inline>>(
            "InlineSource", typeof(InlineTextBehavior));

    static InlineTextBehavior()
    {
        InlineSourceProperty.Changed.Subscribe(args =>
        {
            if (args.Sender is TextBlock text && args.NewValue.Value is { } inlines)
            {
                text.Inlines.Clear();
                text.Inlines.AddRange(inlines);
            }
        });
    }

    public static void SetInlineSource(TextBlock element, IEnumerable<Inline> value) =>
        element.SetValue(InlineSourceProperty, value);

    public static IEnumerable<Inline> GetInlineSource(TextBlock element) =>
        element.GetValue(InlineSourceProperty);
}