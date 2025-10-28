namespace SettlementCalculator;

/// <summary>
/// Simulates the evolution of a bank account with time-varying interest rates and multiple payment streams.
/// Interest is compounded daily and calculated based on the current balance.
/// </summary>
public class BankAccountSimulator
{
    private readonly List<InterestPeriod> _interestPeriods;
    private readonly List<Payment> _payments;

    /// <summary>
    /// Initializes a new instance of the BankAccountSimulator.
    /// </summary>
    public BankAccountSimulator()
    {
        _interestPeriods = new List<InterestPeriod>();
        _payments = new List<Payment>();
    }

    /// <summary>
    /// Adds an interest period to the simulation.
    /// </summary>
    public void AddInterestPeriod(InterestPeriod period)
    {
        _interestPeriods.Add(period);
    }

    /// <summary>
    /// Adds a payment to the simulation.
    /// </summary>
    public void AddPayment(Payment payment)
    {
        _payments.Add(payment);
    }

    /// <summary>
    /// Simulates the account evolution from startDate to endDate with the given initial balance.
    /// Returns a list of daily snapshots showing the account balance and cumulative interest.
    /// </summary>
    /// <param name="startDate">The start date of the simulation.</param>
    /// <param name="endDate">The end date of the simulation.</param>
    /// <param name="initialBalance">The initial balance of the account.</param>
    /// <returns>A list of account snapshots for each day in the simulation period.</returns>
    public List<AccountSnapshot> Simulate(DateTime startDate, DateTime endDate, decimal initialBalance = 0)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be after start date.");
        }

        // Sort interest periods and payments
        var sortedPeriods = _interestPeriods.OrderBy(p => p.StartDate).ToList();
        var sortedPayments = _payments.OrderBy(p => p.Date).ToList();

        var snapshots = new List<AccountSnapshot>();
        decimal currentBalance = initialBalance;
        decimal cumulativeInterest = 0;

        int paymentIndex = 0;
        DateTime currentDate = startDate;

        while (currentDate < endDate)
        {
            // Process all payments for current date
            while (paymentIndex < sortedPayments.Count && sortedPayments[paymentIndex].Date.Date == currentDate.Date)
            {
                currentBalance += sortedPayments[paymentIndex].Amount;
                paymentIndex++;
            }

            // Calculate and apply interest for the day
            decimal dailyInterest = CalculateDailyInterest(currentDate, currentBalance, sortedPeriods);
            currentBalance += dailyInterest;
            cumulativeInterest += dailyInterest;

            // Create snapshot for end of day
            snapshots.Add(new AccountSnapshot(currentDate, currentBalance, cumulativeInterest));

            // Move to next day
            currentDate = currentDate.AddDays(1);
        }

        return snapshots;
    }

    /// <summary>
    /// Calculates the interest for a single day based on the balance and applicable interest period.
    /// Uses the annual rate divided by 365 for daily interest calculation.
    /// </summary>
    private decimal CalculateDailyInterest(DateTime date, decimal balance, List<InterestPeriod> periods)
    {
        // Find the applicable interest period for this date
        var applicablePeriod = periods.FirstOrDefault(p => date >= p.StartDate && date < p.EndDate);

        if (applicablePeriod == null)
        {
            return 0; // No interest if no period applies
        }

        // Calculate daily interest: balance * (annual rate / 365)
        decimal dailyRate = applicablePeriod.AnnualRate / 365m;
        return balance * dailyRate;
    }

    /// <summary>
    /// Calculates the final balance at the end date without generating all snapshots.
    /// More efficient when you only need the final result.
    /// </summary>
    public decimal CalculateFinalBalance(DateTime startDate, DateTime endDate, decimal initialBalance = 0)
    {
        var snapshots = Simulate(startDate, endDate, initialBalance);
        return snapshots.LastOrDefault()?.Balance ?? initialBalance;
    }

    /// <summary>
    /// Calculates the total interest earned over the simulation period.
    /// </summary>
    public decimal CalculateTotalInterest(DateTime startDate, DateTime endDate, decimal initialBalance = 0)
    {
        var snapshots = Simulate(startDate, endDate, initialBalance);
        return snapshots.LastOrDefault()?.CumulativeInterest ?? 0;
    }
}
