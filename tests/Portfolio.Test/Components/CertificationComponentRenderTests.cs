using Bunit;
using Portfolio.Layout.Components;
using Models;
using Portfolio.Pages.ExperiencePage.Components;

namespace Portfolio.Test.Components;

[TestFixture]
public class CertificationComponentRenderTests : BunitTestBase
{
    [Test]
    public void CerfiicateComponent_WithCertificateParameters_RendersCorrectly()
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

        var cut = RenderComponent<CertificationComponent>(p => p.Add(p=>p.Item, certification));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(name));
           Assert.That(markup, Does.Contain(earnedOn.ToString("MMM d yyyy")));
           Assert.That(markup, Does.Contain(icon));
           Assert.That(markup, Does.Contain(link));
        });
    }
    
    [Test]
    public void CertificateComponent_WithoutLink_RendersCorrectly()
    {
        // Arrange
        var earnedOn = new DateOnly(2024,5,1);
        var name = "Azure Fundamentals";
        var issuedBy = "Microsoft";
        var icon = "azure-fundamentals.png";

        Certification certification = new()
        {
            EarnedOn = earnedOn,
            Name = name,
            IssuedBy = issuedBy,
            Icon = icon
        };

        var cut = RenderComponent<CertificationComponent>(p => p.Add(p=>p.Item, certification));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
            Assert.That(markup, Does.Contain(name));
            Assert.That(markup, Does.Contain(earnedOn.ToString("MMM d yyyy")));
            Assert.That(markup, Does.Contain(icon));
            Assert.That(markup, Does.Not.Contain("<a href="));
        });
    }
    [Test]
    public void CertificateComponent_WithoutCertificate_ReturnsEmpty()
    {
        var cut = RenderComponent<CertificationComponent>();
        var markup = cut.Markup;

        Assert.That(markup, Does.Contain(string.Empty));
    }
}