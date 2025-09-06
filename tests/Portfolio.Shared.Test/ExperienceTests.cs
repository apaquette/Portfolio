using Models;

namespace Portfolio.Shared.Test;

public class ExperienceTests
{
    [Test]
    public void Skills_ShouldInitializeAsEmptyList()
    {
        var exp = new Experience(DateTime.Today) { JobStart = DateTime.Today };
        Assert.That(exp.Skills, Is.Not.Null);
        Assert.That(exp.Skills, Is.Empty);
    }

    [Test]
    public void Skills_WhenAdded_ShouldBeSortedAlphabetically()
    {
        var exp = new Experience(DateTime.Today) { JobStart = DateTime.Today, Skills = new() { "Zebra", "Apple", "Monkey" } };

        var sorted = exp.Skills.ToList();
        Assert.That(sorted, Is.EqualTo(new[] { "Apple", "Monkey", "Zebra" }));
    }

    [Test]
    public void CompareTo_WhenThisJobEndMoreRecent_ReturnsBeforeOther()
    {
        var older = new Experience(new DateTime(2019,1,1)) { JobStart = new DateTime(2019, 1, 1), JobEnd = new DateTime(2020, 1, 1) };
        var newer = new Experience(new DateTime(2020,1,1)) { JobStart = new DateTime(2020, 1, 1), JobEnd = new DateTime(2023, 1, 1) };

        Assert.That(newer.CompareTo(older), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_WhenJobEndIsNull_TreatedAsOngoing()
    {
        var past = new Experience(new DateTime(2019,1,1)) { JobStart = new DateTime(2019, 1, 1), JobEnd = new DateTime(2020, 1, 1) };
        var ongoing = new Experience(new DateTime(2021,1,1)) { JobStart = new DateTime(2021, 1, 1), JobEnd = null };

        Assert.That(ongoing.CompareTo(past), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_BothNullJobEnd_ShouldBeEqual()
    {
        var e1 = new Experience(new DateTime(2020, 1, 1)) { JobStart = new DateTime(2020, 1, 1), JobEnd = null };
        var e2 = new Experience(new DateTime(2019, 1, 1)) { JobStart = new DateTime(2019, 1, 1), JobEnd = null };

        Assert.That(e1.CompareTo(e2), Is.EqualTo(0));
    }


    //TODO: Add edge case testing
    [TestCase("2020-01-01", "2022-01-01", "2 yrs")]
    [TestCase("2020-01-01", "2022-04-01", "2 yrs 3 mos")]
    [TestCase("2021-01-01", "2021-06-01", "5 mos")]
    public void Duration_ShouldReturnExpectedFormat(string start, string end, string expected)
    {
        var e = new Experience(DateTime.Parse(start))
        {
            JobStart = DateTime.Parse(start),
            JobEnd = DateTime.Parse(end)
        };

        Assert.That(e.Duration(), Is.EqualTo(expected));
    }

    [Test]
    public void Duration_NullJobEnd_UsesToday()
    {
        var e = new Experience(DateTime.Today.AddYears(-1)) { JobStart = DateTime.Today.AddYears(-1), JobEnd = null };

        var result = e.Duration();

        Assert.That(result, Does.Contain("1 yr"));
    }

    [Test]
    public void JobStart_Required()
    {
        Experience exp = new(new DateTime(2022, 6, 1));
        Assert.That(exp.JobStart, Is.EqualTo(new DateTime(2022, 6, 1)));
    }
}