using Models;

namespace Portfolio.Shared.Test;

public class CertificationTests
{
    [Test]
    public void Constructor_WhenValidArguments_PropertiesAreCorrect()
    {
        // Arrange
        var earnedOn = new DateOnly(2024,5,1);
        var name = "Azure Fundamentals";
        var issuedBy = "Microsoft";
        var icon = "azure-fundamentals.png";
        var link = "https://learn.microsoft.com/certifications/azure-fundamentals/";

        // Act
        Certification certification = new()
        {
            EarnedOn = earnedOn,
            Name = name,
            IssuedBy = issuedBy,
            Icon = icon,
            Link = link
        };

        // Assert
        Assert.That(certification.EarnedOn, Is.EqualTo(earnedOn));
        Assert.That(certification.Name, Is.EqualTo(name));
        Assert.That(certification.IssuedBy, Is.EqualTo(issuedBy));
        Assert.That(certification.Icon, Is.EqualTo(icon));
        Assert.That(certification.Link, Is.EqualTo(link));
    }

    [Test]
    public void CompareTo_WhenEarnedOnIsEarlier_ReturnsPositiveValue()
    {
        // Arrange
        Certification earlier = new()
        {
            EarnedOn = new DateOnly(2023,1,1),
            Name = "Certification A",
            IssuedBy = "Issuer",
            Icon = "icon-a",
            Link = "link-a"
        };
        Certification later = new()
        {
            EarnedOn = new DateOnly(2024,1,1),
            Name = "Certification B",
            IssuedBy = "Issuer",
            Icon = "icon-b",
            Link = "link-b"
        };

        // Act
        var result = earlier.CompareTo(later);

        // Assert
        Assert.That(result, Is.GreaterThan(0));
    }

    [Test]
    public void CompareTo_WhenEarnedOnIsLater_ReturnsNegativeValue()
    {
        // Arrange
        Certification earlier = new()
        {
            EarnedOn = new DateOnly(2023,1,1),
            Name = "Certification A",
            IssuedBy = "Issuer",
            Icon = "icon-a",
            Link = "link-a"
        };
        Certification later = new()
        {
            EarnedOn = new DateOnly(2024,1,1),
            Name = "Certification B",
            IssuedBy = "Issuer",
            Icon = "icon-b",
            Link = "link-b"
        };

        // Act
        var result = later.CompareTo(earlier);

        // Assert
        Assert.That(result, Is.LessThan(0));
    }

