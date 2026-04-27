using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.ExperiencePage.Components;

public partial class CertificationComponent : ComponentBase
{
    [Parameter][Required]
    public Certification? Item { get; set; }
}