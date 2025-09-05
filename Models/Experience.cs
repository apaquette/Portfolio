namespace Models;

public class Experience : IComparable<Experience>
{
    public string? Title { get; set; }
    public string? Company { get; set; }
    public DateTime JobStart { get; set; }
    public DateTime? JobEnd { get; set; }
    public string? Description { get; set; }
    public SortedSet<string> Skills { get; set; } = new();
    public string? Location { get; set; }
    public string? EmployerSite { get; set; }

    public int CompareTo(Experience? other)
    {
        if (other is null) return 1;
        return other.JobEnd?.CompareTo(JobEnd) ?? 1;
    }

    public string Duration()
    {
        TimeSpan duration = (JobEnd ?? DateTime.Now) - JobStart;
        int totalDays = (int)duration.TotalDays;
        int years = totalDays / 365;
        int remainingDays = totalDays % 365;
        int months = remainingDays / 30;
        string yearDuration = years >= 1 ? $"{years} yrs " : "";
        string monthDuration = months > 0 ? $"{months} mos" : "";
        return $"{yearDuration}{monthDuration}";
    } 
}