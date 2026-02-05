using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.Components;

public partial class SectionComponent : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    public string? Title { get; set; }
    [Parameter]
    public string? Id { get; set; }
}