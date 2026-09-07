using Validation.Dates;

namespace Models.Career;

public class Award : IComparable<Award>
{
    private DateOnly date;
    public Award(DateOnly date)
    {
        Date = date;
    }
    public string Title { get; set; } = "";
    public string Issuer { get; set; } = "";
    public DateOnly Date { 
        get => date;
        set {
            if (value == default)
                throw new MissingDateException("Award must contain a valid date.");
            date = value;
        }
    }
    public string Description { get; set; } = "";

    public int CompareTo(Award? other)
    {
        if (other is null) return 1;
        return other.Date.CompareTo(Date);
    }
}