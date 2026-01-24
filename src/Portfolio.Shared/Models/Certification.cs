namespace Models;

public class Certification : IComparable<Certification>
{
    public string? Name { get; set; }
    public DateOnly EarnedOn { get; set; }
    public string? IssuedBy { get; set; }
    public string? Icon { get; set; }
    public string? Link { get; set; }

    public int CompareTo(Certification? other)
    {
        if (other is null) return 1;
        return other.EarnedOn.CompareTo(EarnedOn);
    }
}