using Bunit;
using Portfolio.Layout.Components;

namespace Portfolio.Test.Components;

/// <summary>
/// bUnit tests for ContactInfo Blazor component
/// Tests rendering with various parameter combinations
/// </summary>
[TestFixture]
public class ContactInfoRenderTests : BunitTestBase
{
    [Test]
    public void ContactInfo_WithValidParameters_RendersCorrectly()
    {
        // Arrange
        Context?.JSInterop.SetupVoid("registerToolTips");
        var cut = RenderComponent<ContactInfo>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(cut.Markup, Does.Contain("mailto:alexandre.d.paquette@gmail.com"));
            Assert.That(cut.Markup, Does.Contain("envelope-fill"));
            Assert.That(cut.Markup, Does.Contain("Email"));
            Assert.That(cut.Markup, Does.Contain("https://www.linkedin.com/in/apaquette0/"));
            Assert.That(cut.Markup, Does.Contain("linkedin"));
            Assert.That(cut.Markup, Does.Contain("LinkedIn"));
            Assert.That(cut.Markup, Does.Contain("https://github.com/apaquette"));
            Assert.That(cut.Markup, Does.Contain("github"));
            Assert.That(cut.Markup, Does.Contain("GitHub"));
        });
    }
}