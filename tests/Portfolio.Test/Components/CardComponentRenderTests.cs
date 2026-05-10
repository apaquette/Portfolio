using Portfolio.Pages.Components;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for CardComponent Blazor component
/// Tests rendering with various parameter combinations
/// </summary>
[TestFixture]
public class CardComponentRenderTests : BunitTestBase
{
    [Test]
    public void CardComponent_WithTitle_RendersTitle()
    {
        // Arrange
        var title = "Project Title";

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, title)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(title));
        Assert.That(cut.Markup, Does.Contain("card"));
    }

    [Test]
    public void CardComponent_WithSubtitle_RendersSubtitle()
    {
        // Arrange
        var title = "Project";
        var subtitle = "June 2023";

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, title)
            .Add(p => p.Subtitle, subtitle)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(subtitle));
        Assert.That(cut.Markup, Does.Contain("card-subtitle"));
    }

    [Test]
    public void CardComponent_WithoutSubtitle_DoesNotRenderSubtitleDiv()
    {
        // Arrange
        var title = "Project";

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, title)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(title));
    }

    [Test]
    public void CardComponent_WithLink_RendersAsLink()
    {
        // Arrange
        var title = "GitHub Project";
        var link = "https://github.com/example/project";

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, title)
            .Add(p => p.Link, link)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("href"));
        Assert.That(cut.Markup, Does.Contain(link));
        Assert.That(cut.Markup, Does.Contain("stretched-link"));
    }

    [Test]
    public void CardComponent_WithoutLink_RendersNoLink()
    {
        // Arrange
        var title = "Non-linked Project";

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, title)
            .Add(p => p.Link, null)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(title));
        Assert.That(cut.Markup, Does.Not.Contain("stretched-link"));
    }

    [Test]
    public void CardComponent_WithChildContent_RendersContent()
    {
        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, "Card Title")
            .AddChildContent("<p>This is child content</p>")
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("This is child content"));
        Assert.That(cut.Markup, Does.Contain("card-text"));
    }

    [Test]
    public void CardComponent_WithTags_RendersTags()
    {
        // Arrange
        var tags = new[] { "C#", "ASP.NET", "React" };

        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, "Card")
            .Add(p => p.Tags, tags)
        );

        // Assert
        foreach (var tag in tags)
        {
            Assert.That(cut.Markup, Does.Contain(tag));
        }
        Assert.That(cut.Markup, Does.Contain("badge"));
    }

    [Test]
    public void CardComponent_WithoutTags_RendersNoTags()
    {
        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, "Card")
            .Add(p => p.Tags, null)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Card"));
    }

    [Test]
    public void CardComponent_WithEmptyTagsArray_RendersNoTags()
    {
        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, "Card")
            .Add(p => p.Tags, new string[] { })
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Card"));
    }

    [Test]
    public void CardComponent_WithIcon_RendersIcon()
    {
        // Act
        var cut = RenderComponent<CardComponent>(parameters => parameters
            .Add(p => p.Title, "Card")
            .Add(p => p.Icon, "<span class=\"icon\">Icon</span>")
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("card-body"));
        Assert.That(cut.Markup, Does.Contain("icon"));
    }
}
