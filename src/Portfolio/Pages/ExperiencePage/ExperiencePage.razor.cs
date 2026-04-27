using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;
using Portfolio.Pages.ExperiencePage.Components;

namespace Portfolio.Pages.ExperiencePage;

public partial class ExperiencePage : ComponentBase
{
    protected readonly SectionDefinition[] ExperienceSections = [
        new("Work History", typeof(DataSection<Experience>), 
            "data/workExperience.json", typeof(WorkExperienceComponent),
            "", "margin-left: -0.5rem;"),
        new("Education", typeof(DataSection<Degree>), "data/degrees.json", typeof(EducationComponent)),
        new("Certifications", typeof(DataSection<Certification>), 
            "data/certifications.json", typeof(CertificationComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem;"),
    ];
}