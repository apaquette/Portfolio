using System.Text.Json;
using Models.Career;


namespace Repositories;

public class DegreeRepository(HttpClient client, JsonSerializerOptions options) : IRepository<Degree>
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<Degree>> GetAllAsync()
    {
        var json = await _client.GetStringAsync("data/degrees.json");
        return JsonSerializer.Deserialize<List<Degree>>(json, _options) ?? [];
    }
}