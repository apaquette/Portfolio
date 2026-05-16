using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;

namespace Portfolio.Pages.ProjectsPage;
[ExcludeFromCodeCoverage]
public partial class ProjectsPage : ComponentBase
{
    protected readonly SectionDefinition[] Sections = [
        new("Projects", typeof(DataSection<Project>), 
            "data/projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem", false)
    ];
}