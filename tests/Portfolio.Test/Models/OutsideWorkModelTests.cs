using Microsoft.AspNetCore.Components;
using Portfolio.Pages.AboutPage.Sections;

namespace Portfolio.Test.Models;

public class OutsideWorkModelTests
{
    [Test]
    public void OutsideWorkModel_ShouldInitializeWithGivenValues()
    {
        RenderFragment icon = builder =>
            {
                builder.OpenElement(1, "i");
                builder.AddAttribute(2, "class", "bi bi-terminal-fill fs-3");
                builder.CloseElement();
            };
        string[] tags = ["A", "Test", "Tag"];

        var outsideWorkModel = new OutsideWorkModel()
        {
            Title = "Test",
            Description = "Test description",
            Tags = tags,
            Icon = icon
        };

        Assert.Multiple(() =>
        {
            Assert.That(outsideWorkModel.Title, Is.EqualTo("Test"));
            Assert.That(outsideWorkModel.Description, Is.EqualTo("Test description"));
            Assert.That(outsideWorkModel.Tags, Is.EqualTo(tags));
            Assert.That(outsideWorkModel.Icon, Is.EqualTo(icon));
        });
    }
}