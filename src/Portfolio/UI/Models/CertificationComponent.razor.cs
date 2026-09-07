using Microsoft.AspNetCore.Components;
using Models.Career;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.UI.Models;

public partial class CertificationComponent : ComponentBase
{
    [Parameter][Required]
    public Certification? Item { get; set; }
}