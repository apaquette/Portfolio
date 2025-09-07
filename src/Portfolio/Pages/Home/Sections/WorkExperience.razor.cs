using Microsoft.AspNetCore.Components;
using Models;
using System.Text.Json;

namespace Portfolio.Pages.Home.Sections;

public partial class WorkExperience : ComponentBase
{
    protected SortedSet<Experience> WorkExperiences = new();
    [Inject]
    private HttpClient? Http { get; set; }
    protected override async Task OnInitializedAsync(){
        string data = Http is not null ? await Http.GetStringAsync("data/workExperience.json") : "";
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        WorkExperiences = JsonSerializer.Deserialize<SortedSet<Experience>>(data, options) ?? new();
    }
}