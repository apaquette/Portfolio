using System.Text.Json;
using Models.Career;

namespace Repositories;

public class SkillCategoryRepository(HttpClient client, JsonSerializerOptions options) : IRepository<SkillCategory> 
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = options;

    public async Task<IEnumerable<SkillCategory>> GetAllAsync()
    {
        var json = await _client.GetStringAsync("data/skillSet.json");
        return JsonSerializer.Deserialize<List<SkillCategory>>(json, _options) ?? [];
    }

}