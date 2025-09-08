using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.Components;

public partial class SkillComponent : ComponentBase
{
    [Parameter][Required]
    public string? Skill {get; set;}
}