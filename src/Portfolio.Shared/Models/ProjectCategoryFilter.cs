namespace Models;

public class ProjectCategoryFilter(string category) : IFilter<Project>
{
    private readonly string category = category ?? throw new ArgumentNullException(nameof(category));

    /// <summary>
    /// Gets the name of this filter (the category it filters by).
    /// </summary>
    public string Name { get; } = category;

    /// <summary>
    /// Determines whether the project's category matches the specified category.
    /// </summary>
    public bool Matches(Project item)
    {
        return item.Categories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }
}