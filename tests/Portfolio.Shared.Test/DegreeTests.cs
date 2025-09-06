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
}
