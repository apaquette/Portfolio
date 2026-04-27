using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.Components;

namespace Portfolio.Pages.AboutPage;

public partial class AboutPage : ComponentBase
{
    protected readonly SectionDefinition[] AboutSections = [
        new(null, typeof(Hero)),
        // new(null, typeof(AboutHero)),
        // new("Background", typeof(Background), Centered: true),
        // new("Interests & Hobbies", typeof(Interests), Centered: true)
    ];
}