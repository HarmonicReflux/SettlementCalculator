using SettlementCalculator;

Console.WriteLine("=== Settlement Calculator Examples ===");
Console.WriteLine();

// Example 1: Simple Interest Calculation
Console.WriteLine("Example 1: Simple Interest Calculation");
Console.WriteLine("--------------------------------------");
var example1 = new BankAccountSimulator();
var start1 = new DateTime(2024, 1, 1);
var end1 = new DateTime(2024, 12, 31);

example1.AddInterestPeriod(new InterestPeriod(start1, end1.AddDays(1), 0.05m)); // 5% annual rate

var snapshots1 = example1.Simulate(start1, end1, initialBalance: 10000m);
Console.WriteLine($"Initial Balance: $10,000.00");
Console.WriteLine($"Interest Rate: 5% annual");
Console.WriteLine($"Duration: 1 year (365 days)");
Console.WriteLine($"Final Balance: ${snapshots1.Last().Balance:F2}");
Console.WriteLine($"Total Interest Earned: ${snapshots1.Last().CumulativeInterest:F2}");
Console.WriteLine();

// Example 2: Time-Varying Interest Rates
Console.WriteLine("Example 2: Time-Varying Interest Rates");
Console.WriteLine("---------------------------------------");
var example2 = new BankAccountSimulator();
var start2 = new DateTime(2024, 1, 1);
var mid2 = new DateTime(2024, 7, 1);
var end2 = new DateTime(2025, 1, 1);

example2.AddInterestPeriod(new InterestPeriod(start2, mid2, 0.03m)); // 3% for first half
example2.AddInterestPeriod(new InterestPeriod(mid2, end2, 0.07m)); // 7% for second half

var snapshots2 = example2.Simulate(start2, end2.AddDays(-1), initialBalance: 10000m);
Console.WriteLine($"Initial Balance: $10,000.00");
Console.WriteLine($"Q1-Q2 Interest Rate: 3% annual");
Console.WriteLine($"Q3-Q4 Interest Rate: 7% annual");
Console.WriteLine($"Final Balance: ${snapshots2.Last().Balance:F2}");
Console.WriteLine($"Total Interest Earned: ${snapshots2.Last().CumulativeInterest:F2}");
Console.WriteLine();

// Example 3: Multiple Payment Streams
Console.WriteLine("Example 3: Multiple Payment Streams with Compounding");
Console.WriteLine("----------------------------------------------------");
var example3 = new BankAccountSimulator();
var start3 = new DateTime(2024, 1, 1);
var end3 = new DateTime(2024, 12, 31);

example3.AddInterestPeriod(new InterestPeriod(start3, end3.AddDays(1), 0.04m)); // 4% annual rate

// Monthly deposits
for (int month = 1; month <= 12; month++)
{
    example3.AddPayment(new Payment(new DateTime(2024, month, 1), 1000m, $"Month {month} deposit"));
}

// Quarterly withdrawals
example3.AddPayment(new Payment(new DateTime(2024, 3, 31), -500m, "Q1 withdrawal"));
example3.AddPayment(new Payment(new DateTime(2024, 6, 30), -500m, "Q2 withdrawal"));
example3.AddPayment(new Payment(new DateTime(2024, 9, 30), -500m, "Q3 withdrawal"));
example3.AddPayment(new Payment(new DateTime(2024, 12, 31), -500m, "Q4 withdrawal"));

var snapshots3 = example3.Simulate(start3, end3, initialBalance: 5000m);
Console.WriteLine($"Initial Balance: $5,000.00");
Console.WriteLine($"Monthly Deposits: $1,000.00 × 12 = $12,000.00");
Console.WriteLine($"Quarterly Withdrawals: $500.00 × 4 = $2,000.00");
Console.WriteLine($"Net Payments: $10,000.00");
Console.WriteLine($"Interest Rate: 4% annual");
Console.WriteLine($"Final Balance: ${snapshots3.Last().Balance:F2}");
Console.WriteLine($"Total Interest Earned: ${snapshots3.Last().CumulativeInterest:F2}");
Console.WriteLine($"Expected without interest: ${5000 + 12000 - 2000:F2}");
Console.WriteLine();

// Example 4: Demonstrating Compounding Effect
Console.WriteLine("Example 4: Demonstrating Daily Compounding Effect");
Console.WriteLine("-------------------------------------------------");
var example4 = new BankAccountSimulator();
var start4 = new DateTime(2024, 1, 1);
var end4 = new DateTime(2024, 1, 31);

example4.AddInterestPeriod(new InterestPeriod(start4, end4.AddDays(1), 0.12m)); // 12% annual rate

var snapshots4 = example4.Simulate(start4, end4, initialBalance: 10000m);

