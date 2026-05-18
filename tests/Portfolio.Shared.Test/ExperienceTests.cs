using Models;
using Exceptions;

namespace Portfolio.Shared.Test;

public class ExperienceTests
{
    private readonly string today = DateTime.Today.ToString("yyyy-MM-dd");
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
        var older = new Experience(new(2019, 1, 1)) { JobEnd = new(2020, 1, 1) };
        var newer = new Experience(new(2020, 1, 1)) { JobEnd = new(2023, 1, 1) };

        Assert.That(newer.CompareTo(older), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_WhenJobEndIsNull_TreatedAsOngoing()
    {
        var past = new Experience(new(2019, 1, 1)) { JobEnd = new(2020, 1, 1) };
        var ongoing = new Experience(new(2021, 1, 1)) { JobEnd = null };

        Assert.That(ongoing.CompareTo(past), Is.LessThan(0));
    }

    [Test]
    public void CompareTo_BothNullJobEnd_ShouldBeEqual()
    {
        var e1 = new Experience(new(2020, 1, 1)) { JobEnd = null };
        var e2 = new Experience(new(2019, 1, 1)) { JobEnd = null };

        Assert.That(e1.CompareTo(e2), Is.EqualTo(0));
    }


    [Test]
    [TestCase("2020-01-01", "2022-01-01", "(2 yrs)")]
    [TestCase("2020-01-01", "2022-04-01", "(2 yrs 3 mos)")]
    [TestCase("2021-01-01", "2021-06-01", "(5 mos)")]
    [TestCase("2023-01-01", "2023-12-31", "(11 mos)")]
    [TestCase("2022-01-01", "2024-01-01", "(2 yrs)")]
    [TestCase("2023-06-15", "2023-06-20", "")]
    [TestCase("2020-01-01", "2021-02-01", "(1 yr 1 mo)")]
    [TestCase("2020-01-01", "2020-02-01", "(1 mo)")]
    [TestCase("2020-05-15", "2021-03-10", "(9 mos)")]
    [TestCase("2022-03-31", "2023-02-28", "(10 mos)")]
    [TestCase("2022-11-15", "2024-01-10", "(1 yr 1 mo)")]
    public void Duration_ShouldReturnExpectedFormat(string start, string end, string expected)
    {
        var e = new Experience(DateTime.Parse(start))
        {
            JobStart = DateTime.Parse(start),
            JobEnd = end is not null ? DateTime.Parse(end) : null
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
        Assert.Throws<MissingDateException>(() => new Experience(default));
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

    [Test]
    public void CompareTo_WhenOtherIsNull_ReturnsOne()
    {
        // Arrange
        Experience? other = null;
        Experience exp = new(new(2022, 6, 1))
        {
            Title = "Software Dev",
            Company = "Savage Data",
            Description = "abc descript",
            Location = "Town za",
            EmployerSite = "savage.com"
        };

        // Act
        int result = exp.CompareTo(other);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void JobStart_RequiredByConstructor_ShouldThrowWhenDefault()
    {
        // This should throw MissingDateException
        Assert.Throws<MissingDateException>(() => new Experience(default));
    }

    [Test]
    public void Duration_VeryLongDuration_OverTenYears_ShouldFormat()
    {
        // Arrange
        var e = new Experience(new DateTime(2010, 1, 1))
        {
            JobEnd = new DateTime(2024, 5, 1)
        };

        // Act
        var result = e.Duration();

        // Assert
        Assert.That(result, Does.Contain("14 yrs"));
    }

    [Test]
    public void Duration_LeapYearBoundary_ShouldCalculateCorrectly()
    {
        // Arrange - Feb 29 to next year
        var e = new Experience(new DateTime(2020, 2, 29))
        {
            JobEnd = new DateTime(2021, 2, 28)
        };

        // Act
        var result = e.Duration();

        // Assert
        Assert.That(result, Is.EqualTo("(11 mos)"));
    }

    [Test]
    public void CompareTo_IdenticalJobEndDates_ShouldBeEqual()
    {
        // Arrange
        var e1 = new Experience(new DateTime(2020, 1, 1)) { JobEnd = new DateTime(2022, 6, 1) };
        var e2 = new Experience(new DateTime(2019, 1, 1)) { JobEnd = new DateTime(2022, 6, 1) };

        // Act
        var result = e1.CompareTo(e2);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Skills_WhenEmpty_ShouldRemainEmpty()
    {
        // Arrange
        var exp = new Experience(DateTime.Today);

        // Act
        var skills = exp.Skills;

        // Assert
        Assert.That(skills.Count, Is.EqualTo(0));
    }

    [Test]
    public void Skills_WithSingleSkill_ShouldBeValid()
    {
        // Arrange
        var exp = new Experience(DateTime.Today)
        {
            Skills = new() { "C#" }
        };

        // Act & Assert
        Assert.That(exp.Skills.Count, Is.EqualTo(1));
        Assert.That(exp.Skills.Contains("C#"), Is.True);
    }

    [Test]
    public void JobEnd_CanBeSetToFutureDate()
    {
        // Arrange
        var exp = new Experience(new DateTime(2020, 1, 1));
        var futureDate = DateTime.Now.AddYears(2);

        // Act
        exp.JobEnd = futureDate;

        // Assert
        Assert.That(exp.JobEnd, Is.EqualTo(futureDate));
    }

    [Test]
    public void PropertiesCanBeNull()
    {
        // Arrange & Act
        var exp = new Experience(new DateTime(2020, 1, 1))
        {
            Title = null,
            Company = null,
            Description = null,
            Location = null,
            EmployerSite = null
        };

        // Assert
        Assert.That(exp.Title, Is.Null);
        Assert.That(exp.Company, Is.Null);
        Assert.That(exp.Description, Is.Null);
        Assert.That(exp.Location, Is.Null);
        Assert.That(exp.EmployerSite, Is.Null);
    }

    [Test]
    public void CompareTo_MultipleComparisons_MaintainsConsistency()
    {
        // Arrange
        var experiences = new[]
        {
            new Experience(new DateTime(2020, 1, 1)) { JobEnd = new DateTime(2021, 6, 1) },
            new Experience(new DateTime(2019, 1, 1)) { JobEnd = new DateTime(2022, 6, 1) },
            new Experience(new DateTime(2021, 1, 1)) { JobEnd = new DateTime(2023, 6, 1) }
        };

        // Act
        Array.Sort(experiences);

        // Assert - Should be ordered by most recent JobEnd
        Assert.That(experiences[0].JobEnd, Is.EqualTo(new DateTime(2023, 6, 1)));
        Assert.That(experiences[1].JobEnd, Is.EqualTo(new DateTime(2022, 6, 1)));
        Assert.That(experiences[2].JobEnd, Is.EqualTo(new DateTime(2021, 6, 1)));
    }
}