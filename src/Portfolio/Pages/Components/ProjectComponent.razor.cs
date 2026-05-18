using Microsoft.AspNetCore.Components;
using Models.Portfolio;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class ProjectComponent : ComponentBase
{
    [Parameter][Required]
    public Project? Item { get; set; }
}