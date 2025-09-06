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
        Assert.Throws<MissingDateException>(() => new Project(null));
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
}