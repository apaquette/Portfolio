using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Layout.Components;

namespace Portfolio.Test.Components;

public class NavbarComponentRenderTest : BunitTestBase
{
    [Test]
    public void Navbar_RendersNavItems()
    {
        // Act
        var cut = RenderComponent<Navbar>();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(cut.Markup, Does.Contain("Home"));
            Assert.That(cut.Markup, Does.Contain("Experience"));
            Assert.That(cut.Markup, Does.Contain("About"));
            Assert.That(cut.Markup, Does.Contain("href=\"/\""));
            Assert.That(cut.Markup, Does.Contain("href=\"/experience\""));
            Assert.That(cut.Markup, Does.Contain("href=\"/about\""));

        });
    }

    [Test]
    public void Navbar_ActiveNavItem_HasActiveClass()
    {
        // Arrange
        var cut = RenderComponent<Navbar>();

        // Act
        var homeLink = cut.Find("a[href='/']");
        var experienceLink = cut.Find("a[href='/experience']");
        var aboutLink = cut.Find("a[href='/about']");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(homeLink.ClassList, Does.Contain("active"));
            Assert.That(experienceLink.ClassList, Does.Not.Contain("active"));
            Assert.That(aboutLink.ClassList, Does.Not.Contain("active"));
        });
    }

    [Test]
    public void Navbar_Navigation_UpdatesActiveLinks()
    {
        // Arrange
        var nav = Context?.Services.GetRequiredService<NavigationManager>();
        var cut = RenderComponent<Navbar>();

        // Discover all nav links automatically
        var links = cut.FindAll("a[href]")
            .Select(link => link.GetAttribute("href"))
            .Where(href => !string.IsNullOrWhiteSpace(href))
            .Distinct()
            .ToList();

        Assert.That(links, Is.Not.Empty);

        // Act + Assert
        foreach (var route in links)
        {
            nav?.NavigateTo(route!);

            var currentLinks = cut.FindAll("a[href]");


            Assert.Multiple(() =>
            {
                foreach (var link in currentLinks)
                {
                    var href = link.GetAttribute("href");
                    var isActive = link.ClassList.Contains("active");

                    Assert.That(
                        isActive,
                        Is.EqualTo(href == route),
                        $"Route '{route}' incorrectly set active state for '{href}'");
                }
            });
        }
    }
}