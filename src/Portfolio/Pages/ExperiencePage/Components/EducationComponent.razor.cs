using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.ExperiencePage.Components;

public partial class EducationComponent : ComponentBase
{
    [Parameter][Required]
    public Degree? Item {get; set;}
}