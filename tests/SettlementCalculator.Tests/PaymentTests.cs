using Xunit;

namespace SettlementCalculator.Tests;

public class PaymentTests
{
    [Fact]
    public void Constructor_WithAllParameters_CreatesInstance()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var amount = 1000m;
        var description = "Monthly salary";

        // Act
        var payment = new Payment(date, amount, description);

        // Assert
        Assert.Equal(date, payment.Date);
        Assert.Equal(amount, payment.Amount);
        Assert.Equal(description, payment.Description);
    }

    [Fact]
    public void Constructor_WithoutDescription_CreatesInstance()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var amount = 1000m;

        // Act
        var payment = new Payment(date, amount);

        // Assert
        Assert.Equal(date, payment.Date);
        Assert.Equal(amount, payment.Amount);
        Assert.Null(payment.Description);
    }

    [Fact]
    public void Payment_AllowsNegativeAmount()
    {
        // Arrange
        var date = new DateTime(2024, 1, 15);
        var amount = -500m;

        // Act
        var payment = new Payment(date, amount);

        // Assert
        Assert.Equal(amount, payment.Amount);
    }
}
