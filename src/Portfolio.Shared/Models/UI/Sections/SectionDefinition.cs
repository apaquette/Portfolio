namespace Models.UI.Sections;

public sealed record SectionDefinition(
    string? Title,
    Type ComponentType,
    string? JsonUrl = null,
    Type? DataItemComponentType = null,
    string Class = "",
    string Style = "",
    bool Centered = false
);