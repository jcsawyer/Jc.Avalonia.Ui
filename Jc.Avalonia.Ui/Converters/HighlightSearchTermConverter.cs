using Avalonia.Controls.Documents;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Jc.Avalonia.Ui.Converters;

public static class HighlightSearchTermConverter
{
    public static FuncMultiValueConverter<string, IList<Inline>>? HighlightSearchConverter { get; } = new(enumerable =>
    {
        var values = enumerable.ToArray();
        var text = values[0];
        var searchTerm = values[1];

        var inlines = new List<Inline>();
        var index = 0;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            inlines.Add(new Run(text));
            return inlines;
        }
        
        while (index < text?.Length)
        {
            var searchIndex = text.IndexOf(searchTerm, index, StringComparison.OrdinalIgnoreCase);
            if (searchIndex < 0)
            {
                inlines.Add(new Run(text.Substring(index)));
                break;
            }

            if (searchIndex > index)
            {
                inlines.Add(new Run(text.Substring(index, searchIndex - index)));
            }

            inlines.Add(new Run(text.Substring(searchIndex, searchTerm.Length))
            {
                FontWeight = FontWeight.Bold,
            });

            index = searchIndex + searchTerm.Length;
        }

        return inlines;
    });
}