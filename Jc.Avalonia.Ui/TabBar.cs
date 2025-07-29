using Avalonia.Collections;
using Avalonia.Metadata;

namespace Jc.Avalonia.Ui;

public sealed class TabBar : IShellItem
{
    public string Route { get; set; } = "";

    [Content] public AvaloniaList<ITabBarItem> Items { get; set; } = new AvaloniaList<ITabBarItem>();
}