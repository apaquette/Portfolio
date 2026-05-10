using Microsoft.AspNetCore.Components;
using Portfolio.Pages.AboutPage.Sections;

namespace Portfolio.Test.Models;

public class OutsideWorkDataTests
{
    [Test]
    public void OutsideWorkData_ShouldInitializeWithGivenValues()
    {
        RenderFragment icon = builder =>
            {
                builder.OpenElement(1, "i");
                builder.AddAttribute(2, "class", "bi bi-terminal-fill fs-3");
                builder.CloseElement();
            };
        string[] tags = ["A", "Test", "Tag"];

        var outsideWorkData = new OutsideWorkData()
        {
            Title = "Test",
            Description = "Test description",
            Tags = tags,
            Icon = icon
        };

        Assert.Multiple(() =>
        {
            Assert.That(outsideWorkData.Title, Is.EqualTo("Test"));
            Assert.That(outsideWorkData.Description, Is.EqualTo("Test description"));
            Assert.That(outsideWorkData.Tags, Is.EqualTo(tags));
            Assert.That(outsideWorkData.Icon, Is.EqualTo(icon));
        });
    }
}