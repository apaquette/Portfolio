using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class CardComponent : ComponentBase
{
    [Parameter][Required]
    public string Title { get; set; } = "";
    [Parameter]
    public string? Link { get; set; }
    [Parameter]
    public RenderFragment? Icon { get; set; }
    [Parameter]
    public string? Subtitle { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    [Parameter]
    public string[]? Tags { get; set; }
}