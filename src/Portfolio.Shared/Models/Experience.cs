using Exceptions;

namespace Models;
//TODO: Implement unit tests
public class Experience : IComparable<Experience>
{
    public string? Title { get; set; }
    public string? Company { get; set; }
    private DateTime? jobEnd;
    public DateTime JobStart { get; set; }
    public DateTime? JobEnd
    {
        get { return jobEnd; }
        set
        {
            if (value <= JobStart) throw new InvalidDateException("JobEnd cannot be before or equal to JobStart");
            jobEnd = value;
        }
    }
    public string? Description { get; set; }
    public SortedSet<string> Skills { get; set; } = new();
    public string? Location { get; set; }
    public string? EmployerSite { get; set; }

    public Experience(DateTime? date)
    {
        JobStart = date ?? throw new MissingDateException("Experience needs a start date.");
    }

    public int CompareTo(Experience? other)
    {
        if (other is null) return 1;
        DateTime otherDate = other.JobEnd ?? DateTime.Today;
        DateTime date = JobEnd ?? DateTime.Today;

        return otherDate.CompareTo(date);
    }

    public string Duration()
    {
        TimeSpan duration = (JobEnd ?? DateTime.Now) - JobStart;
        int totalDays = (int)duration.TotalDays;
        int years = totalDays / 365;
        int remainingDays = totalDays % 365;
        int months = remainingDays / 30;
        string yearDuration = years >= 1 ? $"{years} yrs" : "";
        string monthDuration = months > 0 ? $"{months} mos" : "";
        string space = yearDuration != "" && monthDuration != "" ? " ": "";
        return $"{yearDuration}{space}{monthDuration}";
    } 
}