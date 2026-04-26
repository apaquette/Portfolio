using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class CredibilityComponent : ComponentBase
{
    [Parameter][Required]
    public string Text {get; set;} = "";
    [Parameter]
    public RenderFragment? ChildContent {get; set;}
}