namespace Models;
// Job title, company, dates, achievements
// Could use a vertical timeline format
public class Experience
{
    public string? Title { get; set; }
    public string? Company { get; set; }
    public DateTime JobStart { get; set; }
    public DateTime? JobEnd { get; set; }
    public string? Description { get; set; }
    public List<string> Skills { get; set; } = new();
}