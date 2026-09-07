using System.Net;
using System.Text;
using System.Text.Json;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.UI.Composition;
using Filtering.Interfaces;
using Filtering.Core;

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
            => Name.GetHashCode(StringComparison.Ordinal);
    }

    private class FakeComparableItemComponent : ComponentBase
    {
        [Parameter]
        public ComparableItem? Item { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Item is null)
                return;

            builder.OpenElement(0, "span");
            builder.AddContent(1, Item.Name);
            builder.CloseElement();
        }
    }

    private class FakeNonComparableItemComponent : ComponentBase
    {
        [Parameter]
        public NonComparableItem? Item { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Item is null)
                return;

            builder.OpenElement(0, "span");
            builder.AddContent(1, Item.Name);
            builder.CloseElement();
        }
    }

    private class FakeItemComponent : ComponentBase
    {
        [Parameter]
        public TestItem? Item { get; set; }

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

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    _response,
                    Encoding.UTF8,
                    "application/json")
            });
        }
    }

    private static HttpClient CreateHttpClient(object? data)
    {
        var json = JsonSerializer.Serialize(data);

        return new HttpClient(new FakeHandler(json))
        {
            BaseAddress = new Uri("http://test")
        };
    }

    [Test]
    public void DataSection_RendersItems_FromJson()
    {
        var items = new SortedSet<TestItem>(
            Comparer<TestItem>.Create((a, b) =>
                string.Compare(a.Name, b.Name, StringComparison.Ordinal)))
        {
            new() { Name = "A" },
            new() { Name = "B" }
        };

        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
            .Add(x => x.Class, "test")
            .Add(x => x.Style, "color:red"));

        cut.WaitForAssertion(() =>
        {
            var div = cut.Find("div");

            Assert.Multiple(() =>
            {
                Assert.That(div.ClassName, Is.EqualTo("test"));
                Assert.That(
                    div.GetAttribute("style"),
                    Is.EqualTo("color:red"));
                Assert.That(div.TextContent, Is.EqualTo("AB"));
            });
        });
    }

    [Test]
    public void DataSection_RemovesDuplicates_WhenTypeIsNotComparable()
    {
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" }
        };

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeNonComparableItemComponent)));

        cut.WaitForAssertion(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(cut.Markup, Does.Contain("A"));
                Assert.That(cut.Markup, Does.Contain("B"));
                Assert.That(cut.FindAll("span"), Has.Count.EqualTo(2));
            });
        });
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

        var cut = RenderComponent<DataSection<ComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeComparableItemComponent)));

        cut.WaitForAssertion(() =>
        {
            var order = cut.FindAll("span")
                .Select(x => x.TextContent)
                .ToArray();

            Assert.That(order, Is.EqualTo(new[] { "A", "B", "C" }));
        });
    }

    [Test]
    public void DataSection_DoesNotGuaranteeOrder_WhenTypeIsNotComparable()
    {
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" },
            new NonComparableItem { Name = "C" }
        };

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeNonComparableItemComponent)));

        cut.WaitForAssertion(() =>
        {
            var rendered = cut.FindAll("span")
                .Select(x => x.TextContent)
                .ToArray();

            // HashSet enumeration order is not part of the contract.
            Assert.That(
                rendered,
                Is.EquivalentTo(new[] { "A", "B", "C" }));
        });
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

        var cut = RenderComponent<DataSection<ComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeComparableItemComponent)));

        cut.WaitForAssertion(() =>
        {
            Assert.That(cut.FindAll("span"), Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void DataSection_PassesItemsToDynamicComponent()
    {
        var items = new SortedSet<TestItem>(
            Comparer<TestItem>.Create((a, b) =>
                string.Compare(a.Name, b.Name, StringComparison.Ordinal)))
        {
            new() { Name = "X" }
        };

        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.Items, items)
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent))
            .Add(x => x.Class, "my-class")
            .Add(x => x.Style, "width:100%"));

        cut.WaitForAssertion(() =>
        {
            var div = cut.Find("div");

            Assert.Multiple(() =>
            {
                Assert.That(cut.Markup, Does.Contain("X"));
                Assert.That(div.ClassName, Is.EqualTo("my-class"));
                Assert.That(
                    div.GetAttribute("style"),
                    Is.EqualTo("width:100%"));
            });
        });
    }

    [Test]
    public void DataSection_RendersNothing_WhenEmpty()
    {
        var cut = RenderComponent<DataSection<TestItem>>(p => p
            .Add(x => x.ItemComponentType, typeof(FakeItemComponent)));

        // The initial and final render are both empty for an empty array.
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

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeNonComparableItemComponent)));

        cut.WaitForAssertion(() =>
        {
            Assert.That(cut.FindAll("span"), Has.Count.EqualTo(2));
        });
    }

    private class FakeFilter(string name) : IFilter<NonComparableItem>
    {
        public string Name { get; set; } = name;

        public bool Matches(NonComparableItem item)
        {
            return item.Name == Name;
        }
    }

    [Test]
    public void DataSection_ApplyFilters_WithFilter()
    {
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" }
        };
            

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeNonComparableItemComponent))
            .Add(
                x => x.Filters,
                new FilterCollection<NonComparableItem>(
                    [new FakeFilter("A")] )));

        cut.WaitForAssertion(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(cut.Markup, Does.Contain("<div"));
                Assert.That(cut.Markup, Does.Contain("A"));
                Assert.That(cut.Markup, Does.Contain("B"));
                Assert.That(cut.Markup, Does.Contain("</div>"));
            });
        });
    }

    [Test]
    public void DataSection_ApplyFilters_WithDimensionalFilter()
    {
        var items = new[]
        {
            new NonComparableItem { Name = "A" },
            new NonComparableItem { Name = "B" }
        };

        var dimension = new FilterDimension<NonComparableItem>(
            "Name",
            new FakeFilter("A"));

        var collection = new DimensionalFilterCollection<NonComparableItem>();
        collection.AddDimension(dimension);

        var cut = RenderComponent<DataSection<NonComparableItem>>(p => p
            .Add(x => x.Items, items)
            .Add(
                x => x.ItemComponentType,
                typeof(FakeNonComparableItemComponent))
            .Add(x => x.DimensionalFilters, collection));

        cut.WaitForAssertion(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(cut.Markup, Does.Contain("<div"));
                Assert.That(cut.Markup, Does.Contain("A"));
                Assert.That(cut.Markup, Does.Contain("B"));
                Assert.That(cut.Markup, Does.Contain("</div>"));
            });
        });
    }
}
