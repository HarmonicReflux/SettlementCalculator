import pytest
from settlementcalculator.duration import months_between

# Reference cases from README
@pytest.mark.parametrize(
    "start,end,expected",
    [
        ("2024-01-31", "2024-02-29", 1.0),
        ("2023-01-31", "2023-02-28", 1.0),
        ("2024-01-31", "2024-03-15", 1.4839),
        ("2025-03-01", "2025-03-31", 0.9677),
        ("2025-02-10", "2025-02-10", 0.0),
    ],
)
def test_reference_checks(start, end, expected):
    v = months_between(start, end)
    # exact for the 1.0 and 0.0 cases, approx for the fractional ones
    if expected in (0.0, 1.0):
        assert round(v, 6) == expected
    else:
        assert v == pytest.approx(expected, abs=1e-4)


def test_signed_and_error_modes():
    # Negative result when allowed
    v_neg = months_between("2025-03-31", "2025-03-01", allow_negative=True)
    assert v_neg == pytest.approx(-0.9677, abs=1e-4)
    # Error when not allowed
    with pytest.raises(ValueError):
        months_between("2025-03-31", "2025-03-01", allow_negative=False)


def test_symmetry_property():
    a, b = "2024-01-31", "2024-03-15"
    fwd = months_between(a, b)
    rev = months_between(b, a)
    assert fwd == pytest.approx(-rev, abs=1e-12)
