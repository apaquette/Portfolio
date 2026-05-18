using Models.Portfolio;
using Filtering.Interfaces;
using Filtering.ProjectFilters;
using Filtering.Core;

namespace Portfolio.Shared.Filter.Test;

public class ProjectCategoryFilterTests
{
    private static Project CreateProject(params string[] categories)
    {
        return new Project(new DateOnly(2023, 1, 1)) { Categories = categories };
    }

    [Test]
    public void Constructor_WithValidCategory_ShouldSetName()
    {
        var filter = new ProjectCategoryFilter("WebDevelopment");

        Assert.That(filter.Name, Is.EqualTo("WebDevelopment"));
    }

    [Test]
    public void Constructor_WithNullCategory_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProjectCategoryFilter(null!));
    }

    [Test]
    public void Name_ShouldReturnCategoryName()
    {
        var filter = new ProjectCategoryFilter("Backend");

        Assert.That(filter.Name, Is.EqualTo("Backend"));
    }

    [Test]
    public void Matches_ProjectWithMatchingCategory_ShouldReturnTrue()
    {
        var filter = new ProjectCategoryFilter("WebDevelopment");
        var project = CreateProject("WebDevelopment", "Frontend");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_ProjectWithoutMatchingCategory_ShouldReturnFalse()
    {
        var filter = new ProjectCategoryFilter("WebDevelopment");
        var project = CreateProject("Backend", "Database");

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_ProjectWithCategoryAsSingleItem_ShouldReturnTrue()
    {
        var filter = new ProjectCategoryFilter("Mobile");
        var project = CreateProject("Mobile");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_EmptyCategoriesArray_ShouldReturnFalse()
    {
        var filter = new ProjectCategoryFilter("WebDevelopment");
        var project = CreateProject();

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_CaseInsensitive_ShouldReturnTrue()
    {
        var filter = new ProjectCategoryFilter("WEBDEVELOPMENT");
        var project = CreateProject("webdevelopment");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_CaseInsensitiveWithMixedCase_ShouldReturnTrue()
    {
        var filter = new ProjectCategoryFilter("WebDevelopment");
        var project = CreateProject("webdevelopment", "other");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_PartialMatchShouldNotMatch()
    {
        var filter = new ProjectCategoryFilter("Web");
        var project = CreateProject("WebDevelopment");

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_MultipleCategories_OnlyOneMatches()
    {
        var filter = new ProjectCategoryFilter("Frontend");
        var project = CreateProject("Backend", "Frontend", "Database");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_MultipleCategories_FirstMatches()
    {
        var filter = new ProjectCategoryFilter("Backend");
        var project = CreateProject("Backend", "Frontend", "Database");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_MultipleCategories_LastMatches()
    {
        var filter = new ProjectCategoryFilter("Database");
        var project = CreateProject("Backend", "Frontend", "Database");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_ProjectWithDuplicateCategories_ShouldReturnTrue()
    {
        var filter = new ProjectCategoryFilter("API");
        var project = CreateProject("API", "API", "REST");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_SpecialCharactersInCategory_ShouldMatchExactly()
    {
        var filter = new ProjectCategoryFilter("C++");
        var project = CreateProject("C++", "Python");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_WhitespaceInCategory_ShouldNotMatch()
    {
        var filter = new ProjectCategoryFilter("Web Development");
        var project = CreateProject("WebDevelopment");

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_WhitespaceHandling_ExactMatch()
    {
        var filter = new ProjectCategoryFilter("Web Development");
        var project = CreateProject("Web Development");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Implements_IFilter_Interface()
    {
        var filter = new ProjectCategoryFilter("Test");

        Assert.That(filter, Is.InstanceOf<IFilter<Project>>());
    }

    [Test]
    public void MultipleFiltersCanBeCreated()
    {
        var filter1 = new ProjectCategoryFilter("Frontend");
        var filter2 = new ProjectCategoryFilter("Backend");

        Assert.That(filter1.Name, Is.EqualTo("Frontend"));
        Assert.That(filter2.Name, Is.EqualTo("Backend"));
        Assert.That(filter1.Name, Is.Not.EqualTo(filter2.Name));
    }

    [Test]
    public void RealWorldScenario_FilteringMultipleProjects()
    {
        var backendFilter = new ProjectCategoryFilter("Backend");
        var projects = new[]
        {
            CreateProject("Frontend", "React"),
            CreateProject("Backend", "API"),
            CreateProject("Backend", "Database"),
            CreateProject("Mobile", "Android"),
            CreateProject("Frontend", "Backend", "Full-Stack")
        };

        var backendProjects = projects.Where(p => backendFilter.Matches(p)).ToList();

        Assert.That(backendProjects, Has.Count.EqualTo(3));
    }

    [Test]
    public void CanBeUsedWithFilterDimension()
    {
        var f1 = new ProjectCategoryFilter("Frontend");
        var f2 = new ProjectCategoryFilter("Backend");
        var dimension = new FilterDimension<Project>("Category", f1, f2);

        Assert.That(dimension.AvailableFilterNames, Has.Count.EqualTo(2));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("Frontend"));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("Backend"));
    }

    [Test]
    public void CanBeUsedWithFilterCollection()
    {
        var f1 = new ProjectCategoryFilter("Frontend");
        var f2 = new ProjectCategoryFilter("Backend");
        var collection = new FilterCollection<Project>(f1, f2);

        collection.ToggleFilter("Frontend");

        Assert.That(collection.IsFilterActive("Frontend"), Is.True);
    }
}
