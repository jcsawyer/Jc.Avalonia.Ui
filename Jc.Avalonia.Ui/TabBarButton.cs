using System.Windows.Input;
using Avalonia;

namespace Jc.Avalonia.Ui;

public class TabBarButton : TabContent, ITabBarItem
{
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<TabBarButton, ICommand?>(nameof(Command));

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}
