using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Filtering.Core;
using Filtering.ProjectFilters;
using Models.UI.Sections;
using Models.Portfolio;
using Portfolio.UI.Composition;
using Portfolio.UI.Models;
using Repositories;

namespace Portfolio.Features.Projects;

[ExcludeFromCodeCoverage]
public partial class Projects : ComponentBase
{
    private bool IsLoading { get; set; } = true;
    [Inject] protected ProjectRepository ProjectRepository { get; set; } = default!;
    private DimensionalFilterCollection<Project>? projectFilters;
    private IEnumerable<Project>? projects;

    protected readonly SectionDefinition[] Sections = [
        new("Projects", typeof(DataSection<Project>), 
            typeof(ProjectComponent), "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem", false)
    ];

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        AssignFilters();
        projects = await ProjectRepository.GetAllAsync();

        IsLoading = false;
    }

    private void AssignFilters()
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
    
    protected Dictionary<string, object?> DetermineParameters(string title)
    {
        return title switch
        {
            "Projects" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(ProjectComponent),
                ["Items"] = projects,
                ["Class"] = "d-flex flex-wrap justify-content-start",
                ["Style"] = "margin-left: -0.5rem;",
                ["DimensionalFilters"] = projectFilters
            },
            _ => []
        };
     }
}