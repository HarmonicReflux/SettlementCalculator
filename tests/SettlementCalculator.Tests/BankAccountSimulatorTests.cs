using Xunit;

namespace SettlementCalculator.Tests;

public class BankAccountSimulatorTests
{
    [Fact]
    public void Simulate_WithNoInterestOrPayments_ReturnsConstantBalance()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 10);
        var initialBalance = 1000m;

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(9, snapshots.Count); // 9 days
        Assert.All(snapshots, s => Assert.Equal(1000m, s.Balance));
        Assert.All(snapshots, s => Assert.Equal(0m, s.CumulativeInterest));
    }

    [Fact]
    public void Simulate_WithSingleInterestPeriod_CalculatesCompoundInterest()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 11); // 10 days
        var initialBalance = 1000m;
        var annualRate = 0.05m; // 5% annual rate

        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, annualRate));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(10, snapshots.Count);
        
        // Check that balance increases each day
        for (int i = 1; i < snapshots.Count; i++)
        {
            Assert.True(snapshots[i].Balance > snapshots[i - 1].Balance, 
                $"Balance should increase from day {i - 1} to day {i}");
        }

        // Verify cumulative interest is positive
        Assert.True(snapshots.Last().CumulativeInterest > 0);
        
        // Approximate check: 10 days at 5% annual = about 1.37 interest
        // Daily rate = 0.05/365 ≈ 0.000137
        Assert.True(snapshots.Last().CumulativeInterest > 1.3m && 
                    snapshots.Last().CumulativeInterest < 1.5m);
    }

    [Fact]
    public void Simulate_WithMultipleInterestPeriods_AppliesCorrectRates()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var midDate = new DateTime(2024, 1, 6);
        var endDate = new DateTime(2024, 1, 11);
        var initialBalance = 1000m;

        // First period: 5% for 5 days
        simulator.AddInterestPeriod(new InterestPeriod(startDate, midDate, 0.05m));
        // Second period: 10% for 5 days
        simulator.AddInterestPeriod(new InterestPeriod(midDate, endDate, 0.10m));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(10, snapshots.Count);
        
        // Interest should be higher in second period (higher rate)
        var interestFirstPeriod = snapshots[4].CumulativeInterest;
        var interestSecondPeriod = snapshots[9].CumulativeInterest - snapshots[4].CumulativeInterest;
        
        Assert.True(interestSecondPeriod > interestFirstPeriod, 
            "Second period should accumulate more interest due to higher rate and compounding");
    }

    [Fact]
    public void Simulate_WithPayments_AdjustsBalanceCorrectly()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 11);
        var initialBalance = 1000m;

        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.05m));
        
        // Add deposit on day 3
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 3), 500m, "Deposit"));
        // Add withdrawal on day 7
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 7), -200m, "Withdrawal"));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(10, snapshots.Count);
        
        // Check balances around payment dates
        var balanceBeforeDeposit = snapshots[1].Balance; // Jan 2
        var balanceAfterDeposit = snapshots[2].Balance; // Jan 3
        
        // The balance after deposit should be roughly 500 more (plus interest)
        Assert.True(balanceAfterDeposit - balanceBeforeDeposit > 500m);
        
        var balanceBeforeWithdrawal = snapshots[5].Balance; // Jan 6
        var balanceAfterWithdrawal = snapshots[6].Balance; // Jan 7
        
        // The balance after withdrawal should be roughly 200 less (but interest still applies)
        Assert.True(balanceBeforeWithdrawal - balanceAfterWithdrawal > 199m);
    }

    [Fact]
    public void Simulate_WithMultiplePaymentsOnSameDay_ProcessesAllPayments()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 5);
        var initialBalance = 1000m;

        var paymentDate = new DateTime(2024, 1, 3);
        simulator.AddPayment(new Payment(paymentDate, 100m, "Payment 1"));
        simulator.AddPayment(new Payment(paymentDate, 200m, "Payment 2"));
        simulator.AddPayment(new Payment(paymentDate, 300m, "Payment 3"));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        var snapshotOnPaymentDay = snapshots.FirstOrDefault(s => s.Date == paymentDate);
        Assert.NotNull(snapshotOnPaymentDay);
        
        // Balance should reflect all three payments (600 total)
        Assert.True(snapshotOnPaymentDay.Balance >= 1600m);
    }

    [Fact]
    public void Simulate_WithCompoundingEffect_ShowsIncreasingDailyInterest()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 31);
        var initialBalance = 10000m;
        var annualRate = 0.05m;

        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate.AddDays(1), annualRate));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        // Due to compounding, daily interest should increase slightly each day
        var interest1 = snapshots[0].Balance - initialBalance;
        var interest10 = snapshots[9].Balance - snapshots[8].Balance;
        var interest20 = snapshots[19].Balance - snapshots[18].Balance;
        
        Assert.True(interest10 > interest1, "Interest on day 10 should be higher than day 1 due to compounding");
        Assert.True(interest20 > interest10, "Interest on day 20 should be higher than day 10 due to compounding");
    }

    [Fact]
    public void CalculateFinalBalance_ReturnsCorrectFinalAmount()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 11);
        var initialBalance = 1000m;

        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.05m));
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 5), 500m));

        // Act
        var finalBalance = simulator.CalculateFinalBalance(startDate, endDate, initialBalance);
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(snapshots.Last().Balance, finalBalance);
    }

    [Fact]
    public void CalculateTotalInterest_ReturnsCorrectAmount()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 1, 11);
        var initialBalance = 1000m;

        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.05m));

        // Act
        var totalInterest = simulator.CalculateTotalInterest(startDate, endDate, initialBalance);
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(snapshots.Last().CumulativeInterest, totalInterest);
    }

    [Fact]
    public void Simulate_WithPaymentDependentOnCompoundedInterest_CalculatesCorrectly()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 2, 1);
        var initialBalance = 10000m;

        // 12% annual rate for high compounding effect
        simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.12m));
        
        // Multiple payments throughout the period
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 5), 1000m));
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 10), 2000m));
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 15), -1500m));
        simulator.AddPayment(new Payment(new DateTime(2024, 1, 20), 3000m));

        // Act
        var snapshots = simulator.Simulate(startDate, endDate, initialBalance);

        // Assert
        Assert.Equal(31, snapshots.Count);
        
        // Verify interest is compounding on the changing balance
        var finalBalance = snapshots.Last().Balance;
        var totalPayments = 1000m + 2000m - 1500m + 3000m; // = 4500
        
        // Final balance should be: initial + payments + compounded interest
        var expectedMinimum = initialBalance + totalPayments;
        Assert.True(finalBalance > expectedMinimum, 
            "Final balance should include compounded interest on top of principal and payments");
    }

    [Fact]
    public void Simulate_ThrowsException_WhenEndDateBeforeStartDate()
    {
        // Arrange
        var simulator = new BankAccountSimulator();
        var startDate = new DateTime(2024, 1, 10);
        var endDate = new DateTime(2024, 1, 5);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => simulator.Simulate(startDate, endDate));
    }
}
