using Microsoft.AspNetCore.Components;
using Models;
using System.Text.Json;

namespace Portfolio.Pages.Home.Sections;

public partial class WorkExperience : ComponentBase
{
    protected SortedSet<Experience> WorkExperiences = [];
    [Inject]
    private HttpClient? Http { get; set; }
    [Inject]
    private JsonSerializerOptions? JsonOptions {get; set;}
    protected override async Task OnInitializedAsync()
    {
        string data = Http is not null ? await Http.GetStringAsync("data/workExperience.json") : "";
        WorkExperiences = JsonSerializer.Deserialize<SortedSet<Experience>>(data, JsonOptions) ?? [];
    }
}