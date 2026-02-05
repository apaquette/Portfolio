using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace Portfolio.Pages.Home.Sections;

public partial class Skills : ComponentBase
{
    protected Dictionary<string, SortedSet<string>> SkillSet = [];
    [Inject]
    private HttpClient? Http { get; set; }
    protected override async Task OnInitializedAsync(){
        string data = Http is not null ? await Http.GetStringAsync("data/skillSet.json") : "";
        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
        SkillSet = JsonSerializer.Deserialize<Dictionary<string, SortedSet<string>>>(data, options) ?? [];
    }
}