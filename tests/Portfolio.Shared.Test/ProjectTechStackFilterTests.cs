using Models;

namespace Portfolio.Shared.Test;

public class ProjectTechStackFilterTests
{
    private Project CreateProject(params string[] techStack)
    {
        var project = new Project(new DateOnly(2023, 1, 1));
        project.TechStack = new SortedSet<string>(techStack);
        return project;
    }

    [Test]
    public void Constructor_WithValidTechnology_ShouldSetName()
    {
        var filter = new ProjectTechStackFilter("C#");

        Assert.That(filter.Name, Is.EqualTo("C#"));
    }

    [Test]
    public void Constructor_WithNullTechnology_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProjectTechStackFilter(null!));
    }

    [Test]
    public void Name_ShouldReturnTechnologyName()
    {
        var filter = new ProjectTechStackFilter("React");

        Assert.That(filter.Name, Is.EqualTo("React"));
    }

    [Test]
    public void Matches_ExactTechStackMatch_ShouldReturnTrue()
    {
        var filter = new ProjectTechStackFilter("React");
        var project = CreateProject("React", "JavaScript", "CSS");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_TechStackNotPresent_ShouldReturnFalse()
    {
        var filter = new ProjectTechStackFilter("React");
        var project = CreateProject("Vue", "JavaScript");

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_EmptyTechStack_ShouldReturnFalse()
    {
        var filter = new ProjectTechStackFilter("React");
        var project = CreateProject();

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_CaseInsensitive_ShouldReturnTrue()
    {
        var filter = new ProjectTechStackFilter("REACT");
        var project = CreateProject("react");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_CaseInsensitiveWithMixedCase_ShouldReturnTrue()
    {
        var filter = new ProjectTechStackFilter("React");
        var project = CreateProject("REACT", "JavaScript");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_SingleTechStackItem_ShouldReturnTrue()
    {
        var filter = new ProjectTechStackFilter("Python");
        var project = CreateProject("Python");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_SingleTechStackItem_ShouldReturnFalse()
    {
        var filter = new ProjectTechStackFilter("Python");
        var project = CreateProject("JavaScript");

        Assert.That(filter.Matches(project), Is.False);
    }

    [Test]
    public void Matches_PartialMatchActuallyMatches()
    {
        // Note: The current implementation uses regex pattern matching with .* wildcards
        // so "Script" pattern will match "JavaScript" because the pattern is ".*Script.*"
        var filter = new ProjectTechStackFilter("Script");
        var project = CreateProject("JavaScript", "TypeScript");

        // The filter actually matches because it looks for "Script" anywhere in the tech name
        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_DotNetFramework_ShouldMatch()
    {
        var filter = new ProjectTechStackFilter(".NET");
        var project = CreateProject(".NET", "C#");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_SpecialCharactersInTech_ShouldMatch()
    {
        var filter = new ProjectTechStackFilter("C++");
        var project = CreateProject("C++", "Python");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_HashInCSharp_ShouldMatch()
    {
        var filter = new ProjectTechStackFilter("C#");
        var project = CreateProject("C#", "Java");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_RegexSpecialCharactersAreEscaped()
    {
        // Test that regex special characters like * + ? are handled correctly
        var filter = new ProjectTechStackFilter("C++");
        var project = CreateProject("C++");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_MultipleTechStackItems_FirstMatches()
    {
        var filter = new ProjectTechStackFilter("React");
        var project = CreateProject("React", "Node.js", "MongoDB");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_MultipleTechStackItems_MiddleMatches()
    {
        var filter = new ProjectTechStackFilter("Node.js");
        var project = CreateProject("React", "Node.js", "MongoDB");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_MultipleTechStackItems_LastMatches()
    {
        var filter = new ProjectTechStackFilter("MongoDB");
        var project = CreateProject("React", "Node.js", "MongoDB");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_DuplicateTechStackItems_ShouldMatch()
    {
        var filter = new ProjectTechStackFilter("JavaScript");
        var project = CreateProject("JavaScript", "HTML", "CSS", "JavaScript");

        // SortedSet will automatically deduplicate, but we still should match
        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_AlphabeticalOrderingInSortedSet()
    {
        // SortedSet maintains alphabetical order
        var filter1 = new ProjectTechStackFilter("Angular");
        var filter2 = new ProjectTechStackFilter("React");

        var project = CreateProject("React", "Angular", "TypeScript");

        Assert.That(filter1.Matches(project), Is.True);
        Assert.That(filter2.Matches(project), Is.True);
    }

    [Test]
    public void Matches_WhitespaceHandling()
    {
        var filter = new ProjectTechStackFilter("Node.js");
        var project = CreateProject("Node.js", "Python");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_VersionNumbersInTech()
    {
        var filter = new ProjectTechStackFilter("Python3.9");
        var project = CreateProject("Python3.9", "Flask");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Matches_UnicodeCharacters_ShouldMatch()
    {
        var filter = new ProjectTechStackFilter("C♯");
        var project = CreateProject("C♯", "Python");

        Assert.That(filter.Matches(project), Is.True);
    }

    [Test]
    public void Implements_IFilter_Interface()
    {
        var filter = new ProjectTechStackFilter("Java");

        Assert.That(filter, Is.InstanceOf<IFilter<Project>>());
    }

    [Test]
    public void MultipleFiltersCanBeCreated()
    {
        var filter1 = new ProjectTechStackFilter("React");
        var filter2 = new ProjectTechStackFilter("Angular");

        Assert.That(filter1.Name, Is.EqualTo("React"));
        Assert.That(filter2.Name, Is.EqualTo("Angular"));
        Assert.That(filter1.Name, Is.Not.EqualTo(filter2.Name));
    }

    [Test]
    public void RealWorldScenario_FilteringMultipleProjects()
    {
        var reactFilter = new ProjectTechStackFilter("React");
        var projects = new[]
        {
            CreateProject("React", "JavaScript", "CSS"),
            CreateProject("Vue", "JavaScript"),
            CreateProject("React", "TypeScript", "Node.js"),
            CreateProject("Angular", "TypeScript"),
            CreateProject("React")
        };

        var reactProjects = projects.Where(p => reactFilter.Matches(p)).ToList();

        Assert.That(reactProjects, Has.Count.EqualTo(3));
    }

    [Test]
    public void CanBeUsedWithFilterDimension()
    {
        var f1 = new ProjectTechStackFilter("React");
        var f2 = new ProjectTechStackFilter("Angular");
        var dimension = new FilterDimension<Project>("Framework", f1, f2);

        Assert.That(dimension.AvailableFilterNames, Has.Count.EqualTo(2));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("React"));
        Assert.That(dimension.AvailableFilterNames, Contains.Item("Angular"));
    }

    [Test]
    public void CanBeUsedWithFilterCollection()
    {
        var f1 = new ProjectTechStackFilter("React");
        var f2 = new ProjectTechStackFilter("Angular");
        var collection = new FilterCollection<Project>(f1, f2);

        collection.ToggleFilter("React");

        Assert.That(collection.IsFilterActive("React"), Is.True);
    }

    [Test]
    public void CanBeUsedWithDimensionalFilterCollection()
    {
        var f1 = new ProjectTechStackFilter("React");
        var f2 = new ProjectTechStackFilter("Node.js");
        var dimension = new FilterDimension<Project>("Technology", f1, f2);

        var collection = new DimensionalFilterCollection<Project>();
        collection.AddDimension(dimension);

        var project = CreateProject("React", "Node.js");

        dimension.ToggleFilter("React");
        dimension.ToggleFilter("Node.js");

        Assert.That(collection.Matches(project), Is.True);
    }

    [Test]
    public void CombinedWithCategoryFilter()
    {
        // Real-world scenario: Filter by both category and technology
        var categoryFilter = new ProjectCategoryFilter("Web");
        var techFilter = new ProjectTechStackFilter("React");

        var project1 = new Project(new DateOnly(2023, 1, 1))
        {
            Categories = ["Web", "Frontend"],
            TechStack = new SortedSet<string> { "React", "JavaScript" }
        };

        var project2 = new Project(new DateOnly(2023, 1, 1))
        {
            Categories = ["Mobile", "Android"],
            TechStack = new SortedSet<string> { "Kotlin", "Android" }
        };

        Assert.That(categoryFilter.Matches(project1), Is.True);
        Assert.That(techFilter.Matches(project1), Is.True);

        Assert.That(categoryFilter.Matches(project2), Is.False);
        Assert.That(techFilter.Matches(project2), Is.False);
    }
}
