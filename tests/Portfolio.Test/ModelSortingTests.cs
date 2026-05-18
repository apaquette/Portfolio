using Models;
using NUnit.Framework;
using Portfolio.Test.Mocks;

namespace Portfolio.Test;

[TestFixture]
public class ModelSortingTests
{
    [Test]
    public void ExperienceCollection_WhenSorted_ArrangedByMostRecentJobEnd()
    {
        // Arrange
        var experiences = new[]
        {
            new Experience(new DateTime(2020, 1, 1)) { JobEnd = new DateTime(2022, 6, 1) },
            new Experience(new DateTime(2019, 1, 1)) { JobEnd = new DateTime(2021, 6, 1) },
            new Experience(new DateTime(2021, 1, 1)) { JobEnd = new DateTime(2023, 6, 1) }
        };

        // Act
        Array.Sort(experiences);

        // Assert
        Assert.That(experiences[0].JobEnd, Is.EqualTo(new DateTime(2023, 6, 1)));
        Assert.That(experiences[1].JobEnd, Is.EqualTo(new DateTime(2022, 6, 1)));
        Assert.That(experiences[2].JobEnd, Is.EqualTo(new DateTime(2021, 6, 1)));
    }

    [Test]
    public void ProjectCollection_WhenSorted_ArrangedByMostRecentCompletion()
    {
        // Arrange
        var projects = new[]
        {
            new Project(new DateOnly(2020, 6, 1)),
            new Project(new DateOnly(2022, 6, 1)),
            new Project(new DateOnly(2021, 6, 1))
        };

        // Act
        Array.Sort(projects);

        // Assert
        Assert.That(projects[0].Completed, Is.EqualTo(new DateOnly(2022, 6, 1)));
        Assert.That(projects[1].Completed, Is.EqualTo(new DateOnly(2021, 6, 1)));
        Assert.That(projects[2].Completed, Is.EqualTo(new DateOnly(2020, 6, 1)));
    }

    [Test]
    public void DegreeCollection_WhenSorted_ArrangedByMostRecentGraduation()
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

        // Assert
        Assert.That(degrees[0].GraduationDate, Is.EqualTo(new DateOnly(2020, 6, 1)));
        Assert.That(degrees[1].GraduationDate, Is.EqualTo(new DateOnly(2019, 6, 1)));
        Assert.That(degrees[2].GraduationDate, Is.EqualTo(new DateOnly(2018, 6, 1)));
    }

    [Test]
    public void CertificationCollection_WhenSorted_ArrangedByMostRecentEarned()
    {
        // Arrange
        var certs = new[]
        {
            CertificationBuilder.ValidCertification(),
            new Certification { EarnedOn = new DateOnly(2024, 1, 1), Name = "Newer Cert", IssuedBy = "Issuer" },
            new Certification { EarnedOn = new DateOnly(2022, 1, 1), Name = "Older Cert", IssuedBy = "Issuer" }
        };

        // Act
        Array.Sort(certs);

        // Assert
        Assert.That(certs[0].EarnedOn, Is.EqualTo(new DateOnly(2024, 1, 1)));
        Assert.That(certs[2].EarnedOn, Is.EqualTo(new DateOnly(2022, 1, 1)));
    }
}