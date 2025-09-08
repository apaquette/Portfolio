using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.Components;

public partial class SkillSetComponent : ComponentBase
{
    [Parameter][Required]
    public KeyValuePair<string, SortedSet<string>> SkillSet {get;set;}
}