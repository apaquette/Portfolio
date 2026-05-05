using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.AboutPage.Sections;

public partial class OutsideWork : ComponentBase
{
    protected OutsideWorkModel[] OutsideWorkItems =
    [
        new() 
        {
            Title = "Community Leadership",
            Description = "During my time in Ireland, I took initiative to grow and organize the local Pokemon Go community, and it's rewarding to see it still thriving today.",
            Tags = [ "Pokemon Go", "Meetups", "Ireland" ],
            Icon = builder =>
            {
                builder.OpenElement(1, "i");
                builder.AddAttribute(2, "class", "bi bi-people-fill fs-3");
                builder.CloseElement();
            }
        },
        new()
        {
            Title = "Technical Exploration",
            Description = "From moving permanently to NixOS, to building self-hosted media and automation systems, I enjoy exploring infrastructure, tooling, and technology that powers modern workflows.",
            Tags = [ "Linux", "NixOS", "Self-Hosting" ],
            Icon = builder =>
            {                
                builder.OpenElement(1, "i");
                builder.AddAttribute(2, "class", "bi bi-terminal-fill fs-3");
                builder.CloseElement();
            }
            
        },
        new()
        {
            Title = "Systems Beyond Software",
            Description = "I have a strong interest in urbanism, public transit, and walkable cities which reflects the same systems thinking I bring to software design.",
            Tags = [ "Urbanism", "Transit", "Walkability" ],
            Icon = builder =>
            {                
                builder.OpenElement(1, "i");
                builder.AddAttribute(2, "class", "bi bi-train-lightrail-front-fill fs-3");
                builder.CloseElement();
            }
            
        }
    ];
}

public class OutsideWorkModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public RenderFragment Icon { get; set; } = builder => { };
}