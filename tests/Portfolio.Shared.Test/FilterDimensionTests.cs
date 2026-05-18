using Models;

namespace Portfolio.Shared.Test;

public class FilterDimensionTests
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
    public void Constructor_WithValidName_ShouldSetName()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        Assert.That(dimension.Name, Is.EqualTo("Categories"));
    }

    [Test]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        var filter = new CategoryFilter("A");

        Assert.Throws<ArgumentNullException>(() => new FilterDimension<TestItem>(null!, filter));
    }

    [Test]
    public void Constructor_WithMultipleFilters_ShouldInitializeAvailableFilters()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);

        Assert.That(dimension.AvailableFilterNames, Has.Count.EqualTo(2));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("A"));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("B"));
    }

    [Test]
    public void Constructor_NoFilters_ShouldCreateEmptyDimension()
    {
        var dimension = new FilterDimension<TestItem>("Categories");

        Assert.That(dimension.AvailableFilters, Is.Empty);
        Assert.That(dimension.ActiveFilters, Is.Empty);
    }

    [Test]
    public void AvailableFilters_ShouldReturnReadOnlyList()
    {
        var f1 = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", f1);

        var filters = dimension.AvailableFilters;

        Assert.That(filters, Is.AssignableTo<IReadOnlyList<IFilter<TestItem>>>());
    }

    [Test]
    public void ToggleFilter_ActivatesFilter_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        var result = dimension.ToggleFilter("A");

        Assert.That(result, Is.True);
        Assert.That(dimension.ActiveFilters, Has.Count.EqualTo(1));
    }

    [Test]
    public void ToggleFilter_DeactivatesFilter_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);
        dimension.ToggleFilter("A");

        var result = dimension.ToggleFilter("A");

        Assert.That(result, Is.False);
        Assert.That(dimension.ActiveFilters, Is.Empty);
    }

    [Test]
    public void ToggleFilter_InvalidFilterName_ShouldThrowArgumentException()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        var ex = Assert.Throws<ArgumentException>(() => dimension.ToggleFilter("Invalid"));

        Assert.That(ex?.Message, Contains.Substring("Invalid"));
        Assert.That(ex?.Message, Contains.Substring("Categories"));
    }

    [Test]
    public void ToggleFilter_MultipleToggles_ShouldMaintainCorrectState()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);

        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        Assert.That(dimension.ActiveFilters, Has.Count.EqualTo(2));
    }

    [Test]
    public void ClearFilters_WithActiveFilters_ShouldRemoveAll()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        dimension.ClearFilters();

        Assert.That(dimension.ActiveFilters, Is.Empty);
    }

    [Test]
    public void IsFilterActive_ActiveFilter_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);
        dimension.ToggleFilter("A");

        Assert.That(dimension.IsFilterActive("A"), Is.True);
    }

    [Test]
    public void IsFilterActive_InactiveFilter_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        Assert.That(dimension.IsFilterActive("A"), Is.False);
    }

    [Test]
    public void AnyFiltersActive_NoActiveFilters_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        Assert.That(dimension.AnyFiltersActive(), Is.False);
    }

    [Test]
    public void AnyFiltersActive_WithActiveFilters_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);
        dimension.ToggleFilter("A");

        Assert.That(dimension.AnyFiltersActive(), Is.True);
    }

    [Test]
    public void ActiveFilterNames_ShouldReturnNamesOfActiveFilters()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");

        var names = dimension.ActiveFilterNames;

        Assert.That(names, Has.Count.EqualTo(1));
        Assert.That(names, Contains.Item("A"));
    }

    [Test]
    public void ActiveFilterNames_NoActiveFilters_ShouldReturnEmpty()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        var names = dimension.ActiveFilterNames;

        Assert.That(names, Is.Empty);
    }

    [Test]
    public void Matches_NoActiveFilters_ShouldReturnTrueForAllItems()
    {
        var f1 = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", f1);
        var item1 = new TestItem { Category = "A" };
        var item2 = new TestItem { Category = "B" };

        Assert.That(dimension.Matches(item1), Is.True);
        Assert.That(dimension.Matches(item2), Is.True);
    }

    [Test]
    public void Matches_ItemMatchesActiveFilter_ShouldReturnTrue()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);
        dimension.ToggleFilter("A");

        var item = new TestItem { Category = "A" };

        Assert.That(dimension.Matches(item), Is.True);
    }

    [Test]
    public void Matches_ItemDoesNotMatchActiveFilter_ShouldReturnFalse()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);
        dimension.ToggleFilter("A");

        var item = new TestItem { Category = "B" };

        Assert.That(dimension.Matches(item), Is.False);
    }

    [Test]
    public void Matches_ItemMatchesOneOfMultipleActiveFilters_ShouldReturnTrue()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        var item = new TestItem { Category = "A" };

        Assert.That(dimension.Matches(item), Is.True);
    }

    [Test]
    public void Matches_ItemMatchesSecondOfMultipleActiveFilters_ShouldReturnTrue()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        var item = new TestItem { Category = "B" };

        Assert.That(dimension.Matches(item), Is.True);
    }

    [Test]
    public void Matches_ItemDoesNotMatchAnyActiveFilters_ShouldReturnFalse()
    {
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        var item = new TestItem { Category = "C" };

        Assert.That(dimension.Matches(item), Is.False);
    }

    [Test]
    public void Matches_ImplementsOrLogic()
    {
        // This test demonstrates that within a dimension, OR logic is used
        var f1 = new CategoryFilter("A");
        var f2 = new CategoryFilter("B");
        var dimension = new FilterDimension<TestItem>("Categories", f1, f2);
        dimension.ToggleFilter("A");
        dimension.ToggleFilter("B");

        var itemA = new TestItem { Category = "A" };
        var itemB = new TestItem { Category = "B" };
        var itemC = new TestItem { Category = "C" };

        Assert.That(dimension.Matches(itemA), Is.True, "Should match filter A");
        Assert.That(dimension.Matches(itemB), Is.True, "Should match filter B");
        Assert.That(dimension.Matches(itemC), Is.False, "Should not match any filter");
    }

    [Test]
    public void Name_ShouldBeReadOnly()
    {
        var filter = new CategoryFilter("A");
        var dimension = new FilterDimension<TestItem>("Categories", filter);

        var name = dimension.Name;

        Assert.That(name, Is.EqualTo("Categories"));
        // Name property should not have a setter
    }
}
