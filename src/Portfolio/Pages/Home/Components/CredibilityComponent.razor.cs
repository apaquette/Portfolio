using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Home.Components;

public partial class CredibilityComponent : ComponentBase
{
    [Parameter][Required]
    public string Text {get; set;} = "";
    [Parameter]
    public RenderFragment? ChildContent {get; set;}
}