using System.Text.RegularExpressions;
using Filtering.Interfaces;
using Models.Portfolio;

namespace Filtering.ProjectFilters;

/// <summary>
/// A filter that checks if a project's TechStack contains a specific technology.
/// </summary>
public class ProjectTechStackFilter(string technology) : IFilter<Project>
{
    private readonly string technology = technology ?? throw new ArgumentNullException(nameof(technology));

    /// <summary>
    /// Gets the name of this filter (the technology it filters by).
    /// </summary>
    public string Name { get; } = technology;

    /// <summary>
    /// Determines whether the project's tech stack contains the specified using regex to match the pattern.
    /// </summary>
    public bool Matches(Project item)
    {
        string pattern = $@".*{Regex.Escape(technology)}.*";
        bool result = item.TechStack.Any(item => Regex.IsMatch(item, pattern, RegexOptions.IgnoreCase));
        return result;
    }
}
