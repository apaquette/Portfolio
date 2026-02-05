using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Models;

namespace Portfolio.Pages.Home.Sections;

public partial class Certifications : ComponentBase
{
    protected SortedSet<Certification> CertificationSet = [];
    [Inject]
    private HttpClient? Http { get; set; }
    [Inject]
    private JsonSerializerOptions? JsonOptions {get; set;}
    protected override async Task OnInitializedAsync()
    {
        string data = Http is not null ? await Http.GetStringAsync("data/certifications.json") : "";
        CertificationSet = JsonSerializer.Deserialize<SortedSet<Certification>>(data, JsonOptions) ?? [];
    }
}