    [Test]
    public void CompareTo_WhenEqual_ReturnsZero()
    {
        // Arrange
        Certification first = new()
        {
            EarnedOn = new DateOnly(2023,1,1),
            Name = "Certification A",
            IssuedBy = "Issuer",
            Icon = "icon-a",
            Link = "link-a"
        };
        Certification second = new()
        {
            EarnedOn = new DateOnly(2023,1,1),
            Name = "Certification B",
            IssuedBy = "Issuer",
            Icon = "icon-b",
            Link = "link-b"
        };

        // Act
        var result = first.CompareTo(second);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CompareTo_WhenNull_ReturnsOne()
    {
        // Arrange
        Certification first = new()
        {
            EarnedOn = new DateOnly(2023,1,1),
            Name = "Certification A",
            IssuedBy = "Issuer",
            Icon = "icon-a",
            Link = "link-a"
        };
        Certification? second = null;

        // Act
        var result = first.CompareTo(second);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void CompareTo_WhenOtherIsNull_ReturnsOne()
    {
        // Arrange
        Certification? other = null;
        Certification cert = new()
        {
            EarnedOn = new DateOnly(2023, 1, 1),
            Name = "AWS Certified",
            IssuedBy = "Amazon"
        };
        
        // Act
        int result = cert.CompareTo(other);
        
        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void CompareTo_SameEarnedDate_ReturnsZero()
    {
        // Arrange
        Certification cert1 = new()
        {
            EarnedOn = new DateOnly(2023, 5, 1),
            Name = "Cert A",
            IssuedBy = "Issuer"
        };
        Certification cert2 = new()
        {
            EarnedOn = new DateOnly(2023, 5, 1),
            Name = "Cert B",
            IssuedBy = "Issuer"
        };
        
        // Act
        int result = cert1.CompareTo(cert2);
        
        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CompareTo_VeryOldCertification_ShouldBeSortedCorrectly()
    {
        // Arrange
        var ancient = new Certification { EarnedOn = new DateOnly(1990, 1, 1), Name = "Old Cert" };
        var modern = new Certification { EarnedOn = new DateOnly(2023, 5, 1), Name = "Modern Cert" };

        // Act
        var result = modern.CompareTo(ancient);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Modern comes first
    }

    [Test]
    public void CompareTo_FutureCertification_ShouldBeSortedCorrectly()
    {
        // Arrange
        var current = new Certification { EarnedOn = new DateOnly(2023, 5, 1), Name = "Current Cert" };
        var future = new Certification { EarnedOn = new DateOnly(2050, 1, 1), Name = "Future Cert" };

        // Act
        var result = future.CompareTo(current);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Future comes first
    }

    [Test]
    public void CompareTo_MultipleCertifications_ShouldMaintainOrder()
    {
        // Arrange
        var certs = new[]
        {
            new Certification { EarnedOn = new DateOnly(2021, 1, 1), Name = "Cert A" },
            new Certification { EarnedOn = new DateOnly(2023, 1, 1), Name = "Cert B" },
            new Certification { EarnedOn = new DateOnly(2022, 1, 1), Name = "Cert C" }
        };

        // Act
        Array.Sort(certs);

        // Assert - Most recent first
        Assert.That(certs[0].EarnedOn, Is.EqualTo(new DateOnly(2023, 1, 1)));
        Assert.That(certs[1].EarnedOn, Is.EqualTo(new DateOnly(2022, 1, 1)));
        Assert.That(certs[2].EarnedOn, Is.EqualTo(new DateOnly(2021, 1, 1)));
    }

    [Test]
    public void PropertiesCanBeNull()
    {
        // Arrange & Act
        var cert = new Certification
        {
            EarnedOn = new DateOnly(2023, 5, 1),
            Name = null,
            IssuedBy = null,
            Icon = null,
            Link = null
        };

        // Assert
        Assert.That(cert.Name, Is.Null);
        Assert.That(cert.IssuedBy, Is.Null);
        Assert.That(cert.Icon, Is.Null);
        Assert.That(cert.Link, Is.Null);
    }

    [Test]
    public void PropertiesCanBeEmpty()
    {
        // Arrange & Act
        var cert = new Certification
        {
            EarnedOn = new DateOnly(2023, 5, 1),
            Name = "",
            IssuedBy = "",
            Icon = "",
            Link = ""
        };

        // Assert
        Assert.That(cert.Name, Is.EqualTo(""));
        Assert.That(cert.IssuedBy, Is.EqualTo(""));
    }

    [Test]
    [TestCase(2020, 1, 1)]
    [TestCase(2023, 12, 31)]
    [TestCase(2015, 6, 15)]
    public void EarnedOn_VariousDates_ShouldBeValid(int year, int month, int day)
    {
        // Arrange & Act
        var cert = new Certification { EarnedOn = new DateOnly(year, month, day) };

        // Assert
        Assert.That(cert.EarnedOn.Year, Is.EqualTo(year));
        Assert.That(cert.EarnedOn.Month, Is.EqualTo(month));
        Assert.That(cert.EarnedOn.Day, Is.EqualTo(day));
    }

    [Test]
    public void Certification_WithLeapYearDate_ShouldBeValid()
    {
        // Arrange & Act
        var cert = new Certification
        {
            EarnedOn = new DateOnly(2020, 2, 29),
            Name = "Leap Day Cert"
        };

        // Assert
        Assert.That(cert.EarnedOn, Is.EqualTo(new DateOnly(2020, 2, 29)));
    }

    [Test]
    public void Certification_CanHaveVeryLongStrings()
    {
        // Arrange
        var longName = new string('A', 1000);
        var longIssuer = new string('B', 500);

        // Act
        var cert = new Certification
        {
            EarnedOn = new DateOnly(2023, 5, 1),
            Name = longName,
            IssuedBy = longIssuer
        };

        // Assert
        Assert.That(cert.Name, Is.EqualTo(longName));
        Assert.That(cert.IssuedBy, Is.EqualTo(longIssuer));
    }
}