using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Portfolio.Pages.Home.Components;

namespace Portfolio.Test.Components;

[TestFixture]
public class CredibilityComponentRenderTests : BunitTestBase
{
    [Test]
    public void CredibilityComponent_WithParameters_RendersCorrectly()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(1, "div");
            builder.AddContent(2, "dummy content");
            builder.CloseElement();
        };
        string text = "Dummy text";

        var cut = RenderComponent<CredibilityComponent>(parameters => parameters
            .Add(p => p.Text, text)
            .AddChildContent(childContent)
        );

        var markup = cut.Markup;

        Assert.That(markup, Does.Contain("dummy content"));
        Assert.That(markup, Does.Contain(text));
    }
}