using Exceptions;
using System.Text.Json.Serialization;

namespace Models;
public class Experience : IComparable<Experience>
{
    private DateTime jobStart;
    private DateTime? jobEnd;
    [JsonConstructor]
    public Experience(DateTime jobStart)
    {
        JobStart = jobStart;
    }
    public string? Title { get; set; }
    public string? Company { get; set; }
    [JsonPropertyName("jobStart")]
    public DateTime JobStart
    {
        get => jobStart;
        set
        {
            if (value == default)
                throw new MissingDateException("Experience needs a start date.");
            jobStart = value;
        }
    }
    
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
    public SortedSet<string> Skills { get; set; } = [];
    public string? Location { get; set; }
    public string? EmployerSite { get; set; }


    public int CompareTo(Experience? other)
    {
        if (other is null) return 1;
        DateTime otherDate = other.JobEnd ?? DateTime.Today;
        DateTime date = JobEnd ?? DateTime.Today;

        return otherDate.CompareTo(date);
    }

    public string Duration()
    {
        DateTime endDate = JobEnd ?? DateTime.Now;
        
        // Calculate years and months mathematically
        int years = endDate.Year - JobStart.Year;
        int months = endDate.Month - JobStart.Month;
        
        // Adjust if the day hasn't been reached in the current month
        if (endDate.Day < JobStart.Day)
            months--;
        
        // Adjust if months went negative
        if (months < 0)
        {
            years--;
            months += 12;
        }

        string yearDuration = FormatYears(years);
        string monthDuration = FormatMonths(months);
        string space = (yearDuration != "" && monthDuration != "") ? " " : "";

        string result = $"{yearDuration}{space}{monthDuration}".Trim();
        return string.IsNullOrWhiteSpace(result) ? "" : $"({result})";
    }

    private static string FormatYears(int years)
    {
        if (years < 1)
            return "";
        
        return $"{years} yr{(years > 1 ? "s" : "")}";
    }

    private static string FormatMonths(int months)
    {
        if (months <= 0)
            return "";
        
        return $"{months} mo{(months > 1 ? "s" : "")}";
    } 
}