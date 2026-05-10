using Portfolio.Pages.Components;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for SkillComponent Blazor component
/// Tests rendering output with various parameter values
/// </summary>
[TestFixture]
public class SkillComponentRendererTests : BunitTestBase
{
    [Test]
    public void SkillComponent_WithValidSkill_RendersCorrectly()
    {
        // Arrange
        var skill = "C#";

        // Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, skill)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(skill));
        Assert.That(cut.Markup, Does.Contain("badge"));
    }

    [Test]
    public void SkillComponent_WithEmptySkill_RendersNothing()
    {
        // Arrange
        var skill = string.Empty;

        // Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, skill)
        );

        // Assert
        Assert.That(cut.Markup, Is.Empty.Or.EqualTo(""));
    }

    [Test]
    public void SkillComponent_WithNullSkill_RendersNothing()
    {
        // Arrange & Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, null)
        );

        // Assert
        Assert.That(cut.Markup, Is.Empty.Or.EqualTo(""));
    }

    [Test]
    [TestCase("Python")]
    [TestCase("JavaScript")]
    [TestCase("ASP.NET")]
    [TestCase("Blazor")]
    public void SkillComponent_WithVariousSkills_RendersCorrectFormat(string skill)
    {
        // Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, skill)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(skill));
        Assert.That(cut.Markup, Does.Contain("badge"));
        Assert.That(cut.Markup, Does.Contain("rounded-pill"));
    }

    [Test]
    public void SkillComponent_WithLongSkillName_RendersCorrectly()
    {
        // Arrange
        var longSkill = "Enterprise Architecture Pattern Recognition";

        // Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, longSkill)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(longSkill));
        Assert.That(cut.Markup, Does.Contain("badge"));
    }

    [Test]
    public void SkillComponent_WithSpecialCharacters_RendersCorrectly()
    {
        // Arrange
        var skill = "C++";

        // Act
        var cut = RenderComponent<SkillComponent>(parameters => parameters
            .Add(p => p.Skill, skill)
        );

        // Assert
        Assert.That(cut.Markup, Does.Contain(skill));
    }
}
