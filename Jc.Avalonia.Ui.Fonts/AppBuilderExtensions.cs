using Avalonia;

namespace Jc.Avalonia.Ui.Fonts;

public static class AppBuilderExtension
{
    public static AppBuilder WithJcFonts(this AppBuilder appBuilder)
    {
        return appBuilder.ConfigureFonts(fontManager =>
        {
            fontManager.AddFontCollection(new JcFontCollection());
        });
    }
}