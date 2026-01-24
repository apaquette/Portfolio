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
        var certification = new Certification()
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
}