using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.AboutPage.Sections;
using Portfolio.Pages.Components;

namespace Portfolio.Pages.AboutPage;

public partial class AboutPage : ComponentBase
{
    protected readonly SectionDefinition[] AboutSections = [
        new(null, typeof(Hero)),
        new("My Journey", typeof(Introduction)),
        new("How I work", typeof(WorkingStyle)),
    ];
}