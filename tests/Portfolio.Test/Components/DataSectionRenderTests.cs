using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Pages.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NUnit.Framework.Legacy;

namespace Portfolio.Test.Components;

public class DataSectionTests : BunitTestBase
{
    private class TestItem
    {
        public string Name { get; set; } = "";
    }

    private class ComparableItem : IComparable<ComparableItem>
    {
        public string Name { get; set; } = "";

        public int CompareTo(ComparableItem? other)
            => string.Compare(Name, other?.Name, StringComparison.Ordinal);
    }

    private class NonComparableItem
    {
        public string Name { get; set; } = "";

        public override bool Equals(object? obj)
            => obj is NonComparableItem other && Name == other.Name;

        public override int GetHashCode()
            => Name.GetHashCode();
    }
    private class FakeComparableItemComponent : ComponentBase
    {
        [Parameter] public ComparableItem? Item { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Item is null) return;

            builder.OpenElement(0, "span");
            builder.AddContent(1, Item.Name);
            builder.CloseElement();
        }
    }

    private class FakeNonComparableItemComponent : ComponentBase
    {
        [Parameter] public NonComparableItem? Item { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Item is null) return;

            builder.OpenElement(0, "span");
            builder.AddContent(1, Item.Name);
            builder.CloseElement();
        }
    }

    private class FakeItemComponent : ComponentBase
    {
        [Parameter] public TestItem? Item { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, Item?.Name);
        }
    }

    private class FakeHandler : HttpMessageHandler
    {
        private readonly string _response;

        public FakeHandler(string response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            });
        }
    }

    private static HttpClient CreateHttpClient(object data)
    {
        var json = JsonSerializer.Serialize(data);

        var handler = new FakeHandler(json);
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("http://test")
        };
    }

    [Test]
    public void DataSection_RendersItems_FromJson()
    {
        var items = new SortedSet<TestItem>(Comparer<TestItem>.Create((a, b) => a.Name.CompareTo(b.Name)))
        {
            new() { Name = "A" },
            new() { Name = "B" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
            .Add(x => x.Class, "test")
            .Add(x => x.Style, "color:red")
        );

        // Assert DOM structure exists
        var div = cut.Find("div");

        Assert.That(div.ClassName, Is.EqualTo("test"));
        Assert.That(div.GetAttribute("style"), Is.EqualTo("color:red"));

        // Items rendered (via text content)
        Assert.That(div.TextContent, Is.EqualTo("AB"));
    }

    [Test]
    public void DataSection_RemovesDuplicates_WhenTypeIsNotComparable()
    {
        // Arrange
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "A" }, // duplicate
            new NonComparableItem { Name = "B" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        // Act
        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeNonComparableItemComponent))
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("A"));
        Assert.That(cut.Markup, Does.Contain("B"));

        // Ensure only 2 unique renders
        Assert.That(cut.FindAll("span"), Has.Count.EqualTo(2));
    }

    [Test]
    public void DataSection_SortsItems_WhenTypeIsComparable()
    {
        var items = new[]
        {
            new ComparableItem { Name = "B" },
            new ComparableItem { Name = "A" },
            new ComparableItem { Name = "C" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<ComparableItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeComparableItemComponent))
        );

        var spans = cut.FindAll("span");

        var order = spans.Select(x => x.TextContent).ToArray();

        Assert.That(order, Is.EqualTo(new[] { "A", "B", "C" }));
    }

    [Test]
    public void DataSection_DoesNotGuaranteeOrder_WhenTypeIsNotComparable()
    {
        string[] expected = new[] { "A", "B", "C" };
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" },
            new NonComparableItem { Name = "C" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeNonComparableItemComponent))
        );

        var rendered = cut.FindAll("span").Select(x => x.TextContent).ToList();

        // Only invariant: same elements exist
        Assert.That(rendered, Is.EqualTo(expected));
    }

    [Test]
    public void DataSection_RemovesDuplicates_WhenTypeIsComparable()
    {
        var items = new[]
        {
            new ComparableItem { Name = "A" },
            new ComparableItem { Name = "A" },
            new ComparableItem { Name = "B" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<ComparableItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeComparableItemComponent))
        );

        var spans = cut.FindAll("span");

        Assert.That(spans, Has.Count.EqualTo(2));
    }

    [Test]
    public void DataSection_RendersEmpty_WhenUrlIsNullOrWhitespace()
    {
        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.JsonUrl, "")
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
        );

        Assert.That(cut.Markup, Does.Not.Contain("<div"));
    }

    [Test]
    public void DataSection_PassesItemsToDynamicComponent()
    {
        var items = new SortedSet<TestItem>(Comparer<TestItem>.Create((a, b) => a.Name.CompareTo(b.Name)))
        {
            new() { Name = "X" }
        };

        var http = CreateHttpClient(items);

        Context!.Services.AddSingleton(http);
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
            .Add(x => x.Class, "my-class")
            .Add(x => x.Style, "width:100%")
        );

        var div = cut.Find("div");
        Assert.That(cut.Markup, Does.Contain('X'));
        Assert.That(div.ClassName, Is.EqualTo("my-class"));
        Assert.That(div.GetAttribute("style"), Is.EqualTo("width:100%"));
    }

    [Test]
    public void DataSection_RendersNothing_WhenJsonIsEmptyArray()
    {
        Context!.Services.AddSingleton(CreateHttpClient(Array.Empty<TestItem>()));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
        );

        Assert.That(cut.Markup.Trim(), Is.EqualTo(string.Empty));
    }

    [Test]
    public void DataSection_UsesHashSet_WhenNoComparer()
    {
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" }
        };

        Context!.Services.AddSingleton(CreateHttpClient(items));
        Context!.Services.AddSingleton(new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.JsonUrl, "/data.json")
            .Add(x => x.ItemComponentType, typeof(FakeNonComparableItemComponent))
        );

        Assert.That(cut.FindAll("span").Count, Is.EqualTo(2));
    }

    [Test]
    public void DataSection_DoesNotFetch_WhenUrlIsWhitespace()
    {
        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.JsonUrl, "   ")
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
        );

        Assert.That(cut.Markup.Trim(), Is.EqualTo(string.Empty));
    }
}