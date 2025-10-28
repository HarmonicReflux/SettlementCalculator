namespace SettlementCalculator;

/// <summary>
/// Represents the state of an account at a specific point in time.
/// </summary>
public class AccountSnapshot
{
    /// <summary>
    /// The date of this snapshot.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The balance at this date.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// The cumulative interest earned up to this date.
    /// </summary>
    public decimal CumulativeInterest { get; set; }

    public AccountSnapshot(DateTime date, decimal balance, decimal cumulativeInterest)
    {
        Date = date;
        Balance = balance;
        CumulativeInterest = cumulativeInterest;
    }
}
