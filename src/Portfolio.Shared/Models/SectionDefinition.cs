namespace Models;

public sealed record SectionDefinition(
    string Title,
    string? Id,
    Type ComponentType
);