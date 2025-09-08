using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class EducationComponent : ComponentBase
{
    [Parameter][Required]
    public Degree? Degree {get; set;}
}