using Xunit;

namespace SettlementCalculator.Tests;

public class AccountSnapshotTests
{
    [Fact]
    public void Constructor_CreatesInstance()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var balance = 5000m;
        var cumulativeInterest = 123.45m;

        // Act
        var snapshot = new AccountSnapshot(date, balance, cumulativeInterest);

        // Assert
        Assert.Equal(date, snapshot.Date);
        Assert.Equal(balance, snapshot.Balance);
        Assert.Equal(cumulativeInterest, snapshot.CumulativeInterest);
    }

    [Fact]
    public void Constructor_AllowsNegativeBalance()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var balance = -100m;
        var cumulativeInterest = 50m;

        // Act
        var snapshot = new AccountSnapshot(date, balance, cumulativeInterest);

        // Assert
        Assert.Equal(balance, snapshot.Balance);
    }
}
