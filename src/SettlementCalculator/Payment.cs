namespace SettlementCalculator;

/// <summary>
/// Represents a payment at a specific date.
/// </summary>
public class Payment
{
    /// <summary>
    /// The date when this payment occurs.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The amount of the payment (positive for deposits, negative for withdrawals).
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Optional description of the payment.
    /// </summary>
    public string? Description { get; set; }

    public Payment(DateTime date, decimal amount, string? description = null)
    {
        Date = date;
        Amount = amount;
        Description = description;
    }
}
