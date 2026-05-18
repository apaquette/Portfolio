using Microsoft.AspNetCore.Components;

using System.Diagnostics.CodeAnalysis;

using Models.UI.Sections;
using Models.Portfolio;

using Portfolio.UI.Models;
using Portfolio.UI.Composition;
using Portfolio.Features.Home.Sections;

namespace Portfolio.Features.Home;

[ExcludeFromCodeCoverage]
public partial class Home : ComponentBase
{
    protected readonly SectionDefinition[] Sections = [
        new(null, typeof(Hero)),
        new(null, typeof(Summary)),
        new(null, typeof(Buttons)),
        new(null, typeof(CredibilityStrip)),
        new("Technical Strengths", typeof(Skills), Centered: true),
        new("Featured Projects", typeof(DataSection<Project>), 
            "data/featured-projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-center gap-3", "margin: 0;", true),
        // Latest Articles
        new("About", typeof(Sections.About), Centered: true)
    ];
}