using System.Xml.Schema;
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
    
    public static readonly StyledProperty<string> CurrentPageProperty = AvaloniaProperty.Register<TabBar, string>(
        nameof(CurrentPage));

    public string CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
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
        NavigationManager.Current.OnNavigated += CurrentOnOnNavigated;
        foreach (var item in Items)
        {
            item.IsActive = item.Title == NavigationManager.Current.CurrentPage;
        }
        base.OnLoaded(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        NavigationManager.Current.OnNavigated -= CurrentOnOnNavigated;
        base.OnUnloaded(e);
    }

    private void CurrentOnOnNavigated(object? sender, string e)
    {
        foreach (var item in Items)
        {
            item.IsActive = item.Title == e;
        }
    }
}