using System.Text.Json;
using Models.Career;


namespace Repositories;

public class CertificationRepository(HttpClient client, JsonSerializerOptions options) : IRepository<Certification>
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<Certification>> GetAllAsync()
    {
        var json = await _client.GetStringAsync("data/certifications.json");
        return JsonSerializer.Deserialize<List<Certification>>(json, _options) ?? [];
    }
}