namespace Jc.Avalonia.Ui.Navigation;

public interface INavigationViewLocator
{
    public object? LocateView(NavigationElement navigationElement);
}