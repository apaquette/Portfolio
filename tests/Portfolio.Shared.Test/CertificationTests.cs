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
}