using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Jc.Avalonia.Ui;

public class JcUi : Styles
{
    public JcUi(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}