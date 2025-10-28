namespace SettlementCalculator;

/// <summary>
/// Represents a time period with a specific interest rate.
/// </summary>
public class InterestPeriod
{
    /// <summary>
    /// The start date of this interest period (inclusive).
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date of this interest period (exclusive).
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// The annual interest rate for this period (e.g., 0.05 for 5%).
    /// </summary>
    public decimal AnnualRate { get; set; }

    public InterestPeriod(DateTime startDate, DateTime endDate, decimal annualRate)
    {
        if (endDate <= startDate)
        {
            throw new ArgumentException("End date must be after start date.");
        }

        StartDate = startDate;
        EndDate = endDate;
        AnnualRate = annualRate;
    }

    /// <summary>
    /// Calculates the number of days in this period.
    /// </summary>
    public int DurationDays => (EndDate - StartDate).Days;
}
