using Models;

namespace Portfolio.Shared.Test;

public class SectionDefinitionTests
{
    public class MockComponent { }
    public class MockDataItemComponent { }
    [Test]
    public void SectionDefinition_ShouldInitializeWithGivenValues()
    {
        var sectionDefinition = new SectionDefinition(
                Title: "About Me",
                ComponentType: typeof(MockComponent),
                JsonUrl: "https://example.com/about.json",
                DataItemComponentType: typeof(MockDataItemComponent),
                Class: "about-section",
                Style: "background-color: #f0f0f0;",
                Centered: true
        );

        Assert.Multiple(() =>
        {
            Assert.That(sectionDefinition.Title, Is.EqualTo("About Me"));
            Assert.That(sectionDefinition.ComponentType, Is.EqualTo(typeof(MockComponent)));
            Assert.That(sectionDefinition.JsonUrl, Is.EqualTo("https://example.com/about.json"));
            Assert.That(sectionDefinition.DataItemComponentType, Is.EqualTo(typeof(MockDataItemComponent)));
            Assert.That(sectionDefinition.Class, Is.EqualTo("about-section"));
            Assert.That(sectionDefinition.Style, Is.EqualTo("background-color: #f0f0f0;"));
            Assert.That(sectionDefinition.Centered, Is.True);
        });

    }
}