Console.WriteLine($"Initial Balance: $10,000.00");
Console.WriteLine($"Interest Rate: 12% annual (high rate to demonstrate compounding)");
Console.WriteLine($"Duration: 30 days");
Console.WriteLine();
Console.WriteLine("Daily breakdown (showing compounding):");
Console.WriteLine("Day  | Balance      | Daily Interest");
Console.WriteLine("-----|--------------|---------------");
for (int i = 0; i < Math.Min(10, snapshots4.Count); i++)
{
    var dayNum = i + 1;
    var balance = snapshots4[i].Balance;
    var prevBalance = i > 0 ? snapshots4[i - 1].Balance : 10000m;
    var dailyInterest = balance - prevBalance;
    Console.WriteLine($"{dayNum,4} | ${balance,11:F2} | ${dailyInterest,12:F4}");
}
Console.WriteLine("...  | ...          | ...");
Console.WriteLine($"{30,4} | ${snapshots4.Last().Balance,11:F2} | ${snapshots4.Last().Balance - snapshots4[^2].Balance,12:F4}");
Console.WriteLine();
Console.WriteLine($"Total Interest Earned: ${snapshots4.Last().CumulativeInterest:F2}");
Console.WriteLine($"Simple interest (no compounding): ${10000m * 0.12m * 30m / 365m:F2}");
Console.WriteLine($"Compound interest benefit: ${snapshots4.Last().CumulativeInterest - (10000m * 0.12m * 30m / 365m):F2}");
Console.WriteLine();

// Example 5: Complex Scenario - Savings Account with Variable Deposits
Console.WriteLine("Example 5: Complex Scenario - Savings Account Evolution");
Console.WriteLine("--------------------------------------------------------");
var example5 = new BankAccountSimulator();
var start5 = new DateTime(2024, 1, 1);
var end5 = new DateTime(2024, 12, 31);

// Variable interest rates throughout the year (simulating rate changes)
example5.AddInterestPeriod(new InterestPeriod(new DateTime(2024, 1, 1), new DateTime(2024, 4, 1), 0.025m));
example5.AddInterestPeriod(new InterestPeriod(new DateTime(2024, 4, 1), new DateTime(2024, 7, 1), 0.035m));
example5.AddInterestPeriod(new InterestPeriod(new DateTime(2024, 7, 1), new DateTime(2024, 10, 1), 0.045m));
example5.AddInterestPeriod(new InterestPeriod(new DateTime(2024, 10, 1), new DateTime(2025, 1, 1), 0.055m));

// Paycheck deposits (bi-weekly)
var payDate = new DateTime(2024, 1, 5);
while (payDate <= end5)
{
    example5.AddPayment(new Payment(payDate, 2000m, "Paycheck"));
    payDate = payDate.AddDays(14);
}

// Rent payments (monthly)
for (int month = 1; month <= 12; month++)
{
    example5.AddPayment(new Payment(new DateTime(2024, month, 1), -1500m, "Rent"));
}

// Utilities (monthly)
for (int month = 1; month <= 12; month++)
{
    example5.AddPayment(new Payment(new DateTime(2024, month, 15), -200m, "Utilities"));
}

// Bonus payments
example5.AddPayment(new Payment(new DateTime(2024, 3, 15), 5000m, "Q1 Bonus"));
example5.AddPayment(new Payment(new DateTime(2024, 6, 15), 5000m, "Q2 Bonus"));
example5.AddPayment(new Payment(new DateTime(2024, 9, 15), 5000m, "Q3 Bonus"));
example5.AddPayment(new Payment(new DateTime(2024, 12, 15), 5000m, "Q4 Bonus"));

var snapshots5 = example5.Simulate(start5, end5, initialBalance: 10000m);

Console.WriteLine($"Initial Balance: $10,000.00");
Console.WriteLine($"Number of bi-weekly paychecks: {snapshots5.Where(s => s.Date.Day == 5 || s.Date.Day == 19).Count()}");
Console.WriteLine($"Monthly expenses (Rent + Utilities): $1,700.00");
Console.WriteLine($"Quarterly bonuses: $5,000.00 × 4");
Console.WriteLine();
Console.WriteLine("Interest rates by quarter:");
Console.WriteLine("  Q1 (Jan-Mar): 2.5%");
Console.WriteLine("  Q2 (Apr-Jun): 3.5%");
Console.WriteLine("  Q3 (Jul-Sep): 4.5%");
Console.WriteLine("  Q4 (Oct-Dec): 5.5%");
Console.WriteLine();
Console.WriteLine($"Final Balance: ${snapshots5.Last().Balance:F2}");
Console.WriteLine($"Total Interest Earned: ${snapshots5.Last().CumulativeInterest:F2}");

// Calculate monthly progression
Console.WriteLine();
Console.WriteLine("Monthly balance progression:");
Console.WriteLine("Month | End-of-Month Balance");
Console.WriteLine("------|---------------------");
for (int month = 1; month <= 12; month++)
{
    var lastDayOfMonth = new DateTime(2024, month, DateTime.DaysInMonth(2024, month));
    var snapshot = snapshots5.FirstOrDefault(s => s.Date == lastDayOfMonth);
    if (snapshot != null)
    {
        Console.WriteLine($"{month,5} | ${snapshot.Balance,18:F2}");
    }
}

Console.WriteLine();
Console.WriteLine("=== End of Examples ===");
