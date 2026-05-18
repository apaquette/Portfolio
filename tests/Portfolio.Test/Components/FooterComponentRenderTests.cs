using Portfolio.Layout;
using Portfolio.Layout.Components;

namespace Portfolio.Test.Components;

[TestFixture]
public class FooterRenderTests : BunitTestBase
{
    [Test]
    public void Footer_Renders_Correctly()
    {
        // Act
        var cut = RenderComponent<Footer>();
        var year = DateTime.Now.Year;

        // Assert
        Assert.That(cut.Markup, Does.Contain("Alex Paquette"));
        Assert.That(cut.Markup, Does.Contain("Senior Software Developer"));
        Assert.That(cut.Markup, Does.Contain($"{year}"));
        Assert.That(cut.Markup, Does.Contain("Code By Alex"));
        // ContactInfo is present
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