using Models.Career;
using Portfolio.UI.Models;

namespace Portfolio.Test.Components;

[TestFixture]
public class AwardComponentRenderTests : BunitTestBase
{
    [Test]
    public void AwardComponent_WithAwardParameters_RendersCorrectly()
    {
        // Arrange
        var date = new DateOnly(2024,5,1);
        var title = "Test Award";
        var description = "Award Description";

        // Act
        Award award = new(date)
        {
            Title = title,
            Description = description
        };

        var cut = RenderComponent<AwardComponent>(p => p.Add(p=>p.Item, award));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(date.ToString("MMM d yyyy")));
           Assert.That(markup, Does.Contain(title));
           Assert.That(markup, Does.Contain(description));
        });
    }
}