using Models.Career;
using Validation.Dates;

namespace Portfolio.Shared.Test;

public class AwardTests
{
    [Test]
    public void Constructor_WithValidData_ShouldInitializeProperties()
    {
        var award = new Award(new DateOnly(2023, 5, 1))
        {
            Title = "Best Developer",
            Issuer = "Tech Conference",
            Description = "Awarded for outstanding development skills."
        };

        Assert.That(award.Title, Is.EqualTo("Best Developer"));
        Assert.That(award.Issuer, Is.EqualTo("Tech Conference"));
        Assert.That(award.Date, Is.EqualTo(new DateOnly(2023, 5, 1)));
        Assert.That(award.Description, Is.EqualTo("Awarded for outstanding development skills."));
    }

    [Test]
    public void Constructor_WithValidMissingAttributes_ShouldAllowEmpty()
    {
        var award = new Award(new DateOnly(2023, 5, 1));

        Assert.That(award.Title, Is.Empty);
        Assert.That(award.Issuer, Is.Empty);
        Assert.That(award.Date, Is.EqualTo(new DateOnly(2023, 5, 1)));
        Assert.That(award.Description, Is.Empty);
    }

    [Test]
    public void Constructor_WithMisingDate_ShouldThrowMissingDateException()
    {
        Assert.Throws<MissingDateException>(() => { new Award(default); });
    }

    [Test]
    public void AwardIsMoreRecent_ReturnsBeforeOther()
    {
        Award newer = new(new DateOnly(2023, 6, 1));
        Award older = new(new DateOnly(2020, 6, 1));

        Assert.That(newer.CompareTo(older), Is.LessThan(0)); // newer first
    }

    [Test]
    public void AwardIsLessRecent_ReturnsAfterOther()
    {
        Award newer = new(new DateOnly(2023, 6, 1));
        Award older = new(new DateOnly(2020, 6, 1));

        Assert.That(older.CompareTo(newer), Is.GreaterThan(0)); // newer first
    }

    [Test]
    public void AwardDatesAreEqual_ReturnsZero()
    {
        Award a1 = new(new DateOnly(2022, 6, 1));
        Award a2 = new(new DateOnly(2022, 6, 1));

        Assert.That(a1.CompareTo(a2), Is.EqualTo(0));
    }
    [Test]
    public void CompareTo_WhenOtherIsNull_ReturnsOne()
    {
        // Arrange
        Award? other = null;
        Award award = new(new(2022, 6, 1))
        {
            Title = "Best Developer",
            Issuer = "Tech Conference",
            Description = "Awarded for outstanding development skills."
        };

        // Act
        int result = award.CompareTo(other);

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void GraduationDate_VeryOldDate_ShouldBeSortedCorrectly()
    {
        // Arrange
        Award ancient = new(new DateOnly(1980, 6, 1));
        Award modern = new(new DateOnly(2022, 6, 1));

        // Act
        var result = modern.CompareTo(ancient);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Modern comes first
    }

    [Test]
    public void GraduationDate_FarFutureDate_ShouldBeSortedCorrectly()
    {
        // Arrange
        Award current = new(new DateOnly(2022, 6, 1));
        Award future = new(new DateOnly(2100, 6, 1));

        // Act
        var result = future.CompareTo(current);

        // Assert
        Assert.That(result, Is.LessThan(0)); // Future comes first
    }

    [Test]
    public void CompareTo_MultipleComparisons_ShouldMaintainOrder()
    {
        // Arrange
        var Awards = new[]
        {
            new Award(new DateOnly(2018, 6, 1)),
            new Award(new DateOnly(2020, 6, 1)),
            new Award(new DateOnly(2019, 6, 1))
        };

        // Act
        Array.Sort(Awards);

        // Assert - Most recent first
        Assert.That(Awards[0].Date, Is.EqualTo(new DateOnly(2020, 6, 1)));
        Assert.That(Awards[1].Date, Is.EqualTo(new DateOnly(2019, 6, 1)));
        Assert.That(Awards[2].Date, Is.EqualTo(new DateOnly(2018, 6, 1)));
    }

    [Test]
    public void Award_WithLeapYearDate_ShouldBeValid()
    {
        // Arrange & Act
        Award award = new(new DateOnly(2020, 2, 29));

        // Assert
        Assert.That(award.Date, Is.EqualTo(new DateOnly(2020, 2, 29)));
    }
}