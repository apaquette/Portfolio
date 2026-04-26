using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;
using Portfolio.Pages.Home.Sections;

namespace Portfolio.Pages.Home;

public partial class Home : ComponentBase
{
    protected readonly SectionDefinition[] HeroSections = [
        new(null, null, typeof(Hero)),
        new(null, null, typeof(Summary)),
        new(null, null, typeof(Buttons)),
        new(null, null, typeof(CredibilityStrip)),
        new("Technical Strengths", "skills", typeof(Skills)),
        new("Featured Projects", "projects", typeof(DataSection<Project>), 
            "data/projects.json", typeof(ProjectComponent),
            "d-flex flex-wrap justify-content-start", "margin-left: -0.5rem;"),
        // Latest Articles
        // Awards
        new("About", "about", typeof(About))
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
        
    ];
}