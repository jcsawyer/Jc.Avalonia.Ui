using Avalonia.Data.Converters;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Converters;

internal static class TabContentButtonConverter
{
    public static FuncValueConverter<NavigationElement, bool> Converter = new(element =>
    {
        return element?.Page == typeof(TabBarButton);
    });
}