using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models.UI.Sections;
using Portfolio.Features.About.Sections;
using Portfolio.UI.Composition;

namespace Portfolio.Features.About;

[ExcludeFromCodeCoverage]
public partial class About : ComponentBase
{
    protected readonly SectionDefinition[] Sections = [
        new(null, typeof(Hero)),
        new("My Journey", typeof(Introduction)),
        new("How I Work", typeof(WorkingStyle)),
        new("Outside of Work", typeof(OutsideWork))
    ];
}