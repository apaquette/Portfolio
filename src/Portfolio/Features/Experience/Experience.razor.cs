using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models.UI.Sections;
using Models.Career;
using Repositories;

using Portfolio.UI.Composition;
using Portfolio.UI.Models;

namespace Portfolio.Features.Experience;

[ExcludeFromCodeCoverage]
public partial class Experience : ComponentBase
{
    [Inject] protected JsonResourceFetcher ResourceFetcher { get; set; } = default!;
    protected readonly SectionDefinition[] ExperienceSections = [
        new("Work History", typeof(DataSection<Models.Career.Experience>)),
        new("Education", typeof(DataSection<Degree>)),
        new("Certifications", typeof(DataSection<Certification>)), 
        new("Recognition", typeof(DataSection<Award>))
    ];
    protected bool IsLoading { get; set; } = true;
    private IEnumerable<Models.Career.Experience>? workHistory;
    private IEnumerable<Degree>? degrees;
    private IEnumerable<Certification>? certifications;
    private IEnumerable<Award>? awards;

    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        workHistory = await ResourceFetcher.GetAllAsync<Models.Career.Experience>();
        degrees = await ResourceFetcher.GetAllAsync<Degree>();
        certifications = await ResourceFetcher.GetAllAsync<Certification>();
        awards = await ResourceFetcher.GetAllAsync<Award>();
        IsLoading = false;
    }

    protected Dictionary<string, object?> DetermineParameters(string title)
    {
        return title switch
        {
            "Work History" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(WorkExperienceComponent),
                ["Items"] = workHistory,
                ["Class"] = "",
                ["Style"] = "margin-left: -0.5rem;"
            },
            "Education" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(EducationComponent),
                ["Items"] = degrees,
                ["Class"] = "",
                ["Style"] = "margin-left: -0.5rem;"
            },
            "Certifications" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(CertificationComponent),
                ["Items"] = certifications,
                ["Class"] = "d-flex flex-wrap justify-content-start",
                ["Style"] = "margin-left: -0.5rem;"
            },
            "Recognition" => new Dictionary<string, object?> 
            { 
                ["ItemComponentType"] = typeof(AwardComponent),
                ["Items"] = awards,
                ["Class"] = "d-flex flex-wrap justify-content-start",
                ["Style"] = "margin: 0;"
            },
            _ => []
        };
    }
}