namespace Models.UI.Sections;

public sealed record SectionDefinition(
    string? Title,
    Type ComponentType,
    Type? DataItemComponentType = null,
    string Class = "",
    string Style = "",
    bool Centered = false
);