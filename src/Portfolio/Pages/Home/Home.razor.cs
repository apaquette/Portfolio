using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;
using Portfolio.Pages.Home.Sections;

namespace Portfolio.Pages.Home;

public partial class Home : ComponentBase
{
    protected readonly SectionDefinition[] HeroSections = [
      new("Summary", null, typeof(Summary)),
      new("Education", null, typeof(DataSection<Degree>), "data/degrees.json", typeof(EducationComponent))

    ];
    protected readonly SectionDefinition[] SectionsList = 
    [
        new("About", "about", typeof(About)),
        new("Projects", "projects", typeof(DataSection<Project>), 
            "data/projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem;"),
        new("Work History", "work", typeof(DataSection<Experience>), 
            "data/workExperience.json", typeof(WorkExperienceComponent),
            "", "margin-left: -0.5rem;"),
        new("Certifications", "certifications", typeof(DataSection<Certification>), 
            "data/certifications.json", typeof(CertificationComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem;"),
        new("Skills", "skills", typeof(Skills))
    ];
}