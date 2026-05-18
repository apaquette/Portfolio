namespace Filtering.Interfaces;

/// <summary>
/// Represents a dimension of filtering (e.g., Categories, Technologies).
/// A filter dimension groups related filters that can be toggled independently of other dimensions.
/// Filters within a dimension use OR logic (any match = pass), but dimensions themselves use AND logic.
/// </summary>
/// <typeparam name="TItem">The type of items being filtered</typeparam>
public interface IFilterDimension<TItem>
{
    /// <summary>
    /// Gets the name of this filter dimension (e.g., "Categories", "Technologies").
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets all available filters in this dimension.
    /// </summary>
    IReadOnlyList<IFilter<TItem>> AvailableFilters { get; }

    /// <summary>
    /// Gets the currently active filters in this dimension.
    /// </summary>
    IReadOnlyList<IFilter<TItem>> ActiveFilters { get; }

    /// <summary>
    /// Gets the names of all available filters in this dimension.
    /// </summary>
    IReadOnlyList<string> AvailableFilterNames { get; }

    /// <summary>
    /// Gets the names of currently active filters in this dimension.
    /// </summary>
    IReadOnlyList<string> ActiveFilterNames { get; }

    /// <summary>
    /// Toggles a filter on or off within this dimension.
    /// </summary>
    /// <param name="filterName">The name of the filter to toggle</param>
    /// <returns>True if the filter was activated; false if deactivated</returns>
    /// <exception cref="ArgumentException">Thrown if filter name doesn't exist in this dimension</exception>
    bool ToggleFilter(string filterName);

    /// <summary>
    /// Clears all active filters in this dimension.
    /// </summary>
    void ClearFilters();

    /// <summary>
    /// Checks if a specific filter is currently active by name.
    /// </summary>
    /// <param name="filterName">The name of the filter to check</param>
    /// <returns>True if the filter is active; otherwise, false</returns>
    bool IsFilterActive(string filterName);

    /// <summary>
    /// Determines whether any filters in this dimension are currently active.
    /// </summary>
    /// <returns>True if at least one filter is active; otherwise, false</returns>
    bool AnyFiltersActive();

    /// <summary>
    /// Determines whether an item matches this dimension's active filters.
    /// If no filters are active, returns true (item matches).
    /// If filters are active, returns true if item matches ANY active filter (OR logic within dimension).
    /// </summary>
    /// <param name="item">The item to test</param>
    /// <returns>True if the item matches any active filter in this dimension (or no filters are active)</returns>
    bool Matches(TItem item);
}
