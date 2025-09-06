using Exceptions;
using System.Text.Json.Serialization;

namespace Models;
public class Project : IComparable<Project>
{
    private DateOnly completionDate;
    [JsonConstructor]
    public Project(DateOnly completed)
    {
        Completed = completed;
    }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public SortedSet<string> TechStack { get; set; } = [];
    public string? Link { get; set; }
    public string? ImageSource { get; set; }
    [JsonPropertyName("completed")]
    public DateOnly Completed
    {
        get => completionDate;
        set
        {
            if (value == default)
                throw new MissingDateException("Project must have a completion date.");
            completionDate = value;
        }
    }


    public int CompareTo(Project? other)
    {
        if (other is null) return 1;
        return other.Completed.CompareTo(Completed);
    }
}