using Microsoft.AspNetCore.Components;
using Models.Career;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.UI.Models;

public partial class EducationComponent : ComponentBase
{
    [Parameter][Required]
    public Degree? Item {get; set;}
}