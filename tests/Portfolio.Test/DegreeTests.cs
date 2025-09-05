using Models;

namespace Portfolio.Test;

public class DegreeTests
{
    [Test]
    public void GraduationDate_ShouldBeRequired()
    {
        Degree degree = new()
        {
            Diploma = "BSc",
            Institution = "Test University",
            GraduationDate = new DateOnly(2022, 6, 1)
        };

        Assert.That(degree.GraduationDate, Is.EqualTo(new DateOnly(2022, 6, 1)));
    }
    [Test]
    public void CompareTo_WhenThisGraduationIsMoreRecent_ReturnsBeforeOther()
    {
        Degree newer = new() { GraduationDate = new DateOnly(2023, 6, 1) };
        Degree older = new() { GraduationDate = new DateOnly(2020, 6, 1) };

        Assert.That(newer.CompareTo(older), Is.LessThan(0)); // newer first
    }
}
