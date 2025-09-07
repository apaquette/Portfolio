using Microsoft.AspNetCore.Components;
using System.Text.Json;
using Models;

namespace Portfolio.Pages.Home.Sections;

public partial class ClientProjects : ComponentBase
{
    protected SortedSet<Project> Projects = new();
    [Inject]
    private HttpClient? Http { get; set; }
    protected override async Task OnInitializedAsync()
    {
        string data = Http is not null ? await Http.GetStringAsync("data/clientProjects.json") : "";
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        Projects = JsonSerializer.Deserialize<SortedSet<Project>>(data, options) ?? new();
    }
}