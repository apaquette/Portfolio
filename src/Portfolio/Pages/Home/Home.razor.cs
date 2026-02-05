using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Home.Sections;

namespace Portfolio.Pages.Home;

public partial class Home : ComponentBase
{
    protected readonly SectionDefinition[] HeroSections = [
      new("Summary", null, typeof(Summary)),
      new("Education", null, typeof(Education))

    ];
    protected readonly SectionDefinition[] SectionsList = 
    [
        new("About", "about", typeof(About)),
        new("Projects", "projects", typeof(Projects)),
        new("Work History", "work", typeof(WorkExperience)),
        new("Certifications", "certifications", typeof(Certifications)),
        new("Skills", "skills", typeof(Skills))
    ];
}