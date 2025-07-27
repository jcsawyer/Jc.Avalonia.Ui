using Avalonia.Controls;

namespace Jc.Avalonia.Ui.Dialogs;

public interface IDialogManager
{
    void OpenSheet<TContent>(TContent content) where TContent : Control;
}