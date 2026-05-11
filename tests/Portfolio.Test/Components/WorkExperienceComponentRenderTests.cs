using Models;
using Portfolio.Pages.ExperiencePage.Components;

namespace Portfolio.Test.Components;

[TestFixture]
public class WorkExperienceComponentRenderTests : BunitTestBase
{
    [Test]
    public void WorkExperienceComponent_WithCertificateParameters_RendersCorrectly()
    {
        string title = "Dummy Title";
        string company = "Dummy Company";
        DateTime jobStart = new(2026,01,01);
        DateTime jobEnd = new(2027,01,01);
        string description = "Dummy description";
        SortedSet<string> skills = ["A", "C", "D"];
        string location = "Dummy location";
        string employerSite = "www.dummysite.com";


        Experience experience = new(jobStart)
        {
            Title = title,
            Company = company,
            JobEnd = jobEnd,
            Description = description,
            Skills = skills,
            Location = location,
            EmployerSite = employerSite
        };

        var cut = RenderComponent<WorkExperienceComponent>(p => p.Add(p=>p.Item, experience));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(title));
           Assert.That(markup, Does.Contain(company));
           Assert.That(markup, Does.Contain(description));
           Assert.That(markup, Does.Contain(jobStart.ToString("MMM yyyy")));
           Assert.That(markup, Does.Contain(jobEnd.ToString("MMM yyyy")));
           Assert.That(markup, Does.Contain(experience.Duration()));
           Assert.That(markup, Does.Contain(location));
           Assert.That(markup, Does.Contain(employerSite));
           Assert.That(markup, Does.Contain("bi bi-box-arrow-up-right"));
           foreach(var skill in skills)
            {
                Assert.That(markup, Does.Contain(skill));
            }
        });
    }
    [Test]
    public void WorkExperienceComponent_WithoutJobEnd_RendersWithPresent()
    {
        string title = "Dummy Title";
        string company = "Dummy Company";
        DateTime jobStart = new(2026,01,01);
        string description = "Dummy description";
        SortedSet<string> skills = ["A", "C", "D"];
        string location = "Dummy location";
        string employerSite = "www.dummysite.com";


        Experience experience = new(jobStart)
        {
            Title = title,
            Company = company,
            Description = description,
            Skills = skills,
            Location = location,
            EmployerSite = employerSite
        };

        var cut = RenderComponent<WorkExperienceComponent>(p => p.Add(p=>p.Item, experience));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(title));
           Assert.That(markup, Does.Contain(company));
           Assert.That(markup, Does.Contain(description));
           Assert.That(markup, Does.Contain(jobStart.ToString("MMM yyyy")));
           Assert.That(markup, Does.Contain("Present"));
           Assert.That(markup, Does.Contain(experience.Duration()));
           Assert.That(markup, Does.Contain(location));
           Assert.That(markup, Does.Contain(employerSite));
           Assert.That(markup, Does.Contain("bi bi-box-arrow-up-right"));
           foreach(var skill in skills)
            {
                Assert.That(markup, Does.Contain(skill));
            }
        });
    }

    [Test]
    public void WorkExperienceComponent_WithoutEmployerSite_RendersWithPresent()
    {
        string title = "Dummy Title";
        string company = "Dummy Company";
        DateTime jobStart = new(2026,01,01);
        DateTime jobEnd = new(2027,01,01);
        string description = "Dummy description";
        SortedSet<string> skills = ["A", "C", "D"];
        string location = "Dummy location";


        Experience experience = new(jobStart)
        {
            Title = title,
            Company = company,
            JobEnd = jobEnd,
            Description = description,
            Skills = skills,
            Location = location
        };

        var cut = RenderComponent<WorkExperienceComponent>(p => p.Add(p=>p.Item, experience));
        var markup = cut.Markup;

        Assert.Multiple(() =>
        {
           Assert.That(markup, Does.Contain(title));
           Assert.That(markup, Does.Contain(company));
           Assert.That(markup, Does.Contain(description));
           Assert.That(markup, Does.Contain(jobStart.ToString("MMM yyyy")));
           Assert.That(markup, Does.Contain(jobEnd.ToString("MMM yyyy")));
           Assert.That(markup, Does.Contain(experience.Duration()));
           Assert.That(markup, Does.Contain(location));
           Assert.That(markup, Does.Not.Contain("href"));
           Assert.That(markup, Does.Not.Contain("bi bi-box-arrow-up-right"));
           foreach(var skill in skills)
            {
                Assert.That(markup, Does.Contain(skill));
            }
        });
    }
}