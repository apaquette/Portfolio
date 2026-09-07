using System.Text.Json;
using Models.Career;
using Models.Portfolio;

namespace Repositories;

public class JsonResourceFetcher(HttpClient client, JsonSerializerOptions options)
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;
    private readonly Dictionary<string, string> _resources = new()
    {
        { nameof(Award), "data/awards.json" },
        { nameof(Certification), "data/certifications.json" },
        { nameof(Degree), "data/degrees.json" },
        { nameof(Experience), "data/workExperience.json" },
        { nameof(SkillCategory), "data/skillSet.json" },
        { nameof(Project), "data/projects.json" },
    };
    public async Task<IEnumerable<T>> GetAllAsync<T>()
    {
        if (!_resources.TryGetValue(typeof(T).Name, out var filePath))
        {
            throw new ArgumentException("Resource not found", nameof(T));
        }

        var json = await _client.GetStringAsync(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, _options) ?? [];
    }
}