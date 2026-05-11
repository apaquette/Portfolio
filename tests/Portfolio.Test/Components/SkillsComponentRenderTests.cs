using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Pages.Home.Sections;

namespace Portfolio.Test.Components;

[TestFixture]
public class SkillsTests : BunitTestBase
{
    private static readonly JsonSerializerOptions JsonOptions = new();
    

    [SetUp]
    public void TestSetup()
    {
        Context!.Services.AddSingleton(JsonOptions);
    }

    [Test]
    public void Skills_RendersNothing_WhenJsonIsEmpty()
    {
        ConfigureHttp("{}");

        var component = RenderComponent<Skills>();

        var badges = component.FindAll(".skill-badge");
        var headers = component.FindAll("h5");

        Assert.Multiple(() =>
        {
            Assert.That(headers, Is.Empty);
            Assert.That(badges, Is.Empty);
        });
    }

    [Test]
    public void Skills_RendersAllCategories()
    {
        ConfigureHttp("""
        {
            "Languages": ["C#", "Python"],
            "Frameworks": ["Blazor"]
        }
        """);

        var component = RenderComponent<Skills>();

        var headers = component.FindAll("h5");

        Assert.Multiple(() =>
        {
            Assert.That(headers, Has.Count.EqualTo(2));
            Assert.That(headers[0].TextContent, Is.EqualTo("Languages"));
            Assert.That(headers[1].TextContent, Is.EqualTo("Frameworks"));
        });
    }

    [Test]
    public void Skills_RendersAllSkills()
    {
        ConfigureHttp("""
        {
            "Languages": ["C#", "Python", "Go"]
        }
        """);

        var component = RenderComponent<Skills>();

        var badges = component.FindAll(".skill-badge");

        string[] expected = ["C#", "Python", "Go"];

        Assert.That(
            badges.Select(x => x.TextContent),
            Is.EquivalentTo(expected));
    }

    [Test]
    public void Skills_RendersSkillsInSortedOrder()
    {
        ConfigureHttp("""
        {
            "Languages": ["Python", "C#", "Go"]
        }
        """);

        var component = RenderComponent<Skills>();

        var badges = component.FindAll(".skill-badge");

        string[] expected = ["C#", "Go", "Python"];

        Assert.That(
            badges.Select(x => x.TextContent),
            Is.EqualTo(expected)); // SortedSet
    }

    [Test]
    public void Skills_DoesNotThrow_WhenJsonIsInvalid()
    {
        ConfigureHttp("invalid json");

        Assert.That(
            () => RenderComponent<Skills>(),
            Throws.Nothing);
    }

    private void ConfigureHttp(string content)
    {
        var handler = new StubHttpMessageHandler(content);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        Context!.Services.AddSingleton(httpClient);
    }


    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _content;

        public StubHttpMessageHandler(string content)
        {
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        _content,
                        Encoding.UTF8,
                        "application/json")
                });
        }
    }
}