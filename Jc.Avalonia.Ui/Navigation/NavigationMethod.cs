namespace Jc.Avalonia.Ui.Navigation;

public enum NavigationMethod
{
    /// <summary>
    ///     Pushes a new page onto the navigation stack.
    /// </summary>
    Push,

    /// <summary>
    ///     Pops the current page from the navigation stack.
    /// </summary>
    Pop,

    /// <summary>
    ///     Replaces the current page with a new one.
    /// </summary>
    Replace,

    /// <summary>
    ///     Clears the navigation stack.
    /// </summary>
    Clear
}