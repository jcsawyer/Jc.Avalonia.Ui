using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Dialogs;

public class DialogManager : IDialogManager
{
    public static EventHandler? OnSheetClosing;
    public static EventHandler? OnSheetClosed;
    public static EventHandler? OnSheetOpening;
    public static EventHandler? OnSheetOpened;
    
    public void OpenSheet<TContent>(TContent content) where TContent : Control
    {
        DialogHost.OpenSheet(content);
    }

    public void Close()
    {
        DialogHost.CloseAllDialogs();
    }
}