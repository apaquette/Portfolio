using Microsoft.AspNetCore.Components;
using Models;
using System.Text.Json;

namespace Portfolio.Pages.Home.Sections;

public partial class Education : ComponentBase
{
    protected SortedSet<Degree> Degrees = new();
    [Inject]
    private HttpClient? Http { get; set; }
    protected override async Task OnInitializedAsync(){
        string data = Http is not null ? await Http.GetStringAsync("data/degrees.json") : "";
        Degrees = JsonSerializer.Deserialize<SortedSet<Degree>>(data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new();
    }
}