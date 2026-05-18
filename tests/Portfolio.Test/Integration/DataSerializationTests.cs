using System.Text.Json;
using Models;

namespace Portfolio.Test.Integration;

/// <summary>
/// Tests for data serialization/deserialization and collection behavior
/// </summary>
[TestFixture]
public class DataSerializationTests
{
    private JsonSerializerOptions jsonOptions = null!;

    [SetUp]
    public void Setup()
    {
        jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Test]
    public void Project_JsonDeserialization_ShouldWork()
    {
        // Arrange
        var projectJson = @"{
            ""completed"":""2022-06-01"",
            ""title"":""Test Project"",
            ""description"":""A test""
        }";

        // Act
        var project = JsonSerializer.Deserialize<Project>(projectJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(project, Is.Not.Null);
            Assert.That(project!.Completed, Is.EqualTo(new DateOnly(2022, 6, 1)));
            Assert.That(project.Title, Is.EqualTo("Test Project"));
        });

    }

    [Test]
    public void Projects_CollectionSerialization_ShouldMaintainSort()
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

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(list[0].Title, Is.EqualTo("Recent")); // Newest first due to CompareTo
            Assert.That(list[1].Title, Is.EqualTo("Middle"));
            Assert.That(list[2].Title, Is.EqualTo("Old"));
        });

    }

    [Test]
    public void Experience_JsonDeserialization_ShouldWork()
    {
        // Arrange
        var experienceJson = @"{
            ""jobStart"":""2020-01-01T00:00:00"",
            ""title"":""Senior Dev"",
            ""company"":""Tech Corp""
        }";

        // Act
        var experience = JsonSerializer.Deserialize<Experience>(experienceJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(experience, Is.Not.Null);
            Assert.That(experience!.JobStart, Is.EqualTo(new DateTime(2020, 1, 1)));
            Assert.That(experience.Title, Is.EqualTo("Senior Dev"));
        });
    }

    [Test]
    public void Degree_JsonDeserialization_ShouldWork()
    {
        // Arrange
        var degreeJson = @"{
            ""graduationDate"":""2020-06-01"",
            ""diploma"":""BSc (Hons)"",
            ""institution"":""Test Uni""
        }";

        // Act
        var degree = JsonSerializer.Deserialize<Degree>(degreeJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(degree, Is.Not.Null);
            Assert.That(degree!.GraduationDate, Is.EqualTo(new DateOnly(2020, 6, 1)));
            Assert.That(degree.Diploma, Is.EqualTo("BSc (Hons)"));
        });
    }

    [Test]
    public void Certification_JsonDeserialization_ShouldWork()
    {
        // Arrange
        var certJson = @"{
            ""earnedOn"":""2023-03-15"",
            ""name"":""AWS Architect"",
            ""issuedBy"":""Amazon""
        }";

        // Act
        var cert = JsonSerializer.Deserialize<Certification>(certJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(cert, Is.Not.Null);
            Assert.That(cert!.EarnedOn, Is.EqualTo(new DateOnly(2023, 3, 15)));
            Assert.That(cert.Name, Is.EqualTo("AWS Architect"));
        });
    }

    [Test]
    public void Projects_InvalidJson_ShouldThrowException()
    {
        // Arrange
        var invalidJson = "{invalid}";

        // Act & Assert
        Assert.Throws<JsonException>(() =>
        {
            JsonSerializer.Deserialize<SortedSet<Project>>(invalidJson, jsonOptions);
        });
    }

    [Test]
    public void EmptyArray_ShouldDeserializeToEmptyCollection()
    {
        // Arrange
        var emptyJson = "[]";

        // Act
        var projects = JsonSerializer.Deserialize<SortedSet<Project>>(emptyJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(projects, Is.Not.Null);
            Assert.That(projects!.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public void SkillDictionary_JsonDeserialization_ShouldWork()
    {
        // Arrange
        var skillJson = @"{
            ""Languages"": [""C#"", ""Python""],
            ""Frameworks"": [""Blazor"", ""ASP.NET""]
        }";

        // Act
        var skills = JsonSerializer.Deserialize<Dictionary<string, SortedSet<string>>>(skillJson, jsonOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(skills, Is.Not.Null);
            Assert.That(skills!["Languages"].Count, Is.EqualTo(2));
            Assert.That(skills["Languages"], Contains.Item("C#"));
        });
    }

    [Test]
    public void SkillsWithinCategory_ShouldBeSortedAlphabetically()
    {
        // Arrange
        var skillJson = @"{
            ""Languages"": [""Python"", ""C#"", ""JavaScript""]
        }";

        // Act
        var skills = JsonSerializer.Deserialize<Dictionary<string, SortedSet<string>>>(skillJson, jsonOptions);
        var languages = skills!["Languages"].ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(languages[0], Is.EqualTo("C#"));
            Assert.That(languages[1], Is.EqualTo("JavaScript"));
            Assert.That(languages[2], Is.EqualTo("Python"));
        });
    }
}
