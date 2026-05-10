using Models;

namespace Portfolio.Shared.Test;

public class NavItemTests
{
    [Test]
    public void NavItem_ShouldInitializeWithGivenValues()
    {
        var navItem = new NavItem() { Label = "Home", Route = "/home" };

        Assert.Multiple(() =>
        {
            Assert.That(navItem.Label, Is.EqualTo("Home"));
            Assert.That(navItem.Route, Is.EqualTo("/home"));
        });

    }
}