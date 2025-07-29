using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Jc.Avalonia.Ui.Converters;

namespace Jc.Avalonia.Ui.Navigation.Tabular;

public partial class TabBar : UserControl
{
    public static readonly StyledProperty<AvaloniaList<TabItemModel>> ItemsProperty = AvaloniaProperty.Register<TabBar, AvaloniaList<TabItemModel>>(
        nameof(Items), defaultValue: new AvaloniaList<TabItemModel>());

    public AvaloniaList<TabItemModel> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public TabBar()
    {
        InitializeComponent();
        if (!Resources.ContainsKey("IsCallToActionTabItemToTemplateConverter"))
        {
            Resources.Add("IsCallToActionTabItemToTemplateConverter", new IsCallToActionTabItemToTemplateConverter
            {
                TabBarItem = (IDataTemplate)Resources["TabBarItem"]!,
                CallToActionTabBarItem = (IDataTemplate)Resources["CallToActionTabBarItem"]!,
            });
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
    }
}