# SettlementCalculator

A .NET library for simulating bank account evolution with time-varying interest rates and multiple payment streams. The library computes interest over several durations with time-adjusting values based on compounded interest.

## Features

- **Time-Varying Interest Rates**: Support for multiple interest periods with different annual rates
- **Compounding Interest**: Daily interest calculation with compounding effect
- **Multiple Payment Streams**: Handle deposits and withdrawals at different dates
- **Account Simulation**: Simulate complete account evolution over time
- **Detailed Snapshots**: Get daily snapshots showing balance and cumulative interest

## Installation

Build the project using .NET 9 SDK:

```bash
dotnet build
```

Run the tests:

```bash
dotnet test
```

## Usage

### Basic Example

```csharp
using SettlementCalculator;

// Create a simulator
var simulator = new BankAccountSimulator();

// Define interest periods
var startDate = new DateTime(2024, 1, 1);
var endDate = new DateTime(2024, 12, 31);
simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.05m)); // 5% annual rate

// Add payments
simulator.AddPayment(new Payment(new DateTime(2024, 1, 15), 1000m, "Initial deposit"));
simulator.AddPayment(new Payment(new DateTime(2024, 6, 1), 500m, "Mid-year deposit"));

// Simulate account evolution
var snapshots = simulator.Simulate(startDate, endDate, initialBalance: 0);

// Access results
var finalBalance = snapshots.Last().Balance;
var totalInterest = snapshots.Last().CumulativeInterest;
```

### Time-Varying Interest Rates

```csharp
var simulator = new BankAccountSimulator();

// Different rates for different periods
simulator.AddInterestPeriod(new InterestPeriod(
    new DateTime(2024, 1, 1), 
    new DateTime(2024, 6, 1), 
    0.03m)); // 3% for first half

simulator.AddInterestPeriod(new InterestPeriod(
    new DateTime(2024, 6, 1), 
    new DateTime(2025, 1, 1), 
    0.05m)); // 5% for second half

var snapshots = simulator.Simulate(
    new DateTime(2024, 1, 1), 
    new DateTime(2025, 1, 1), 
    10000m);
```

### Multiple Payment Streams

```csharp
var simulator = new BankAccountSimulator();
simulator.AddInterestPeriod(new InterestPeriod(startDate, endDate, 0.04m));

// Regular monthly deposits
for (int month = 1; month <= 12; month++)
{
    simulator.AddPayment(new Payment(
        new DateTime(2024, month, 15), 
        1000m, 
        $"Month {month} deposit"));
}

// Occasional withdrawals
simulator.AddPayment(new Payment(new DateTime(2024, 4, 20), -500m, "Withdrawal"));
simulator.AddPayment(new Payment(new DateTime(2024, 8, 10), -750m, "Withdrawal"));

var snapshots = simulator.Simulate(startDate, endDate, 5000m);
```

## How It Works

The `BankAccountSimulator` processes each day in the simulation period:

1. **Payment Processing**: All payments scheduled for the current date are applied to the balance
2. **Interest Calculation**: Daily interest is calculated based on the current balance and applicable interest rate
3. **Compounding**: Interest is added to the balance, affecting future interest calculations
4. **Snapshot Creation**: An `AccountSnapshot` is created recording the end-of-day state

Interest is calculated using the formula:
```
Daily Interest = Current Balance × (Annual Rate / 365)
```

## Core Classes

### `BankAccountSimulator`
Main class for running simulations. Methods:
- `AddInterestPeriod(InterestPeriod)`: Add an interest rate period
- `AddPayment(Payment)`: Add a payment transaction
- `Simulate(startDate, endDate, initialBalance)`: Run the simulation and get daily snapshots
- `CalculateFinalBalance(startDate, endDate, initialBalance)`: Get only the final balance
- `CalculateTotalInterest(startDate, endDate, initialBalance)`: Get only the total interest

### `InterestPeriod`
Represents a time period with a specific interest rate:
- `StartDate`: Period start date (inclusive)
- `EndDate`: Period end date (exclusive)
- `AnnualRate`: Annual interest rate (e.g., 0.05 for 5%)
- `DurationDays`: Calculated number of days in the period

### `Payment`
Represents a payment transaction:
- `Date`: Payment date
- `Amount`: Payment amount (positive for deposits, negative for withdrawals)
- `Description`: Optional description

### `AccountSnapshot`
Represents account state at a specific date:
- `Date`: Snapshot date
- `Balance`: Account balance
- `CumulativeInterest`: Total interest earned up to this date

## Testing

The library includes comprehensive tests covering:
- Interest calculation with compounding
- Multiple interest periods with different rates
- Payment processing
- Edge cases and error handling

Run tests with:
```bash
dotnet test
```

## License

This project is provided as-is for demonstration purposes.