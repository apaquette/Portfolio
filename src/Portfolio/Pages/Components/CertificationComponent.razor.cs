using Microsoft.AspNetCore.Components;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Pages.Components;

public partial class CertificationComponent : ComponentBase
{
    [Parameter][Required]
    public Certification? Certification { get; set; }
}