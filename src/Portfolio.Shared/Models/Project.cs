using Exceptions;

namespace Models;
public class Project : IComparable<Project>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public SortedSet<string> TechStack { get; set; } = new();
    public string? Link { get; set; }
    public string? ImageSource { get; set; }
    public DateOnly Completed { get; set; }

    public Project(DateOnly? date)
    {
        Completed = date ?? throw new MissingDateException("Project must have a completion date.");
    }

    public int CompareTo(Project? other)
    {
        if (other is null) return 1;
        return other.Completed.CompareTo(Completed);
    }
}