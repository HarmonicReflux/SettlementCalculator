# Settlement Calculator

Utilities for computing durations as decimal months and for discrete month-by-month compounding. Built in Python with pandas. Sized for quick Hacktoberfest contributions.

## What this library does

1. `months_elapsed_decimal(start, end, eom_rule=True)`  
   Returns elapsed time as decimal months. Counts whole calendar months first, then a final fractional month. End-of-month aware.

2. `compounded_value(principal, monthly_rate, start, end)`  
   Computes value under discrete monthly compounding. Applies whole months as powers, then a pro-rata fractional month.

## Requirements

* Python 3.12
* pandas and pytest are installed via the steps below

## Quick start with uv

1. Ensure `uv` is installed and on your PATH  
   `uv --version`

2. Create a fresh environment and install locked dependencies  
   `uv sync`

3. Run tests  
   `uv run pytest`

## Quick start with pip

1. Create a virtual environment  
   `python3 -m venv .venv`  
   `. .venv/bin/activate`

2. Install dependencies  
   `pip install -r requirements.txt`

3. Run tests  
   `python -m pytest`

## Usage examples

Python API

```python
from settlementcalculator import months_elapsed_decimal, compounded_value

# Decimal months
m = months_elapsed_decimal("2024-01-31", "2024-03-15")
print(round(m, 4))  # expected about 1.4839

# Discrete monthly compounding
v = compounded_value(principal=1000.0, monthly_rate=0.01,
                     start="2024-01-31", end="2024-03-15")
print(round(v, 2))
