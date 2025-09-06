using Models;
using Exceptions;

namespace Portfolio.Shared.Test;

public class DegreeTests
{
    [Test]
    public void GradDateRequired()
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
        Assert.Throws<MissingGradDateException>(() => new Degree(null));
    }
}
