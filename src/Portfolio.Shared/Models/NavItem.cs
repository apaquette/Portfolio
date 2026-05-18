namespace Models;

public sealed record NavItem
{
    public string Label { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
}
