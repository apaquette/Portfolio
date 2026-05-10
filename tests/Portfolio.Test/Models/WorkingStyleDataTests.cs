using Portfolio.Pages.AboutPage.Sections;

namespace Portfolio.Test.Models;

public class WorkingStyleDataTests
{
    [Test]
    public void WorkingStyleData_ShouldInitializeWithGivenValues()
    {
        string[] highlights = ["A", "Test", "Highlight"];
        var workingStyleData = new WorkingStyleData()
        {
            Title = "Test",
            Subtitle = "Test subtitle",
            Highlights = highlights
        };

        Assert.Multiple(() =>
        {
           Assert.That(workingStyleData.Title, Is.EqualTo("Test"));
           Assert.That(workingStyleData.Subtitle, Is.EqualTo("Test subtitle"));
           Assert.That(workingStyleData.Highlights, Is.EqualTo(highlights));
        });
    }
}