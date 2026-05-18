namespace Models;

/// <summary>
/// Represents a filter that can be applied to a collection of items.
/// Filters are composable and can be combined with other filters.
/// </summary>
/// <typeparam name="TItem">The type of items being filtered</typeparam>
public interface IFilter<TItem>
{
    /// <summary>
    /// Gets the name/label of this filter option.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Determines whether the given item matches this filter criteria.
    /// </summary>
    /// <param name="item">The item to test</param>
    /// <returns>True if the item matches this filter; otherwise, false</returns>
    bool Matches(TItem item);
}
