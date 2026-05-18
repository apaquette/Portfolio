using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models.UI.Sections;
using Models.Career;

using Portfolio.UI.Composition;
using Portfolio.UI.Models;

namespace Portfolio.Features.Experience;

[ExcludeFromCodeCoverage]
public partial class Experience : ComponentBase
{
    protected readonly SectionDefinition[] ExperienceSections = [
        new("Work History", typeof(DataSection<Models.Career.Experience>), 
            "data/workExperience.json", typeof(WorkExperienceComponent),
            "", "margin-left: -0.5rem;"),
        new("Education", typeof(DataSection<Degree>), "data/degrees.json", typeof(EducationComponent)),
        new("Certifications", typeof(DataSection<Certification>), 
            "data/certifications.json", typeof(CertificationComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem;"),
        new("Recognition", typeof(DataSection<Award>), 
            "data/awards.json", typeof(AwardComponent),
            "d-flex flex-wrap justify-content-start", "margin: 0;"),
    ];
}