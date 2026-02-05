using Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.Pages.Components;

public partial class WorkExperienceComponent : ComponentBase
{
    [Parameter][Required]
    public Experience? Item {get; set;}
}