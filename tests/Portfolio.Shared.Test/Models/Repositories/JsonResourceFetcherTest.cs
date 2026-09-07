using System.Net;
using System.Text;
using System.Text.Json;

// Replace this with the namespace containing JsonResourceFetcher and your models.
using Repositories;
using Models.Career;
using Models.Portfolio;
using Validation.Dates;

namespace Portfolio.Shared.Test;

[TestFixture]
public class JsonResourceFetcherTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Test]
    public async Task GetAllAsync_WhenResourceExists_ReturnsDeserializedItems()
    {
        const string json = "[]";

        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        var result = await fetcher.GetAllAsync<Award>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
        Assert.That(handler.RequestedUris, Has.Count.EqualTo(1));
        Assert.That(handler.RequestedUris[0].ToString(), Is.EqualTo("https://example.test/data/awards.json"));
    }

    [Test]
    public async Task GetAllAsync_WhenJsonContainsAwards_ReturnsDeserializedAwards()
    {
        const string json = """
            [
                {
                    "Title": "Best Project",
                    "Issuer": "Example Organization",
                    "Date": "2024-05-15",
                    "Description": "Award description"
                }
            ]
            """;

        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        var result = (await fetcher.GetAllAsync<Award>()).ToList();

        Assert.That(result, Has.Count.EqualTo(1));

        var award = result[0];

        Assert.That(award.Title, Is.EqualTo("Best Project"));
        Assert.That(award.Issuer, Is.EqualTo("Example Organization"));
        Assert.That(award.Date, Is.EqualTo(new DateOnly(2024, 5, 15)));
        Assert.That(award.Description, Is.EqualTo("Award description"));
    }

    [Test]
    public async Task GetAllAsync_WhenAwardDateIsMissing_ThrowsMissingDateException()
    {
        const string json = """
            [
                {
                    "Title": "Best Project",
                    "Issuer": "Example Organization",
                    "Date": "0001-01-01",
                    "Description": "Award description"
                }
            ]
            """;

        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        Assert.ThrowsAsync<MissingDateException>(
            async () => await fetcher.GetAllAsync<Award>());
    }



    [Test]
    public async Task GetAllAsync_WhenJsonIsNull_ReturnsEmptyCollection()
    {
        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        var result = await fetcher.GetAllAsync<Award>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GetAllAsync_WhenResourceDoesNotExist_ThrowsArgumentException()
    {
        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        var exception = Assert.ThrowsAsync<ArgumentException>(
            async () => await fetcher.GetAllAsync<UnmappedResource>());

        Assert.That(exception!.ParamName, Is.EqualTo("T"));
        Assert.That(exception.Message, Does.Contain("Resource not found"));
        Assert.That(handler.RequestedUris, Is.Empty);
    }

    [Test]
    public async Task GetAllAsync_UsesExpectedResourcePathForEachSupportedType()
    {
        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[]", Encoding.UTF8, "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        await fetcher.GetAllAsync<Award>();
        await fetcher.GetAllAsync<Certification>();
        await fetcher.GetAllAsync<Degree>();
        await fetcher.GetAllAsync<Experience>();
        await fetcher.GetAllAsync<SkillCategory>();
        await fetcher.GetAllAsync<Project>();

        var requestedPaths = handler.RequestedUris
            .Select(uri => uri.AbsolutePath.TrimStart('/'))
            .ToList();

        Assert.That(
            requestedPaths,
            Is.EqualTo(new[]
            {
                "data/awards.json",
                "data/certifications.json",
                "data/degrees.json",
                "data/workExperience.json",
                "data/skillSet.json",
                "data/projects.json"
            }));
    }

    [Test]
    public void GetAllAsync_WhenHttpRequestFails_PropagatesHttpRequestException()
    {
        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("Not found")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        Assert.ThrowsAsync<HttpRequestException>(
            async () => await fetcher.GetAllAsync<Award>());
    }

    [Test]
    public void GetAllAsync_WhenJsonIsInvalid_PropagatesJsonException()
    {
        var handler = new TestHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{ invalid json }", Encoding.UTF8, "application/json")
            });

        using var client = CreateHttpClient(handler);
        var fetcher = new JsonResourceFetcher(client, JsonOptions);

        Assert.ThrowsAsync<JsonException>(
            async () => await fetcher.GetAllAsync<Award>());
    }

    private static HttpClient CreateHttpClient(HttpMessageHandler handler)
    {
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test/")
        };
    }

    private sealed class UnmappedResource
    {
    }

    private sealed class TestHttpMessageHandler
        : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;

        public List<Uri> RequestedUris { get; } = [];

        public TestHttpMessageHandler(
            Func<HttpRequestMessage, HttpResponseMessage> responseFactory)
        {
            _responseFactory = responseFactory;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestedUris.Add(request.RequestUri!);

            var response = _responseFactory(request);
            return Task.FromResult(response);
        }
    }
}
