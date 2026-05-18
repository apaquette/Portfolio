using Models;
using Portfolio.Pages.ExperiencePage.Components;

namespace Portfolio.Test.Components;

[TestFixture]
public class EducationComponentRenderTests : BunitTestBase
{
    [Test]
    public void EducationComponent_WithEducationParameters_RendersCorrectly()
    {
        // Arrange
        var graduationDate = new DateOnly(2024,5,1);
        var diploma = "Test Credentials";
        var institution = "Test College";
        var logo = "college-logo.png";
        var website = "https://some-college-website.com";

        // Act
        Degree degree = new(graduationDate)
        {
            Diploma = diploma,
            Institution = institution,
            Logo = logo,
            Website = website
        };

        var cut = RenderComponent<EducationComponent>(p => p.Add(p=>p.Item, degree));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(graduationDate.ToString("MMM yyy")));
           Assert.That(markup, Does.Contain(diploma));
           Assert.That(markup, Does.Contain(institution));
           Assert.That(markup, Does.Contain(logo));
           Assert.That(markup, Does.Contain(website));
        });
    }
}