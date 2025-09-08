using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class ProjectComponent : ComponentBase
{
    [Parameter][Required]
    public Project? Project { get; set; }
}