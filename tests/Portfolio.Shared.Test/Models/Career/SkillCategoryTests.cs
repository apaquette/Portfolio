using Models.Career;

namespace Portfolio.Shared.Test;

public class SkillCategoryTests
{
    [Test]
    public void Constructor_WithValidData_ShouldInitializeProperties()
    {
        var skillCategory = new SkillCategory()
        {
            Name = "Programming Languages",
            Skills = new SortedSet<string> { "C#", "JavaScript", "Python" }
        };

        Assert.That(skillCategory.Name, Is.EqualTo("Programming Languages"));
        Assert.That(skillCategory.Skills, Is.EqualTo(new SortedSet<string> { "C#", "JavaScript", "Python" }));
    }
}