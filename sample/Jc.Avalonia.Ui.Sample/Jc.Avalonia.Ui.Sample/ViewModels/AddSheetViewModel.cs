using ReactiveUI;

namespace Jc.Avalonia.Ui.Sample.ViewModels;

public sealed class AddSheetViewModel : ViewModelBase
{
    private string _searchTerm = string.Empty;

    public string SearchTerm
    {
        get => _searchTerm;
        set => this.RaiseAndSetIfChanged(ref _searchTerm, value);
    }
}