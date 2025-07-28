using System.Globalization;
using Avalonia.Controls.Templates;
using Avalonia.Data.Converters;
using Jc.Avalonia.Ui.Navigation;

namespace Jc.Avalonia.Ui.Converters;

internal sealed class NavigationModeToTemplateConverter : IValueConverter
{
    public IDataTemplate NoneTemplate { get; init; }
    public IDataTemplate TabularTemplate { get; init; }
    public IDataTemplate HamburgerTemplate { get; init; }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            NavigationMode.None => NoneTemplate,
            NavigationMode.Tabular => TabularTemplate,
            NavigationMode.Hamburger => HamburgerTemplate,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, $"Invalid {nameof(NavigationMode)} value.")
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}