namespace Models;

public class Degree : IComparable<Degree>
{

    public string? Diploma { get; set; }
    public string? Institution { get; set; }
    public DateOnly GraduationDate { get; set; }
    public string? Logo { get; set; }
    public string? Website { get; set; }

    public int CompareTo(Degree? other)
    {
        if(other is null) return 1;
        return other.GraduationDate.CompareTo(GraduationDate);
    }
}