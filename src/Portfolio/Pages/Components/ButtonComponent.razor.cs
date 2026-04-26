using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class ButtonComponent : ComponentBase
{
    [Parameter][Required]
    public string Title { get; set; } = "";
    [Parameter][Required]
    public string Link { get; set; } = "";
}