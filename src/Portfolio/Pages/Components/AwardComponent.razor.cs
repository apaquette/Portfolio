using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Models.Career;

namespace Portfolio.Pages.Components;

public partial class AwardComponent : ComponentBase
{
    [Parameter][Required]
    public Award? Item { get; set; }
}