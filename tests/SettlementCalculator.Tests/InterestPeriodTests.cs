using Xunit;

namespace SettlementCalculator.Tests;

public class InterestPeriodTests
{
    [Fact]
    public void Constructor_WithValidDates_CreatesInstance()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);
        var rate = 0.05m;

        // Act
        var period = new InterestPeriod(startDate, endDate, rate);

        // Assert
        Assert.Equal(startDate, period.StartDate);
        Assert.Equal(endDate, period.EndDate);
        Assert.Equal(rate, period.AnnualRate);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenEndDateBeforeStartDate()
    {
        // Arrange
        var startDate = new DateTime(2024, 12, 31);
        var endDate = new DateTime(2024, 1, 1);
        var rate = 0.05m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new InterestPeriod(startDate, endDate, rate));
    }

    [Fact]
    public void Constructor_ThrowsException_WhenEndDateEqualsStartDate()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);
        var rate = 0.05m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new InterestPeriod(date, date, rate));
    }

    [Fact]
    public void DurationDays_CalculatesCorrectNumberOfDays()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 11);
        var period = new InterestPeriod(startDate, endDate, 0.05m);

        // Act
        var duration = period.DurationDays;

        // Assert
        Assert.Equal(10, duration);
    }

    [Fact]
    public void DurationDays_HandlesLeapYear()
    {
        // Arrange
        var startDate = new DateTime(2024, 2, 1);
        var endDate = new DateTime(2024, 3, 1);
        var period = new InterestPeriod(startDate, endDate, 0.05m);

        // Act
        var duration = period.DurationDays;

        // Assert
        Assert.Equal(29, duration); // 2024 is a leap year
    }
}
