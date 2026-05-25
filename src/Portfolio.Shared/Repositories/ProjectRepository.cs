using System.Text.Json;
using Models.Portfolio;

namespace Repositories;
public class ProjectRepository(HttpClient client, JsonSerializerOptions options) : IRepository<Project>
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        string json = await _client.GetStringAsync("data/projects.json");
        return JsonSerializer.Deserialize<IEnumerable<Project>>(json, _options) ?? [];
    }

    public async Task<IEnumerable<Project>> GetFeaturedAsync()
    {
        string json = await _client.GetStringAsync("data/featured-projects.json");
        return JsonSerializer.Deserialize<IEnumerable<Project>>(json, _options) ?? [];
    }

    public async Task<Project?> GetByNameAsync(string name)
    {
        var projects = await GetAllAsync();
        return projects.FirstOrDefault(p => p.Title.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}