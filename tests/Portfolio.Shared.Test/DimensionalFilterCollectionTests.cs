using Models;

namespace Portfolio.Shared.Test;

public class DimensionalFilterCollectionTests
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
    public void Constructor_ShouldCreateEmptyCollection()
    {
        var collection = new DimensionalFilterCollection<TestItem>();

        Assert.That(collection.DimensionNames, Is.Empty);
    }

    [Test]
    public void AddDimension_WithNewDimension_ShouldAddSuccessfully()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));

        collection.AddDimension(dimension);

        Assert.That(collection.DimensionNames, Has.Count.EqualTo(1));
        Assert.That(collection.DimensionNames, Contains.Item("Categories"));
    }

    [Test]
    public void AddDimension_WithDuplicateName_ShouldThrowArgumentException()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Categories", new CategoryFilter("B"));

        collection.AddDimension(d1);

        var ex = Assert.Throws<ArgumentException>(() => collection.AddDimension(d2));

        Assert.That(ex?.Message, Contains.Substring("Categories"));
    }

    [Test]
    public void AddDimension_MultipleDimensions_ShouldAllBeAdded()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));

        collection.AddDimension(d1);
        collection.AddDimension(d2);

        Assert.That(collection.DimensionNames, Has.Count.EqualTo(2));
        Assert.That(collection.DimensionNames, Contains.Item("Categories"));
        Assert.That(collection.DimensionNames, Contains.Item("Tags"));
    }

    [Test]
    public void GetDimension_WithExistingDimension_ShouldReturnDimension()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

        var result = collection.GetDimension("Categories");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Categories"));
    }

    [Test]
    public void GetDimension_WithNonExistentDimension_ShouldReturnNull()
    {
        var collection = new DimensionalFilterCollection<TestItem>();

        var result = collection.GetDimension("NonExistent");

        Assert.That(result, Is.Null);
    }

    [Test]
    public void DimensionNames_ShouldReturnReadOnlyList()
    {
        var collection = new DimensionalFilterCollection<TestItem>();

        var names = collection.DimensionNames;

        Assert.That(names, Is.AssignableTo<IReadOnlyList<string>>());
    }

    [Test]
    public void Matches_NoActivatedFilters_ShouldReturnTrue()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

        var item = new TestItem { Category = "B" };

        Assert.That(collection.Matches(item), Is.True);
    }

    [Test]
    public void Matches_SingleDimensionMatch_ShouldReturnTrue()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("A");

        var item = new TestItem { Category = "A" };

        Assert.That(collection.Matches(item), Is.True);
    }

    [Test]
    public void Matches_SingleDimensionNoMatch_ShouldReturnFalse()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("A");

        var item = new TestItem { Category = "B" };

        Assert.That(collection.Matches(item), Is.False);
    }

    [Test]
    public void Matches_MultipleDimensionsAllMatch_ShouldReturnTrue()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d2.ToggleFilter("Important");

        var item = new TestItem { Category = "A", Tags = ["Important"] };

        Assert.That(collection.Matches(item), Is.True);
    }

    [Test]
    public void Matches_MultipleDimensionsOneDoesNotMatch_ShouldReturnFalse()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d2.ToggleFilter("Important");

        var item = new TestItem { Category = "A", Tags = ["Other"] };

        Assert.That(collection.Matches(item), Is.False);
    }

    [Test]
    public void Matches_ImplementsAndLogicBetweenDimensions()
    {
        // This test verifies that ALL dimensions must match (AND logic)
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"), new CategoryFilter("B"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"), new TagFilter("Urgent"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d1.ToggleFilter("B"); // OR logic within dimension
        d2.ToggleFilter("Important");
        d2.ToggleFilter("Urgent"); // OR logic within dimension

        // Matches both filters in both dimensions
        var item1 = new TestItem { Category = "A", Tags = ["Important"] };
        Assert.That(collection.Matches(item1), Is.True);

        var item2 = new TestItem { Category = "B", Tags = ["Urgent"] };
        Assert.That(collection.Matches(item2), Is.True);

        // Matches one dimension but not the other
        var item3 = new TestItem { Category = "A", Tags = ["Random"] };
        Assert.That(collection.Matches(item3), Is.False);

        // Doesn't match any filters
        var item4 = new TestItem { Category = "C", Tags = ["Random"] };
        Assert.That(collection.Matches(item4), Is.False);
    }

    [Test]
    public void Apply_WithActiveFilters_ShouldReturnOnlyMatchingItems()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("A");

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
    public void Apply_NoActiveFilters_ShouldReturnAllItems()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

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
    public void Apply_EmptyCollection_ShouldReturnEmpty()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("A");

        var result = collection.Apply(Array.Empty<TestItem>()).ToList();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ClearAllFilters_ShouldClearAllDimensions()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d2.ToggleFilter("Important");

        collection.ClearAllFilters();

        Assert.That(d1.AnyFiltersActive(), Is.False);
        Assert.That(d2.AnyFiltersActive(), Is.False);
    }

    [Test]
    public void AnyFiltersActive_NoActiveFilters_ShouldReturnFalse()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

        Assert.That(collection.AnyFiltersActive(), Is.False);
    }

    [Test]
    public void AnyFiltersActive_WithActiveFiltersInOneDimension_ShouldReturnTrue()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");

        Assert.That(collection.AnyFiltersActive(), Is.True);
    }

    [Test]
    public void AnyFiltersActive_WithActiveFiltersInMultipleDimensions_ShouldReturnTrue()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d2.ToggleFilter("Important");

        Assert.That(collection.AnyFiltersActive(), Is.True);
    }

    [Test]
    public void GetActiveFilters_ShouldReturnAllActiveFiltersByDimension()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"), new CategoryFilter("B"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Important"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        d1.ToggleFilter("A");
        d1.ToggleFilter("B");
        d2.ToggleFilter("Important");

        var activeFilters = collection.GetActiveFilters();

        Assert.That(activeFilters, Has.Count.EqualTo(2));
        Assert.That(activeFilters["Categories"], Has.Count.EqualTo(2));
        Assert.That(activeFilters["Categories"], Contains.Item("A"));
        Assert.That(activeFilters["Categories"], Contains.Item("B"));
        Assert.That(activeFilters["Tags"], Has.Count.EqualTo(1));
        Assert.That(activeFilters["Tags"], Contains.Item("Important"));
    }

    [Test]
    public void GetActiveFilters_NoActiveFilters_ShouldReturnEmptyDictionary()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

        var activeFilters = collection.GetActiveFilters();

        Assert.That(activeFilters, Has.Count.EqualTo(1));
        Assert.That(activeFilters["Categories"], Is.Empty);
    }

    [Test]
    public void GetActiveFilters_AfterClearingFilters_ShouldReturnEmpty()
    {
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Categories", new CategoryFilter("A"));
        collection.AddDimension(dimension);

        dimension.ToggleFilter("A");
        collection.ClearAllFilters();

        var activeFilters = collection.GetActiveFilters();

        Assert.That(activeFilters["Categories"], Is.Empty);
    }

    [Test]
    public void ComplexScenario_MultiDimensionFilter()
    {
        // Complex real-world scenario
        var collection = new DimensionalFilterCollection<TestItem>();
        var categoryDim = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"), 
            new CategoryFilter("Personal"));
        var tagDim = new FilterDimension<TestItem>("Priority", 
            new TagFilter("High"), 
            new TagFilter("Medium"));

        collection.AddDimension(categoryDim);
        collection.AddDimension(tagDim);

        // Activate Work and High priority
        categoryDim.ToggleFilter("Work");
        tagDim.ToggleFilter("High");

        var items = new[]
        {
            new TestItem { Category = "Work", Tags = ["High", "Urgent"] },       // Match
            new TestItem { Category = "Work", Tags = ["Medium"] },               // No match (no High)
            new TestItem { Category = "Personal", Tags = ["High"] },             // No match (no Work)
            new TestItem { Category = "Personal", Tags = ["Low"] }               // No match
        };

        var result = collection.Apply(items).ToList();

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Category, Is.EqualTo("Work"));
        Assert.That(result[0].Tags, Contains.Item("High"));
    }
}
