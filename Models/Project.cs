namespace Models;

public class Project
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<string> TechStack { get; set; } = new();
    public string? Link { get; set; }
    public string? ImageSource { get; set; }
}