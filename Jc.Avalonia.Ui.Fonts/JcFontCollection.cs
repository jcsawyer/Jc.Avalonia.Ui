using Avalonia.Media.Fonts;

namespace Jc.Avalonia.Ui.Fonts;

public sealed class JcFontCollection : EmbeddedFontCollection
{
    public JcFontCollection() : base(
        new Uri("fonts:Jc", UriKind.Absolute),
        new Uri("avares://Jc.Avalonia.Ui.Fonts/Assets", UriKind.Absolute))
    {
    }
}