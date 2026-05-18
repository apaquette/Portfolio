namespace Models;

/// <summary>
/// Represents a dimension of filtering (e.g., Categories, Technologies).
/// Manages a group of related filters that can be toggled independently.
/// Within a dimension, active filters use OR logic (item matches if it matches ANY active filter).
/// </summary>
/// <typeparam name="TItem">The type of items being filtered</typeparam>
public class FilterDimension<TItem>(string dimensionName, params IFilter<TItem>[] filters) : IFilterDimension<TItem>
{
    private readonly List<IFilter<TItem>> activeFilters = [];
    private readonly Dictionary<string, IFilter<TItem>> filtersByName = filters.ToDictionary(f => f.Name);

    /// <summary>
    /// Gets the name of this filter dimension.
    /// </summary>
    public string Name { get; } = dimensionName ?? throw new ArgumentNullException(nameof(dimensionName));

    /// <summary>
    /// Gets all available filters in this dimension.
    /// </summary>
    public IReadOnlyList<IFilter<TItem>> AvailableFilters => filters.ToList().AsReadOnly();

    /// <summary>
    /// Gets the currently active filters in this dimension.
    /// </summary>
    public IReadOnlyList<IFilter<TItem>> ActiveFilters => activeFilters.AsReadOnly();

    /// <summary>
    /// Gets the names of all available filters in this dimension.
    /// </summary>
    public IReadOnlyList<string> AvailableFilterNames => filtersByName.Keys.ToList().AsReadOnly();

    /// <summary>
    /// Gets the names of currently active filters in this dimension.
    /// </summary>
    public IReadOnlyList<string> ActiveFilterNames => activeFilters.Select(f => f.Name).ToList().AsReadOnly();

    /// <summary>
    /// Toggles a filter on or off within this dimension.
    /// </summary>
    public bool ToggleFilter(string filterName)
    {
        if (!filtersByName.TryGetValue(filterName, out var filter))
        {
            throw new ArgumentException($"Filter '{filterName}' not found in dimension '{Name}'.", nameof(filterName));
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
    /// Clears all active filters in this dimension.
    /// </summary>
    public void ClearFilters()
    {
        activeFilters.Clear();
    }

    /// <summary>
    /// Checks if a specific filter is currently active by name.
    /// </summary>
    public bool IsFilterActive(string filterName)
    {
        return activeFilters.Any(f => f.Name == filterName);
    }

    /// <summary>
    /// Determines whether any filters in this dimension are currently active.
    /// </summary>
    public bool AnyFiltersActive() => activeFilters.Count > 0;

    /// <summary>
    /// Determines whether an item matches this dimension's active filters.
    /// Uses OR logic within the dimension: item matches if it matches ANY active filter.
    /// </summary>
    public bool Matches(TItem item)
    {
        // If no filters are active, item passes this dimension
        if (!AnyFiltersActive())
            return true;

        // If filters are active, item must match at least one (OR logic)
        return activeFilters.Any(filter => filter.Matches(item));
    }
}
