using Bunit;
using Microsoft.AspNetCore.Components;
using Filtering.Interfaces;
using Filtering.Core;
using Filtering.ProjectFilters;
using Models.Portfolio;
using Portfolio.UI.Primitives;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for FilterBar generic Blazor component
/// Tests rendering, filter toggling, and filter state management
/// </summary>
[TestFixture]
public class FilterBarComponentTests : BunitTestBase
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
    public void FilterBar_WithNullFilterCollection_DoesNotRender()
    {
        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, null)
        );

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("filter-bar"));
    }

    [Test]
    public void FilterBar_WithEmptyFilterCollection_DoesNotRender()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("filter-bar"));
    }

    [Test]
    public void FilterBar_WithFilterCollection_RendersFilterBar()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"), 
            new CategoryFilter("Personal"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("filter-bar"));
    }

    [Test]
    public void FilterBar_DisplaysDimensionName()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Category"));
        Assert.That(cut.Markup, Does.Contain("filter-dimension-label"));
    }

    [Test]
    public void FilterBar_DisplaysFilterButtons()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"),
            new CategoryFilter("Personal"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("[Work]"));
        Assert.That(cut.Markup, Does.Contain("[Personal]"));
        Assert.That(cut.Markup, Does.Contain("btn"));
    }

    [Test]
    public void FilterBar_InactiveFilterUsesOutlineStyle()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("btn-outline-custom"));
        Assert.That(cut.Markup, Does.Not.Contain("btn-custom"));
    }

    [Test]
    public void FilterBar_ActiveFilterUsesSolidStyle()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("Work");

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("btn-custom"));
    }

    [Test]
    public void FilterBar_NoClearButtonWhenNoFiltersActive()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("Clear All Filters"));
        Assert.That(cut.Markup, Does.Not.Contain("btn-outline-danger"));
    }

    [Test]
    public void FilterBar_ShowsClearButtonWhenFiltersActive()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("Work");

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Clear All Filters"));
        Assert.That(cut.Markup, Does.Contain("btn-outline-danger"));
    }

    [Test]
    public void FilterBar_ClickingFilterButtonTogglesFilter()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Act - Click the Work button
        var button = cut.FindAll("button").First(b => b.TextContent.Contains("Work"));
        button.Click();

        // Assert
        Assert.That(dimension.IsFilterActive("Work"), Is.True);
    }

    [Test]
    public void FilterBar_ClickingActiveFilterToggleOff()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("Work");

        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Act - Click the Work button again
        var button = cut.FindAll("button").First(b => b.TextContent.Contains("Work"));
        button.Click();

        // Assert
        Assert.That(dimension.IsFilterActive("Work"), Is.False);
    }

    [Test]
    public void FilterBar_OnFiltersChanged_IsInvokedWhenFilterToggled()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        var callbackInvoked = false;
        var callback = new EventCallback(null, () => { callbackInvoked = true; });

        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
            .Add(p => p.OnFiltersChanged, callback)
        );

        // Act - Click the Work button
        var button = cut.FindAll("button").First(b => b.TextContent.Contains("Work"));
        button.Click();

        // Assert
        Assert.That(callbackInvoked, Is.True);
    }

    [Test]
    public void FilterBar_OnFiltersChanged_IsInvokedWhenClearAllClicked()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("Work");

        var callbackInvoked = false;
        var callback = new EventCallback(null, () => { callbackInvoked = true; });

        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
            .Add(p => p.OnFiltersChanged, callback)
        );

        // Act - Click the Clear All Filters button
        var clearButton = cut.FindAll("button").First(b => b.TextContent.Contains("Clear All Filters"));
        clearButton.Click();

        // Assert
        Assert.That(callbackInvoked, Is.True);
    }

    [Test]
    public void FilterBar_ClearAllFiltersButton_ClearsAllFilters()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Category", new CategoryFilter("Work"));
        var d2 = new FilterDimension<TestItem>("Tags", new TagFilter("Urgent"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);
        d1.ToggleFilter("Work");
        d2.ToggleFilter("Urgent");

        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Act
        var clearButton = cut.FindAll("button").First(b => b.TextContent.Contains("Clear All Filters"));
        clearButton.Click();

        // Assert
        Assert.That(d1.AnyFiltersActive(), Is.False);
        Assert.That(d2.AnyFiltersActive(), Is.False);
    }

    [Test]
    public void FilterBar_MultipleFiltersInMultipleDimensions()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"), 
            new CategoryFilter("Personal"));
        var d2 = new FilterDimension<TestItem>("Priority", 
            new TagFilter("Urgent"), 
            new TagFilter("Normal"));
        collection.AddDimension(d1);
        collection.AddDimension(d2);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Category"));
        Assert.That(cut.Markup, Does.Contain("Priority"));
        Assert.That(cut.Markup, Does.Contain("[Work]"));
        Assert.That(cut.Markup, Does.Contain("[Personal]"));
        Assert.That(cut.Markup, Does.Contain("[Urgent]"));
        Assert.That(cut.Markup, Does.Contain("[Normal]"));
    }

    [Test]
    public void FilterBar_FilterButtons_HaveCorrectClasses()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        var button = cut.Find("button");
        Assert.That(button.ClassList, Does.Contain("btn"));
        Assert.That(button.ClassList, Does.Contain("btn-outline-custom"));
    }

    [Test]
    public void FilterBar_ClearButtonHasCorrectClasses()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);
        dimension.ToggleFilter("Work");

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        var clearButton = cut.FindAll("button").First(b => b.TextContent.Contains("Clear All Filters"));
        Assert.That(clearButton.ClassList, Does.Contain("btn"));
        Assert.That(clearButton.ClassList, Does.Contain("btn-outline-danger"));
    }

    [Test]
    public void FilterBar_DimensionHasCorrectStructure()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Status", 
            new CategoryFilter("Active"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("filter-dimension"));
        Assert.That(cut.Markup, Does.Contain("filter-dimension-label"));
        Assert.That(cut.Markup, Does.Contain("fw-bold"));
        Assert.That(cut.Markup, Does.Contain("d-flex"));
        Assert.That(cut.Markup, Does.Contain("gap-2"));
        Assert.That(cut.Markup, Does.Contain("flex-wrap"));
    }

    [Test]
    public void FilterBar_ManyFiltersInDimension()
    {
        // Arrange
        var filterNames = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };
        var filters = filterNames.Select(name => new CategoryFilter(name)).Cast<IFilter<TestItem>>().ToArray();
        
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Letters", filters);
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        foreach (var name in filterNames)
        {
            Assert.That(cut.Markup, Does.Contain($"[{name}]"));
        }
    }

    [Test]
    public void FilterBar_WithProject_Works()
    {
        // This test demonstrates that FilterBar works with the actual Project model
        // Arrange
        var projectFilter = new DimensionalFilterCollection<Project>();
        var categoryDim = new FilterDimension<Project>("Category",
            new ProjectCategoryFilter("Frontend"),
            new ProjectCategoryFilter("Backend"));
        projectFilter.AddDimension(categoryDim);

        // Act
        var cut = RenderComponent<FilterBar<Project>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, projectFilter)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Category"));
        Assert.That(cut.Markup, Does.Contain("[Frontend]"));
        Assert.That(cut.Markup, Does.Contain("[Backend]"));
    }

    [Test]
    public void FilterBar_FilterButtonsHaveConsistentSpacing()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("A"),
            new CategoryFilter("B"),
            new CategoryFilter("C"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert - Check for flex layout with gap
        Assert.That(cut.Markup, Does.Contain("d-flex"));
        Assert.That(cut.Markup, Does.Contain("gap-2"));
    }

    [Test]
    public void FilterBar_NoFiltersAvailableInDimension_DoesNotRenderDimension()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimensionWithFilters = new FilterDimension<TestItem>("HasFilters", 
            new CategoryFilter("Work"));
        
        // Create a dimension with no filters - this is an edge case
        var emptyDimension = new FilterDimension<TestItem>("Empty");
        
        collection.AddDimension(dimensionWithFilters);
        collection.AddDimension(emptyDimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert - Only the dimension with filters should be rendered
        Assert.That(cut.Markup, Does.Contain("HasFilters"));
        Assert.That(cut.Markup, Does.Not.Contain("Empty"));
    }

    [Test]
    public void FilterBar_RendersCorrectMarkupStructure()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var dimension = new FilterDimension<TestItem>("Category", 
            new CategoryFilter("Work"));
        collection.AddDimension(dimension);

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert - Verify the main structure
        Assert.That(cut.Markup, Does.Contain("filter-bar"));
        Assert.That(cut.Markup, Does.Contain("mb-4"));
    }

    [Test]
    public void FilterBar_WithMultipleDimensionsAndActiveFilters()
    {
        // Arrange
        var collection = new DimensionalFilterCollection<TestItem>();
        var d1 = new FilterDimension<TestItem>("Status", 
            new CategoryFilter("Active"), 
            new CategoryFilter("Inactive"));
        var d2 = new FilterDimension<TestItem>("Type", 
            new TagFilter("Premium"), 
            new TagFilter("Standard"));
        
        collection.AddDimension(d1);
        collection.AddDimension(d2);
        
        d1.ToggleFilter("Active");
        d2.ToggleFilter("Premium");

        // Act
        var cut = RenderComponent<FilterBar<TestItem>>(parameters => parameters
            .Add(p => p.FilterCollectionDimensional, collection)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Status"));
        Assert.That(cut.Markup, Does.Contain("Type"));
        Assert.That(cut.Markup, Does.Contain("btn-custom")); // Active buttons should be solid
    }
}
