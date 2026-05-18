using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Filtering.Core;
using Filtering.ProjectFilters;
using Models.UI.Sections;
using Models.Portfolio;
using Portfolio.Pages.Components;

namespace Portfolio.Pages.ProjectsPage;

[ExcludeFromCodeCoverage]
public partial class ProjectsPage : ComponentBase
{
    private DimensionalFilterCollection<Project>? projectFilters;

    protected readonly SectionDefinition[] Sections = [
        new("Projects", typeof(DataSection<Project>), 
            "data/projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem", false)
    ];

    protected override void OnInitialized()
    {
        projectFilters = new DimensionalFilterCollection<Project>();

        // Add Categories dimension
        var categoryDimension = new FilterDimension<Project>(
            "Categories",
            new ProjectCategoryFilter("Education"),
            new ProjectCategoryFilter("Personal"),
            new ProjectCategoryFilter("Client")
        );
        projectFilters.AddDimension(categoryDimension);

        // Add Technologies dimension
        var techDimension = new FilterDimension<Project>(
            "Technologies",
            new ProjectTechStackFilter(".NET"),
            new ProjectTechStackFilter("Python"),
            new ProjectCategoryFilter("Web"),
            new ProjectTechStackFilter("DevOps"),
            new ProjectCategoryFilter("Game")
        );
        projectFilters.AddDimension(techDimension);
    }
}