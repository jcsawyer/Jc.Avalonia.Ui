using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Dialogs;

public class DialogManager : IDialogManager
{
    public void OpenSheet<TContent>(TContent content) where TContent : Control
    {
        Sheet.OpenSheet(content);
    }
}