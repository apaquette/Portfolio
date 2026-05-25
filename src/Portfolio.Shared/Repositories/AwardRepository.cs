using System.Text.Json;
using Models.Career;


namespace Repositories;

public class AwardRepository(HttpClient client, JsonSerializerOptions options) : IRepository<Award>
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<Award>> GetAllAsync()
    {
        var json = await _client.GetStringAsync("data/awards.json");
        return JsonSerializer.Deserialize<List<Award>>(json, _options) ?? [];
    }
}