using System.Text.Json.Serialization;
using Validation.Dates;

namespace Models.Career;
public class Degree : IComparable<Degree>
{
    private DateOnly graduationDate;
    [JsonConstructor]
    public Degree(DateOnly graduationDate)
    {
        GraduationDate = graduationDate;
    }
    public string? Diploma { get; set; }
    public string? Institution { get; set; }
    [JsonPropertyName("graduationDate")]
    public DateOnly GraduationDate
    {
        get => graduationDate;
        set
        {
            if (value == default)
                throw new MissingDateException("Degree must contain a graduation date.");
            graduationDate = value;
        }
    }
    public string? Logo { get; set; }
    public string? Website { get; set; }

    public int CompareTo(Degree? other)
    {
        if (other is null) return 1;
        return other.GraduationDate.CompareTo(GraduationDate);
    }
}