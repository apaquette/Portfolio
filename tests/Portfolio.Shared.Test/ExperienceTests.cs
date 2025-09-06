using Models;
using Exceptions;

namespace Portfolio.Shared.Test;

public class ExperienceTests
{
    [Test]
    public void Skills_ShouldInitializeAsEmptyList()
    {
        var exp = new Experience(DateTime.Today);
        Assert.That(exp.Skills, Is.Not.Null);
        Assert.That(exp.Skills, Is.Empty);
    }

    [Test]
    public void Skills_WhenAdded_ShouldBeSortedAlphabetically()
    {
        var exp = new Experience(DateTime.Today) { Skills = ["Zebra", "Apple", "Monkey"] };

        Assert.That(exp.Skills, Is.Ordered.Ascending);
    }

    [Test]
    public void CompareTo_WhenThisJobEndMoreRecent_ReturnsBeforeOther()
    {
        var older = new Experience(new (2019, 1, 1)) { JobEnd = new (2020, 1, 1) };
        var newer = new Experience(new (2020, 1, 1)) { JobEnd = new (2023, 1, 1) };

        Assert.That(newer.CompareTo(older), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_WhenJobEndIsNull_TreatedAsOngoing()
    {
        var past = new Experience(new (2019, 1, 1)) { JobEnd = new (2020, 1, 1) };
        var ongoing = new Experience(new (2021, 1, 1)) { JobEnd = null };

        Assert.That(ongoing.CompareTo(past), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_BothNullJobEnd_ShouldBeEqual()
    {
        var e1 = new Experience(new (2020, 1, 1)) { JobEnd = null };
        var e2 = new Experience(new (2019, 1, 1)) { JobEnd = null };

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
        var e = new Experience(DateTime.Today.AddYears(-1)) { JobEnd = null };

        var result = e.Duration();

        Assert.That(result, Does.Contain("1 yr"));
    }

    [Test]
    public void JobStart_Required()
    {
        Experience exp = new(new DateTime(2022, 6, 1));
        Assert.That(exp.JobStart, Is.EqualTo(new DateTime(2022, 6, 1)));
    }

    [Test]
    public void JobStartIsMissing_ThrowsException()
    {
        Assert.Throws<MissingDateException>(() => new Experience(null));
    }

    [Test]
    public void JobEnd_SetBeforeJobStart_ShouldThrowException()
    {
        var exp = new Experience(new DateTime(2022, 1, 1));
        Assert.Throws<InvalidDateException>(() => exp.JobEnd = new DateTime(2021, 12, 31));
    }

    [Test]
    public void JobEnd_SetEqualToJobStart_ShouldThrowException()
    {
        var date = new DateTime(2022, 1, 1);
        var exp = new Experience(date);

        Assert.Throws<InvalidDateException>(() => exp.JobEnd = date);
    }

    [Test]
    public void PropertiesMatch()
    {
        DateTime jobStart = new(2022, 6, 1);
        Experience exp = new(jobStart)
        {
            Title = "Software Dev",
            Company = "Savage Data",
            Description = "abc descript",
            Location = "Town za",
            EmployerSite = "savage.com"
        };

        Assert.Multiple(() =>
        {
            Assert.That(exp.Title, Is.EqualTo("Software Dev"));
            Assert.That(exp.Company, Is.EqualTo("Savage Data"));
            Assert.That(exp.Description, Is.EqualTo("abc descript"));
            Assert.That(exp.Location, Is.EqualTo("Town za"));
            Assert.That(exp.EmployerSite, Is.EqualTo("savage.com"));
        });
    }
}