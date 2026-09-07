namespace Models.Career;

public sealed record SkillCategory
{
    public string Name { get; set; } = string.Empty;
    public SortedSet<string> Skills { get; set; } = [];
}