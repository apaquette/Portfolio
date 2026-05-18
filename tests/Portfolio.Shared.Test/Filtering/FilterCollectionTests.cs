using Filtering.Interfaces;
using Filtering.Core;

namespace Portfolio.Shared.Filter.Test;

public class FilterCollectionTests
{
    private class TestItem
    {
        public string Category { get; set; } = "";
        public string[] Tags { get; set; } = [];
    }

    private class CategoryFilter(string category) : IFilter<TestItem>
    {
        public string Name => category;

        public bool Matches(TestItem item)
        {
            return item.Category == category;
        }
    }

    private class TagFilter(string tag) : IFilter<TestItem>
    {
        public string Name => tag;

        public bool Matches(TestItem item)
        {
            return item.Tags.Contains(tag);
        }
    }

    [Test]
    public void Constructor_WithFilters_ShouldInitializeAvailableFilters()
    {
        var filter1 = new CategoryFilter("A");
        var filter2 = new CategoryFilter("B");
        var collection = new FilterCollection<TestItem>(filter1, filter2);

        Assert.That(collection.AvailableFilterNames, Has.Count.EqualTo(2));
        Assert.That(collection.AvailableFilterNames, Contains.Item("A"));
        Assert.That(collection.AvailableFilterNames, Contains.Item("B"));
    }

    [Test]
    public void Constructor_EmptyFilters_ShouldCreateEmptyCollection()
    {
        var collection = new FilterCollection<TestItem>();

        Assert.That(collection.AvailableFilterNames, Is.Empty);
        Assert.That(collection.ActiveFilters, Is.Empty);
    }

