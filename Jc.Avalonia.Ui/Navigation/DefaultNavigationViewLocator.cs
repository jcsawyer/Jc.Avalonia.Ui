namespace Jc.Avalonia.Ui.Navigation;

internal sealed class DefaultNavigationViewLocator : INavigationViewLocator
{
    public object? LocateView(NavigationElement navigationElement)
    {
#pragma warning disable IL2072
        return Activator.CreateInstance(navigationElement.Page) ??
#pragma warning restore IL2072
               throw new InvalidOperationException($"Cannot create instance of {navigationElement.Page.FullName}");
    }
}