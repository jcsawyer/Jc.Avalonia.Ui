using System.Windows.Input;
using ReactiveUI;

namespace Jc.Avalonia.Ui.Navigation;

public sealed record NavigationElement
{
    public string Route { get; init; }
    
    public Type Page { get; init; }
    
    public string Title { get; init; }
    
    public string? Icon { get; init; }
    
    private List<NavigationElement> _children = new List<NavigationElement>();
    
    public IReadOnlyList<NavigationElement> Children => _children.AsReadOnly();
    
    public NavigationElement Parent { get; private set; }
    
    public NavigationElementType Type { get; init; }
    
    internal ICommand Command { get; set; }

    public NavigationElement()
    {
        Command = ReactiveCommand.CreateFromTask(Clicked);
    }
    
    internal async Task Clicked(CancellationToken cancel)
    {
        await NavigationManager.Current.NavigateAsync(Route, NavigationMethod.Pop, CancellationToken.None);
    }
    
    public void AddElement(NavigationElement element)
    {
        if (element.Parent is not null)
        {
            throw new InvalidOperationException($"Element '{element.Route}' already has a parent.");
        }
        
        _children.Add(element);
        element.Parent = this;
    }
}