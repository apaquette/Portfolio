using Models;

namespace Portfolio.Shared.Test;

public class ExperienceTests
{
    [Test]
    public void Skills_ShouldInitializeAsEmptyList()
    {
        var exp = new Experience { JobStart = DateTime.Today };
        Assert.That(exp.Skills, Is.Not.Null);
        Assert.That(exp.Skills, Is.Empty);
    }

    [Test]
    public void Skills_WhenAdded_ShouldBeSortedAlphabetically()
    {
        var exp = new Experience { JobStart = DateTime.Today, Skills = new() { "Zebra", "Apple", "Monkey" } };

        var sorted = exp.Skills.ToList();
        Assert.That(sorted, Is.EqualTo(new[] { "Apple", "Monkey", "Zebra" }));
    }

    [Test]
    public void CompareTo_WhenThisJobEndMoreRecent_ReturnsBeforeOther()
    {
        var older = new Experience { JobStart = new DateTime(2019, 1, 1), JobEnd = new DateTime(2020, 1, 1) };
        var newer = new Experience { JobStart = new DateTime(2020, 1, 1), JobEnd = new DateTime(2023, 1, 1) };

        Assert.That(newer.CompareTo(older), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_WhenJobEndIsNull_TreatedAsOngoing()
    {
        var past = new Experience { JobStart = new DateTime(2019, 1, 1), JobEnd = new DateTime(2020, 1, 1) };
        var ongoing = new Experience { JobStart = new DateTime(2021, 1, 1), JobEnd = null };

        Assert.That(ongoing.CompareTo(past), Is.LessThan(0));
    }
}