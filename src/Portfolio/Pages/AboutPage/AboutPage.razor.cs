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
        // new("Background", typeof(Background), Centered: true),
        // new("Interests & Hobbies", typeof(Interests), Centered: true)
    ];
}