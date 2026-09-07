using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Filtering.Core;

namespace Portfolio.UI.Composition;

public partial class DataSection<TItem> : ComponentBase
{
    [Parameter][Required] public Type? ItemComponentType {get;set;}
    [Parameter][Required] public IEnumerable<TItem> Items { get; set; } = [];
    [Parameter] public string Class { get; set;} = "";
    [Parameter] public string Style { get; set;} = "";
    [Parameter] public FilterCollection<TItem>? Filters { get; set; }
    [Parameter] public DimensionalFilterCollection<TItem>? DimensionalFilters { get; set; }
    protected IEnumerable<TItem>? DisplayItems = null;


    protected override void OnInitialized()
    {
        Items = new HashSet<TItem>(Items);
        var comparer = ResolveComparer();
        if (comparer is not null)
            Items = new SortedSet<TItem>(Items, comparer);
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        // Support both old FilterCollection and new DimensionalFilterCollection
        if (DimensionalFilters is not null)
            DisplayItems = DimensionalFilters.Apply(Items!);
        else if (Filters is not null)
            DisplayItems = Filters.Apply(Items!);
        else
            DisplayItems = Items;
    }

    private static Comparer<TItem>? ResolveComparer()
    {
        return typeof(IComparable<TItem>).IsAssignableFrom(typeof(TItem))
            ? Comparer<TItem>.Default
            : null;
    }
}