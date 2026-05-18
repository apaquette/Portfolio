using Models;
using Exceptions;

namespace Portfolio.Shared.Test;

public class DegreeTests
{
    [Test]
    public void GradDate_Required()
    {
        Degree degree = new(new DateOnly(2022, 6, 1))
        {
            Diploma = "BSc",
            Institution = "Test University"
        };

        Assert.That(degree.GraduationDate, Is.EqualTo(new DateOnly(2022, 6, 1)));
    }
    [Test]
    public void GraduationIsMoreRecent_ReturnsBeforeOther()
    {
        Degree newer = new(new DateOnly(2023, 6, 1));
        Degree older = new(new DateOnly(2020, 6, 1));

        Assert.That(newer.CompareTo(older), Is.LessThan(0)); // newer first
    }

    [Test]
    public void GraduationIsLessRecent_ReturnsAfterOther()
    {
        Degree newer = new(new DateOnly(2023, 6, 1));
        Degree older = new(new DateOnly(2020, 6, 1));

        Assert.That(older.CompareTo(newer), Is.GreaterThan(0)); // newer first
    }

    [Test]
    public void GraduationDatesAreEqual_ReturnsZero()
    {
        Degree d1 = new(new DateOnly(2022, 6, 1));
        Degree d2 = new(new DateOnly(2022, 6, 1));

        Assert.That(d1.CompareTo(d2), Is.EqualTo(0));
    }

    [Test]
    public void GraduationIsMissing_ThrowsException()
    {
        Assert.Throws<MissingDateException>(() => new Degree(default));
    }

    [Test]
    public void PropertiesMatch()
    {
        DateOnly gradDate = new(2022, 6, 1);
        Degree degree = new(gradDate)
        {
            Diploma = "BSc (hons)",
            Institution = "Canadore",
            Logo = "path/to/logo.jpg",
            Website = "canadore.ca"
        };

        Assert.Multiple(() =>
        {
            Assert.That(degree.Diploma, Is.EqualTo("BSc (hons)"));
            Assert.That(degree.Institution, Is.EqualTo("Canadore"));
            Assert.That(degree.Logo, Is.EqualTo("path/to/logo.jpg"));
            Assert.That(degree.Website, Is.EqualTo("canadore.ca"));
        });
    }

    [Test]
    public void CompareTo_WhenOtherIsNull_ReturnsOne()
    {
        // Arrange
        Degree? other = null;
        Degree degree = new(new(2022, 6, 1))
        {
            Diploma = "BSc (hons)",
            Institution = "Canadore",
            Logo = "path/to/logo.jpg",
            Website = "canadore.ca"
        };

        // Act
        int result = degree.CompareTo(other);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GraduationDate_VeryOldDate_ShouldBeSortedCorrectly()
    {
        // Arrange
        Degree ancient = new(new DateOnly(1980, 6, 1));
        Degree modern = new(new DateOnly(2022, 6, 1));

        // Act
        var result = modern.CompareTo(ancient);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Modern comes first
    }

    [Test]
    public void GraduationDate_FarFutureDate_ShouldBeSortedCorrectly()
    {
        // Arrange
        Degree current = new(new DateOnly(2022, 6, 1));
        Degree future = new(new DateOnly(2100, 6, 1));

        // Act
        var result = future.CompareTo(current);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Future comes first
    }

    [Test]
    public void CompareTo_MultipleComparisons_ShouldMaintainOrder()
    {
        // Arrange
        var degrees = new[]
        {
            new Degree(new DateOnly(2018, 6, 1)),
            new Degree(new DateOnly(2020, 6, 1)),
            new Degree(new DateOnly(2019, 6, 1))
        };

        // Act
        Array.Sort(degrees);

        // Assert - Most recent first
        Assert.That(degrees[0].GraduationDate, Is.EqualTo(new DateOnly(2020, 6, 1)));
        Assert.That(degrees[1].GraduationDate, Is.EqualTo(new DateOnly(2019, 6, 1)));
        Assert.That(degrees[2].GraduationDate, Is.EqualTo(new DateOnly(2018, 6, 1)));
    }

    [Test]
    public void Degree_WithLeapYearDate_ShouldBeValid()
    {
        // Arrange & Act
        Degree degree = new(new DateOnly(2020, 2, 29))
        {
            Diploma = "Special Degree",
            Institution = "Leap Year University"
        };

        // Assert
        Assert.That(degree.GraduationDate, Is.EqualTo(new DateOnly(2020, 2, 29)));
    }

    [Test]
    public void PropertiesCanBeNull()
    {
        // Arrange & Act
        Degree degree = new(new DateOnly(2022, 6, 1))
        {
            Diploma = null,
            Institution = null,
            Logo = null,
            Website = null
        };

        // Assert
        Assert.That(degree.Diploma, Is.Null);
        Assert.That(degree.Institution, Is.Null);
        Assert.That(degree.Logo, Is.Null);
        Assert.That(degree.Website, Is.Null);
    }

    [Test]
    public void Degree_CanHaveEmptyStrings()
    {
        // Arrange & Act
        Degree degree = new(new DateOnly(2022, 6, 1))
        {
            Diploma = "",
            Institution = "",
            Logo = "",
            Website = ""
        };

        // Assert
        Assert.That(degree.Diploma, Is.EqualTo(""));
        Assert.That(degree.Institution, Is.EqualTo(""));
    }

    [Test]
    [TestCase(2020, 1, 1)]
    [TestCase(2020, 12, 31)]
    [TestCase(2010, 6, 15)]
    public void GraduationDate_VariousDates_ShouldBeValid(int year, int month, int day)
    {
        // Arrange & Act
        Degree degree = new(new DateOnly(year, month, day));

        // Assert
        Assert.That(degree.GraduationDate.Year, Is.EqualTo(year));
        Assert.That(degree.GraduationDate.Month, Is.EqualTo(month));
        Assert.That(degree.GraduationDate.Day, Is.EqualTo(day));
    }
}
