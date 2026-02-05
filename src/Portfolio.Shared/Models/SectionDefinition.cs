using System.Dynamic;

namespace Models;

public sealed record SectionDefinition(
    string Title,
    string? Id,
    Type ComponentType,
    string? JsonUrl = null,
    Type? DataItemComponentType = null,
    string? Class = "",
    string? Style = ""
);