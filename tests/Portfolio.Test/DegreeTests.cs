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
}
