using Models;
using Exceptions;

namespace Portfolio.Shared.Test;

public class ProjectTests
{
    [Test]
    public void CompareTo_MoreRecentCompleted_ShouldComeFirst()
    {
        var older = new Project(new DateOnly(2020, 6, 1));
        var newer = new Project(new DateOnly(2022, 6, 1));

        Assert.That(newer.CompareTo(older), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_SameCompletedDate_ShouldBeEqual()
    {
        var p1 = new Project(new DateOnly(2021, 6, 1));
        var p2 = new Project(new DateOnly(2021, 6, 1));

        Assert.That(p1.CompareTo(p2), Is.EqualTo(0));
    }

    [Test]
    public void TechStack_ShouldAlwaysBeAlphabetical()
    {
        var project = new Project(new DateOnly(2021, 6, 1)) { TechStack = ["Zebra", "Apple", "Monkey"] };
        Assert.That(project.TechStack, Is.Ordered.Ascending);
    }

    [Test]
    public void CompareTo_NullProperties_ShouldNotAffectComparison()
    {
        var p1 = new Project(new DateOnly(2022, 6, 1)) { Title = null };
        var p2 = new Project(new DateOnly(2020, 6, 1)) { Description = null };

        Assert.That(p1.CompareTo(p2), Is.LessThan(0));
    }

    [Test]
    public void DateCompleted_Required()
    {
        Project proj = new(new DateOnly(2022, 6, 1));

        Assert.That(proj.Completed, Is.EqualTo(new DateOnly(2022, 6, 1)));
    }

    [Test]
    public void CompletionDateIsMissing_ThrowsException()
    {
        Assert.Throws<MissingDateException>(() => new Project(default));
    }

    [Test]
    public void PropertiesMatch()
    {
        DateOnly completed = new(2022, 6, 1);
        Project proj = new(completed)
        {
            Title = "Portfolio",
            Description = "abc descript",
            Link = "abc.com",
            ImageSource = "path/to/image.png"
        };

        Assert.Multiple(() =>
        {
            Assert.That(proj.Title, Is.EqualTo("Portfolio"));
            Assert.That(proj.Description, Is.EqualTo("abc descript"));
            Assert.That(proj.Link, Is.EqualTo("abc.com"));
            Assert.That(proj.ImageSource, Is.EqualTo("path/to/image.png"));
        });
    }

    [Test]
    public void CompareTo_WhenOtherIsNull_ReturnsOne()
    {
        // Arrange
        Project? other = null;
        Project proj = new(new(2022, 6, 1))
        {
            Title = "Portfolio",
            Description = "abc descript",
            Link = "abc.com",
            ImageSource = "path/to/image.png"
        };

        // Act
        int result = proj.CompareTo(other);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    [TestCase("1980-01-01")] // Very old project
    [TestCase("2100-12-31")] // Far future
    public void CompareTo_WithExtremeDates_ShouldWorkCorrectly(string dateStr)
    {
        // Arrange
        var project = new Project(DateOnly.Parse(dateStr));
        var recentProject = new Project(new DateOnly(2020, 6, 1));
        
        // Act
        int result = project.CompareTo(recentProject);
        
        // Assert
        Assert.That(result, Is.Not.EqualTo(0));
    }

    [Test]
    public void TechStack_Empty_ShouldBeValid()
    {
        // Arrange & Act
        var project = new Project(new DateOnly(2021, 6, 1));

        // Assert
        Assert.That(project.TechStack.Count, Is.EqualTo(0));
    }

    [Test]
    public void TechStack_SingleItem_ShouldBeValid()
    {
        // Arrange & Act
        var project = new Project(new DateOnly(2021, 6, 1))
        {
            TechStack = new() { "C#" }
        };

        // Assert
        Assert.That(project.TechStack.Count, Is.EqualTo(1));
        Assert.That(project.TechStack.Contains("C#"), Is.True);
    }

    [Test]
    public void CompareTo_MultipleProjects_ShouldMaintainOrder()
    {
        // Arrange
        var projects = new[]
        {
            new Project(new DateOnly(2020, 6, 1)),
            new Project(new DateOnly(2022, 6, 1)),
            new Project(new DateOnly(2021, 6, 1))
        };

        // Act
        Array.Sort(projects);

        // Assert - Most recent first
        Assert.That(projects[0].Completed, Is.EqualTo(new DateOnly(2022, 6, 1)));
        Assert.That(projects[1].Completed, Is.EqualTo(new DateOnly(2021, 6, 1)));
        Assert.That(projects[2].Completed, Is.EqualTo(new DateOnly(2020, 6, 1)));
    }

    [Test]
    public void PropertiesCanBeNull()
    {
        // Arrange & Act
        var project = new Project(new DateOnly(2021, 6, 1))
        {
            Title = null,
            Description = null,
            Link = null,
            ImageSource = null
        };

        // Assert
        Assert.That(project.Title, Is.Null);
        Assert.That(project.Description, Is.Null);
        Assert.That(project.Link, Is.Null);
        Assert.That(project.ImageSource, Is.Null);
    }

    [Test]
    public void ProjectCanHaveVeryLongStrings()
    {
        // Arrange
        var longTitle = new string('A', 1000);
        var longDescription = new string('B', 5000);

        // Act
        var project = new Project(new DateOnly(2021, 6, 1))
        {
            Title = longTitle,
            Description = longDescription
        };

        // Assert
        Assert.That(project.Title, Is.EqualTo(longTitle));
        Assert.That(project.Description, Is.EqualTo(longDescription));
    }

    [Test]
    [TestCase(2020, 1, 1)]
    [TestCase(2020, 12, 31)]
    [TestCase(2015, 6, 15)]
    public void CompletionDate_VariousDates_ShouldBeValid(int year, int month, int day)
    {
        // Arrange & Act
        var project = new Project(new DateOnly(year, month, day));

        // Assert
        Assert.That(project.Completed.Year, Is.EqualTo(year));
        Assert.That(project.Completed.Month, Is.EqualTo(month));
        Assert.That(project.Completed.Day, Is.EqualTo(day));
    }

    [Test]
    public void Project_WithLeapYearDate_ShouldBeValid()
    {
        // Arrange & Act
        var project = new Project(new DateOnly(2020, 2, 29))
        {
            Title = "Leap Day Project"
        };

        // Assert
        Assert.That(project.Completed, Is.EqualTo(new DateOnly(2020, 2, 29)));
    }
}