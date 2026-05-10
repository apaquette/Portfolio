using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.Components;

public partial class DataSection<TItem> : ComponentBase
{
    [Parameter][Required] public string? JsonUrl {get; set;}
    [Parameter][Required] public Type? ItemComponentType {get;set;}
    [Parameter] public string Class { get; set;} = "";
    [Parameter] public string Style { get; set;} = "";

    protected IEnumerable<TItem>? Items;
    [Inject] private HttpClient? Http {get;set;}


    protected override async Task OnInitializedAsync()
    {
        if(Http is null || string.IsNullOrWhiteSpace(JsonUrl)) return;

        var json = await Http.GetStringAsync(JsonUrl);

        var items = JsonSerializer.Deserialize<List<TItem>>(json) ?? [];

        var comparer = ResolveComparer();

        Items = comparer is null
        ? new HashSet<TItem>(items)
        : new SortedSet<TItem>(items, comparer);
    }

    private static IComparer<TItem>? ResolveComparer()
    {
        return typeof(IComparable<TItem>).IsAssignableFrom(typeof(TItem))
            ? Comparer<TItem>.Default
            : null;
    }
}