    [Test]
    public void ToggleFilter_ActivatesFilter_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);

        var result = collection.ToggleFilter("A");

        Assert.That(result, Is.True);
        Assert.That(collection.ActiveFilters, Has.Count.EqualTo(1));
    }

    [Test]
    public void ToggleFilter_DeactivatesFilter_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        collection.ToggleFilter("A");

        var result = collection.ToggleFilter("A");

        Assert.That(result, Is.False);
        Assert.That(collection.ActiveFilters, Is.Empty);
    }

    [Test]
    public void ToggleFilter_MultipleToggles_ShouldMaintainCorrectState()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var collection = new FilterCollection<TestItem>(f1, f2);

        collection.ToggleFilter("A");
        collection.ToggleFilter("B");

        Assert.That(collection.ActiveFilters, Has.Count.EqualTo(2));
        Assert.That(collection.IsFilterActive("A"), Is.True);
        Assert.That(collection.IsFilterActive("B"), Is.True);
    }

    [Test]
    public void ToggleFilter_InvalidFilterName_ShouldThrowArgumentException()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);

        var ex = Assert.Throws<ArgumentException>(() => collection.ToggleFilter("NonExistent"));

        Assert.That(ex?.Message, Contains.Substring("NonExistent"));
    }

    [Test]
    public void SetActiveFilters_WithValidNames_ShouldActivateOnlyThoseFilters()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var f3 = new CategoryFilter("C");
        var collection = new FilterCollection<TestItem>(f1, f2, f3);

        collection.SetActiveFilters("A", "C");

        Assert.That(collection.ActiveFilters, Has.Count.EqualTo(2));
        Assert.That(collection.IsFilterActive("A"), Is.True);
        Assert.That(collection.IsFilterActive("B"), Is.False);
        Assert.That(collection.IsFilterActive("C"), Is.True);
    }

    [Test]
    public void SetActiveFilters_EmptyArray_ShouldClearAllFilters()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var collection = new FilterCollection<TestItem>(f1, f2);
        collection.ToggleFilter("A");
        collection.ToggleFilter("B");

        collection.SetActiveFilters();

        Assert.That(collection.ActiveFilters, Is.Empty);
    }

    [Test]
    public void SetActiveFilters_InvalidFilterName_ShouldThrowArgumentException()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);

        var ex = Assert.Throws<ArgumentException>(() => collection.SetActiveFilters("A", "Invalid"));

        Assert.That(ex?.Message, Contains.Substring("Invalid"));
    }

    [Test]
    public void SetActiveFilters_ReplacesPreviousFilters()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var f3 = new CategoryFilter("C");
        var collection = new FilterCollection<TestItem>(f1, f2, f3);

        collection.SetActiveFilters("A", "B");
        collection.SetActiveFilters("C");

        Assert.That(collection.ActiveFilters, Has.Count.EqualTo(1));
        Assert.That(collection.IsFilterActive("C"), Is.True);
        Assert.That(collection.IsFilterActive("A"), Is.False);
    }

    [Test]
    public void ClearFilters_WithActiveFilters_ShouldRemoveAll()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var collection = new FilterCollection<TestItem>(f1, f2);
        collection.ToggleFilter("A");
        collection.ToggleFilter("B");

        collection.ClearFilters();

        Assert.That(collection.ActiveFilters, Is.Empty);
    }

    [Test]
    public void Matches_NoActiveFilters_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        var item = new TestItem { Category = "B" };

        var result = collection.Matches(item);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Matches_ItemMatchesAllActiveFilters_ShouldReturnTrue()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new TagFilter("Important");
        var collection = new FilterCollection<TestItem>(f1, f2);
        collection.ToggleFilter("A");
        collection.ToggleFilter("Important");

        var item = new TestItem { Category = "A", Tags = ["Important", "Other"] };

        var result = collection.Matches(item);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Matches_ItemFailsOneFilter_ShouldReturnFalse()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new TagFilter("Important");
        var collection = new FilterCollection<TestItem>(f1, f2);
        collection.ToggleFilter("A");
        collection.ToggleFilter("Important");

        var item = new TestItem { Category = "A", Tags = ["Other"] };

        var result = collection.Matches(item);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Matches_ItemFailsAllFilters_ShouldReturnFalse()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new TagFilter("Important");
        var collection = new FilterCollection<TestItem>(f1, f2);
        collection.ToggleFilter("A");
        collection.ToggleFilter("Important");

        var item = new TestItem { Category = "B", Tags = ["Other"] };

        var result = collection.Matches(item);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Apply_NoFiltersActive_ShouldReturnAllItems()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        var items = new[]
        {
            new TestItem { Category = "A" },
            new TestItem { Category = "B" },
            new TestItem { Category = "C" }
        };

        var result = collection.Apply(items).ToList();

        Assert.That(result, Has.Count.EqualTo(3));
    }

    [Test]
    public void Apply_WithActiveFilters_ShouldReturnOnlyMatchingItems()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        collection.ToggleFilter("A");

        var items = new[]
        {
            new TestItem { Category = "A" },
            new TestItem { Category = "B" },
            new TestItem { Category = "A" }
        };

        var result = collection.Apply(items).ToList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result, Has.All.Matches<TestItem>(t => t.Category == "A"));
    }

    [Test]
    public void Apply_EmptyCollection_ShouldReturnEmpty()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        collection.ToggleFilter("A");

        var result = collection.Apply(Array.Empty<TestItem>()).ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void IsFilterActive_ActiveFilter_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        collection.ToggleFilter("A");

        Assert.That(collection.IsFilterActive("A"), Is.True);
    }

    [Test]
    public void IsFilterActive_InactiveFilter_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);

        Assert.That(collection.IsFilterActive("A"), Is.False);
    }

    [Test]
    public void AnyFiltersActive_NoActiveFilters_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);

        Assert.That(collection.AnyFiltersActive(), Is.False);
    }

    [Test]
    public void AnyFiltersActive_WithActiveFilters_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var collection = new FilterCollection<TestItem>(filter);
        collection.ToggleFilter("A");

        Assert.That(collection.AnyFiltersActive(), Is.True);
    }
}
