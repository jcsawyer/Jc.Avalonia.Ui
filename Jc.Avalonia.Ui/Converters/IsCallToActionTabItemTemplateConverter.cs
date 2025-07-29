using System.Globalization;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;

namespace Jc.Avalonia.Ui.Converters;

internal sealed class IsCallToActionTabItemToTemplateConverter : IValueConverter
{
    public IDataTemplate TabBarItem { get; init; }
    public IDataTemplate CallToActionTabBarItem { get; init; }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is true ? CallToActionTabBarItem : TabBarItem;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}