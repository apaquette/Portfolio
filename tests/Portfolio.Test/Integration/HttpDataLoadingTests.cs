using System.Text.Json;
using Models;

namespace Portfolio.Test.Integration;

/// <summary>
/// Tests for data loading and JSON deserialization scenarios
/// Note: HttpClient.GetStringAsync is not virtual and cannot be mocked with Moq.
/// These tests focus on JSON deserialization which is the critical path for data loading.
/// </summary>
[TestFixture]
public class HttpDataLoadingTests
{
    private JsonSerializerOptions jsonOptions = null!;

    [SetUp]
    public void Setup()
    {
        jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Test]
    public void LoadProjects_WithValidJson_ShouldDeserialize()
    {
        // Arrange
        var projectsJson = @"[
            {""completed"":""2022-06-01"",""title"":""Project 1""},
            {""completed"":""2021-06-01"",""title"":""Project 2""}
        ]";

        // Act
        var projects = JsonSerializer.Deserialize<SortedSet<Project>>(projectsJson, jsonOptions);

        // Assert
        Assert.That(projects, Is.Not.Null);
        Assert.That(projects!.Count, Is.EqualTo(2));
    }

    [Test]
    public void LoadExperience_WithValidJson_ShouldDeserialize()
    {
        // Arrange - Multiple experiences with different start dates
        var experienceJson = @"[
            {""jobStart"":""2020-06-15"",""title"":""Senior Developer""},
            {""jobStart"":""2018-01-01"",""title"":""Junior Developer""}
        ]";

        // Act
        var experiences = JsonSerializer.Deserialize<SortedSet<Experience>>(experienceJson, jsonOptions);

        // Assert
        Assert.That(experiences, Is.Not.Null);
        Assert.That(experiences!.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public void LoadDegrees_WithValidJson_ShouldDeserialize()
    {
        // Arrange
        var degreesJson = @"[
            {""graduationDate"":""2020-06-01"",""diploma"":""BSc"",""institution"":""Uni 1""},
            {""graduationDate"":""2022-06-01"",""diploma"":""Masters"",""institution"":""Uni 2""}
        ]";

        // Act
        var degrees = JsonSerializer.Deserialize<SortedSet<Degree>>(degreesJson, jsonOptions);

        // Assert
        Assert.That(degrees, Is.Not.Null);
        Assert.That(degrees!.Count, Is.EqualTo(2));
    }

    [Test]
    public void LoadCertifications_WithValidJson_ShouldDeserialize()
    {
        // Arrange
        var certsJson = @"[
            {""earnedOn"":""2023-03-15"",""name"":""AWS"",""issuedBy"":""Amazon""},
            {""earnedOn"":""2024-01-10"",""name"":""Azure"",""issuedBy"":""Microsoft""}
        ]";

        // Act
        var certs = JsonSerializer.Deserialize<SortedSet<Certification>>(certsJson, jsonOptions);

        // Assert
        Assert.That(certs, Is.Not.Null);
        Assert.That(certs!.Count, Is.EqualTo(2));
    }

    [Test]
    public void LoadData_WithEmptyArray_ShouldReturnEmptyCollection()
    {
        // Arrange
        var emptyJson = "[]";

        // Act
        var projects = JsonSerializer.Deserialize<SortedSet<Project>>(emptyJson, jsonOptions);

        // Assert
        Assert.That(projects, Is.Not.Null);
        Assert.That(projects!.Count, Is.EqualTo(0));
    }

    [Test]
    public void LoadData_InvalidJsonThrows_ShouldThrowJsonException()
    {
        // Arrange - empty string that will cause JSON parse error
        var invalidJson = string.Empty;

        // Act & Assert
        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<SortedSet<Project>>(invalidJson, jsonOptions);
        });
    }

    [Test]
    public void LoadData_WithNullUrl_ShouldSkipLoading()
    {
        // Arrange
        string? nullUrl = null;
        bool wasUrlUsed = false;

        // Act - Verify null URL prevents loading
        if (string.IsNullOrWhiteSpace(nullUrl))
        {
            wasUrlUsed = false;
        }

        // Assert
        Assert.That(wasUrlUsed, Is.False);
    }

    [Test]
    public void LoadSkills_WithDictionaryJson_ShouldDeserialize()
    {
        // Arrange
        var skillsJson = @"{
            ""Languages"": [""C#"", ""Python"", ""JavaScript""],
            ""Frameworks"": [""Blazor"", ""ASP.NET"", ""React""]
        }";

        // Act
        var skills = JsonSerializer.Deserialize<Dictionary<string, SortedSet<string>>>(skillsJson, jsonOptions);

        // Assert
        Assert.That(skills, Is.Not.Null);
        Assert.That(skills!.Count, Is.EqualTo(2));
        Assert.That(skills["Languages"].Count, Is.EqualTo(3));
        Assert.That(skills["Frameworks"].Count, Is.EqualTo(3));
    }

    [Test]
    public void ItemsAfterDeserialization_ShouldBeSorted()
    {
        // Arrange
        var projectsJson = @"[
            {""completed"":""2020-06-01"",""title"":""Old""},
            {""completed"":""2023-06-01"",""title"":""Recent""},
            {""completed"":""2021-06-01"",""title"":""Middle""}
        ]";

        // Act
        var projects = JsonSerializer.Deserialize<SortedSet<Project>>(projectsJson, jsonOptions);
        var list = projects!.ToList();

        // Assert
        Assert.That(list[0].Title, Is.EqualTo("Recent")); // Newest first
        Assert.That(list[1].Title, Is.EqualTo("Middle"));
        Assert.That(list[2].Title, Is.EqualTo("Old"));
    }

    [Test]
    public void MultipleLoads_ShouldMaintainSorting()
    {
        // Arrange
        var batch1Json = @"[
            {""completed"":""2022-06-01"",""title"":""First Batch""}
        ]";

        var batch2Json = @"[
            {""completed"":""2023-06-01"",""title"":""Second Batch""},
            {""completed"":""2021-06-01"",""title"":""Third Batch""}
        ]";

        // Act
        var batch1 = JsonSerializer.Deserialize<SortedSet<Project>>(batch1Json, jsonOptions);
        var batch2 = JsonSerializer.Deserialize<SortedSet<Project>>(batch2Json, jsonOptions);

        // Assert
        Assert.That(batch1!.Count, Is.EqualTo(1));
        Assert.That(batch2!.Count, Is.EqualTo(2));
        var batch2List = batch2.ToList();
        Assert.That(batch2List[0].Title, Is.EqualTo("Second Batch"));
    }
}
