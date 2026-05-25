using Microsoft.AspNetCore.Components;

using System.Diagnostics.CodeAnalysis;

using Models.UI.Sections;
using Models.Portfolio;

using Portfolio.UI.Models;
using Portfolio.UI.Composition;
using Portfolio.Features.Home.Sections;
using Repositories;

namespace Portfolio.Features.Home;

[ExcludeFromCodeCoverage]
public partial class Home : ComponentBase
{
    protected bool IsLoading { get; set; } = true;
    [Inject] protected ProjectRepository ProjectRepository { get; set; } = default!;
    protected readonly SectionDefinition[] Sections = [
        new(null, typeof(Hero)),
        new(null, typeof(Summary)),
        new(null, typeof(Buttons)),
        new(null, typeof(CredibilityStrip)),
        new("Technical Strengths", typeof(Skills), Centered: true),
        new("Featured Projects", typeof(DataSection<Project>), Centered: true),
        // // Latest Articles
        new("About", typeof(Sections.About), Centered: true)
    ];

    private IEnumerable<Project>? projects;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        projects = await ProjectRepository.GetFeaturedAsync();
        IsLoading = false;
    }

    protected Dictionary<string, object?> DetermineParameters(string title)
    {
        return title switch
        {
            "Featured Projects" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(ProjectComponent),
                ["Items"] = projects,
                ["Class"] = "d-flex flex-wrap justify-content-center gap-3",
                ["Style"] = "margin: 0;",
                ["Filters"] = null
            },
            _ => []
        };
     }
}