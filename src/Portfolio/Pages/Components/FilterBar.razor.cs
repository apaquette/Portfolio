using Microsoft.AspNetCore.Components;
using Filtering.Core;

namespace Portfolio.Pages.Components;

public partial class FilterBar<TItem> : ComponentBase
{
    [Parameter] public DimensionalFilterCollection<TItem>? FilterCollectionDimensional { get; set; }

    [Parameter] public EventCallback OnFiltersChanged { get; set; }

    private bool AnyFiltersActive()
    {
        return FilterCollectionDimensional?.AnyFiltersActive() ?? false;
    }

    private async Task HandleFilterToggle(string dimensionName, string filterName)
    {
        if (FilterCollectionDimensional is not null)
        {
            var dimension = FilterCollectionDimensional.GetDimension(dimensionName);
            if (dimension != null)
            {
                dimension.ToggleFilter(filterName);
                await OnFiltersChanged.InvokeAsync();
            }
        }
    }

    private async Task ClearAllFilters()
    {
        if (FilterCollectionDimensional is not null)
        {
            FilterCollectionDimensional.ClearAllFilters();
            await OnFiltersChanged.InvokeAsync();
        }
    }
}