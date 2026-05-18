using Models.Career;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Portfolio.UI.Models;

public partial class WorkExperienceComponent : ComponentBase
{
    [Parameter][Required]
    public Experience? Item {get; set;}
}