using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Models;
using Portfolio.Pages.AboutPage.Sections;
using Portfolio.Pages.Components;

namespace Portfolio.Pages.AboutPage;

[ExcludeFromCodeCoverage]
public partial class AboutPage : ComponentBase
{
    protected readonly SectionDefinition[] AboutSections = [
        new(null, typeof(Hero)),
        new("My Journey", typeof(Introduction)),
        new("How I Work", typeof(WorkingStyle)),
        new("Outside of Work", typeof(OutsideWork))
    ];
}