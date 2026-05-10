using Portfolio.Pages.Components;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for SectionComponent Blazor component
/// Tests rendering with various parameter combinations
/// </summary>
[TestFixture]
public class SectionComponentRendererTests : BunitTestBase
{
    [Test]
    public void SectionComponent_WithTitle_RendersTitle()
    {
        // Arrange
        var title = "Experience";

        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Title, title)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(title));
        Assert.That(cut.Markup, Does.Contain("section-title"));
    }

    [Test]
    public void SectionComponent_WithoutTitle_RendersNoTitle()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Title, null)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("section"));
    }

    [Test]
    public void SectionComponent_WithId_RendersIdAttribute()
    {
        // Arrange
        var id = "experience-section";

        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Id, id)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain($"id=\"{id}\""));
    }

    [Test]
    public void SectionComponent_WithCentered_AppliesCenteredClass()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Centered, true)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("section-centered"));
        Assert.That(cut.Markup, Does.Contain("home-section"));
    }

    [Test]
    public void SectionComponent_NotCentered_DoesNotApplyCenteredClass()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Centered, false)
        );

        // Assert
        Assert.That(cut.Markup, Does.Not.Contain("section-centered"));
    }

    [Test]
    public void SectionComponent_WithChildContent_RendersContent()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Title, "Section")
            .AddChildContent("<div class=\"content\">Section content here</div>")
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Section content here"));
    }

    [Test]
    public void SectionComponent_WithTitleAndContent_RendersBoth()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Title, "My Section")
            .AddChildContent("<p>This is the section content</p>")
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("My Section"));
        Assert.That(cut.Markup, Does.Contain("This is the section content"));
    }

    [Test]
    public void SectionComponent_WithAllParameters_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<SectionComponent>(parameters => parameters
            .Add(p => p.Title, "Complete Section")
            .Add(p => p.Id, "complete")
            .Add(p => p.Centered, true)
            .AddChildContent("<span>Full content</span>")
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Complete Section"));
        Assert.That(cut.Markup, Does.Contain("id=\"complete\""));
        Assert.That(cut.Markup, Does.Contain("section-centered"));
        Assert.That(cut.Markup, Does.Contain("Full content"));
    }
}
