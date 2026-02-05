using Microsoft.AspNetCore.Components;
using Models;
using System.Text.Json;

namespace Portfolio.Pages.Home.Sections;

public partial class Projects : ComponentBase
{
    protected SortedSet<Project> ProjectsSet = [];
    [Inject]
    private HttpClient? Http { get; set; }
    
    protected override async Task OnInitializedAsync(){
        string data = Http is not null ? await Http.GetStringAsync("data/projects.json") : "";
        ProjectsSet = JsonSerializer.Deserialize<SortedSet<Project>>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? [];
    }
}