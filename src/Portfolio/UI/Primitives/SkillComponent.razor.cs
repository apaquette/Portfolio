using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.UI.Primitives;

public partial class SkillComponent : ComponentBase
{
    [Parameter][Required]
    public string? Skill {get; set;}
}