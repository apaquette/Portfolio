using Microsoft.AspNetCore.Components;
using Models;
using System.Text.Json;

namespace Portfolio.Pages.Sections;

public partial class PersonalProjects : ComponentBase
{
    protected SortedSet<Project> Projects = new();
    [Inject]
    private HttpClient? Http { get; set; }
    
    protected override async Task OnInitializedAsync(){
        string data = Http is not null ? await Http.GetStringAsync("data/personalProjects.json") : "";
        Projects = JsonSerializer.Deserialize<SortedSet<Project>>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new();
    }
}