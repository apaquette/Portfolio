using Filtering.Interfaces;
namespace Filtering.Core;

/// <summary>
/// Manages a collection of filters and applies them to items.
/// Supports combining multiple filters with AND logic (all must match).
/// </summary>
/// <typeparam name="TItem">The type of items being filtered</typeparam>
public class FilterCollection<TItem>(params IFilter<TItem>[] filters)
{
    private readonly List<IFilter<TItem>> activeFilters = [];

    /// <summary>
    /// Gets the currently active filters.
    /// </summary>
    public IReadOnlyList<IFilter<TItem>> ActiveFilters => activeFilters.AsReadOnly();

    /// <summary>
    /// Gets all available filter options.
    /// </summary>
    public IReadOnlyList<string> AvailableFilterNames => filtersByName.Keys.ToList().AsReadOnly();

    private readonly Dictionary<string, IFilter<TItem>> filtersByName = filters.ToDictionary(f => f.Name);

    /// <summary>
    /// Toggles a filter on or off by name.
    /// </summary>
    /// <param name="filterName">The name of the filter to toggle</param>
    /// <returns>True if the filter was activated; false if deactivated</returns>
    /// <exception cref="ArgumentException">Thrown if filter name doesn't exist</exception>
    public bool ToggleFilter(string filterName)
    {
        if (!filtersByName.TryGetValue(filterName, out var filter))
        {
            throw new ArgumentException($"Filter '{filterName}' not found.", nameof(filterName));
        }

        var index = activeFilters.FindIndex(f => f.Name == filterName);
        if (index >= 0)
        {
            activeFilters.RemoveAt(index);
            return false;
        }

        activeFilters.Add(filter);
        return true;
    }

    /// <summary>
    /// Sets the active filters by name.
    /// </summary>
    /// <param name="filterNames">The names of filters to activate. Other filters will be deactivated.</param>
    public void SetActiveFilters(params string[] filterNames)
    {
        activeFilters.Clear();
        foreach (var name in filterNames)
        {
            if (!filtersByName.TryGetValue(name, out var filter))
            {
                throw new ArgumentException($"Filter '{name}' not found.", nameof(filterNames));
            }
            activeFilters.Add(filter);
        }
    }

    /// <summary>
    /// Clears all active filters.
    /// </summary>
    public void ClearFilters()
    {
        activeFilters.Clear();
    }

    /// <summary>
    /// Determines whether an item matches all active filters.
    /// </summary>
    /// <param name="item">The item to test</param>
    /// <returns>True if the item matches all active filters (or no filters are active); otherwise, false</returns>
    public bool Matches(TItem item)
    {
        return activeFilters.All(filter => filter.Matches(item));
    }

    /// <summary>
    /// Filters a collection of items based on active filters.
    /// </summary>
    /// <param name="items">The items to filter</param>
    /// <returns>A new collection containing only items that match all active filters</returns>
    public IEnumerable<TItem> Apply(IEnumerable<TItem> items)
    {
        return items.Where(Matches);
    }

    /// <summary>
    /// Checks if a specific filter is currently active by name.
    /// </summary>
    /// <param name="filterName">The name of the filter to check</param>
    /// <returns>True if the filter is active; otherwise, false</returns>
    public bool IsFilterActive(string filterName)
    {
        return activeFilters.Any(f => f.Name == filterName);
    }

    public bool AnyFiltersActive() => activeFilters.Count > 0;
}
