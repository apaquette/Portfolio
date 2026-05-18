namespace Models;

/// <summary>
/// Manages multiple filter dimensions and applies them to items.
/// Supports combining multiple filter dimensions with AND logic (all dimensions must match).
/// Within each dimension, filters use OR logic (any match = pass).
/// </summary>
/// <typeparam name="TItem">The type of items being filtered</typeparam>
public class DimensionalFilterCollection<TItem>
{
    private readonly Dictionary<string, IFilterDimension<TItem>> dimensionsByName = [];

    /// <summary>
    /// Gets all available filter dimensions.
    /// </summary>
    public IReadOnlyList<string> DimensionNames => dimensionsByName.Keys.ToList().AsReadOnly();

    /// <summary>
    /// Gets a specific dimension by name.
    /// </summary>
    /// <param name="dimensionName">The name of the dimension</param>
    /// <returns>The filter dimension, or null if not found</returns>
    public IFilterDimension<TItem>? GetDimension(string dimensionName)
    {
        return dimensionsByName.TryGetValue(dimensionName, out var dimension) ? dimension : null;
    }

    /// <summary>
    /// Adds a filter dimension to the collection.
    /// </summary>
    /// <param name="dimension">The dimension to add</param>
    /// <exception cref="ArgumentException">Thrown if a dimension with the same name already exists</exception>
    public void AddDimension(IFilterDimension<TItem> dimension)
    {
        if (dimensionsByName.ContainsKey(dimension.Name))
        {
            throw new ArgumentException($"Dimension '{dimension.Name}' already exists.", nameof(dimension));
        }

        dimensionsByName[dimension.Name] = dimension;
    }

    /// <summary>
    /// Determines whether an item matches all active filter dimensions.
    /// An item passes if it matches all active dimensions (AND logic between dimensions).
    /// Within each dimension, an item matches if it matches any active filter (OR logic).
    /// </summary>
    /// <param name="item">The item to test</param>
    /// <returns>True if the item matches all active dimensions; otherwise, false</returns>
    public bool Matches(TItem item)
    {
        return dimensionsByName.Values.All(dimension => dimension.Matches(item));
    }

    /// <summary>
    /// Filters a collection of items based on all active filters across all dimensions.
    /// </summary>
    /// <param name="items">The items to filter</param>
    /// <returns>A new collection containing only items that match all active dimensions</returns>
    public IEnumerable<TItem> Apply(IEnumerable<TItem> items)
    {
        return items.Where(Matches);
    }

    /// <summary>
    /// Clears all active filters across all dimensions.
    /// </summary>
    public void ClearAllFilters()
    {
        foreach (var dimension in dimensionsByName.Values)
        {
            dimension.ClearFilters();
        }
    }

    /// <summary>
    /// Determines whether any filters are active across any dimension.
    /// </summary>
    /// <returns>True if at least one filter is active in any dimension; otherwise, false</returns>
    public bool AnyFiltersActive()
    {
        return dimensionsByName.Values.Any(d => d.AnyFiltersActive());
    }

    /// <summary>
    /// Gets all active filters across all dimensions, grouped by dimension name.
    /// </summary>
    /// <returns>A dictionary mapping dimension names to their active filter names</returns>
    public Dictionary<string, IReadOnlyList<string>> GetActiveFilters()
    {
        return dimensionsByName
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ActiveFilterNames);
    }
}
