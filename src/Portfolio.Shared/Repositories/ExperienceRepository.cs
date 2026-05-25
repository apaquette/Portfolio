using System.Text.Json;
using Models.Career;

namespace Repositories;

public class ExperienceRepository(HttpClient client, JsonSerializerOptions options) : IRepository<Experience>
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<Experience>> GetAllAsync()
    {
        var json = await _client.GetStringAsync("data/workExperience.json");
        return JsonSerializer.Deserialize<List<Experience>>(json, _options) ?? [];
    }
}