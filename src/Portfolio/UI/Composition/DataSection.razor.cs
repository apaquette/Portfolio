using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Filtering.Core;

namespace Portfolio.UI.Composition;

public partial class DataSection<TItem> : ComponentBase
{
    [Parameter][Required] public string? JsonUrl {get; set;}
    [Parameter][Required] public Type? ItemComponentType {get;set;}
    [Parameter] public string Class { get; set;} = "";
    [Parameter] public string Style { get; set;} = "";
    [Parameter] public FilterCollection<TItem>? Filters { get; set; }
    [Parameter] public DimensionalFilterCollection<TItem>? DimensionalFilters { get; set; }

    protected IEnumerable<TItem>? Items = null;
    protected IEnumerable<TItem>? DisplayItems = null;
    [Inject] private HttpClient? Http {get;set;}
    [Inject]
    private JsonSerializerOptions? JsonOptions {get; set;}

    protected override async Task OnInitializedAsync()
    {
        if(Http is null || string.IsNullOrWhiteSpace(JsonUrl)) return;

        var json = await Http.GetStringAsync(JsonUrl);

        var items = JsonSerializer.Deserialize<List<TItem>>(json, JsonOptions) ?? [];

        var comparer = ResolveComparer();

        Items = comparer is null
        ? new HashSet<TItem>(items)
        : new SortedSet<TItem>(items, comparer);
        
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        // Support both old FilterCollection and new DimensionalFilterCollection
        if (DimensionalFilters is not null)
        {
            DisplayItems = DimensionalFilters.Apply(Items!);
        }
        else if (Filters is not null)
        {
            DisplayItems = Filters.Apply(Items!);
        }
        else
        {
            DisplayItems = Items;
        }
    }

    private static Comparer<TItem>? ResolveComparer()
    {
        return typeof(IComparable<TItem>).IsAssignableFrom(typeof(TItem))
            ? Comparer<TItem>.Default
            : null;
    }
}