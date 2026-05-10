using Portfolio.Pages.Components;
using Models;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for ProjectComponent Blazor component
/// Tests rendering with various Project parameters
/// </summary>
[TestFixture]
public class ProjectComponentRendererTests : BunitTestBase
{
    private Project? testProject;

    [SetUp]
    public new void Setup()
    {
        base.Setup();
        
        // Create a valid test project
        testProject = new Project(new DateOnly(2023, 6, 15))
        {
            Title = "E-Commerce Platform",
            Description = "Built a scalable e-commerce platform",
            Link = "https://github.com/example/ecommerce"
        };
        testProject.TechStack.Add("C#");
        testProject.TechStack.Add("ASP.NET");
        testProject.TechStack.Add("React");
    }

    [Test]
    public void ProjectComponent_WithValidProject_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, testProject)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("E-Commerce Platform"));
        Assert.That(cut.Markup, Does.Contain("Jun 15 2023"));
    }

    [Test]
    public void ProjectComponent_WithNullProject_RendersNothing()
    {
        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, null)
        );

        // Assert
        Assert.That(cut.Markup, Is.Empty.Or.EqualTo(""));
    }

    [Test]
    public void ProjectComponent_RendersCardComponent()
    {
        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, testProject)
        );

        // Assert - CardComponent should be rendered as part of ProjectComponent
        Assert.That(cut.Markup, Does.Contain("card"));
    }

    [Test]
    public void ProjectComponent_RendersTechStack()
    {
        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, testProject)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("C#"));
        Assert.That(cut.Markup, Does.Contain("ASP.NET"));
        Assert.That(cut.Markup, Does.Contain("React"));
    }

    [Test]
    public void ProjectComponent_WithProjectWithoutLink_RendersWithoutLink()
    {
        // Arrange
        testProject!.Link = null;

        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, testProject)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("E-Commerce Platform"));
    }

    [Test]
    public void ProjectComponent_WithProjectWithDescription_RendersDescription()
    {
        // Act
        var cut = RenderComponent<ProjectComponent>(parameters => parameters
            .Add(p => p.Item, testProject)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain("Built a scalable e-commerce platform"));
    }
}
