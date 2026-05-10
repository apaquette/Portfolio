using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;
using Portfolio.Pages.Home.Sections;

namespace Portfolio.Pages.Home;
[ExcludeFromCodeCoverage]
public partial class Home : ComponentBase
{
    protected readonly SectionDefinition[] HomeSections = [
        new(null, typeof(Hero)),
        new(null, typeof(Summary)),
        new(null, typeof(Buttons)),
        new(null, typeof(CredibilityStrip)),
        new("Technical Strengths", typeof(Skills), Centered: true),
        new("Featured Projects", typeof(DataSection<Project>), 
            "data/projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-center gap-3", "margin: 0;", true),
        // Latest Articles
        // Awards
        new("About", typeof(About), Centered: true)
    ];
}