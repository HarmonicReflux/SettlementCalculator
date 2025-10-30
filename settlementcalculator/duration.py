import pandas as pd
from typing import Union


TimestampLike = Union[str, pd.Timestamp]

def months_between(start: TimestampLike, end: TimestampLike, *, eom_rule: bool = True, allow_negative: bool = True) -> float:
    """
    Minimal function to compute elapsed time between two dates **in decimal months** using pandas.
    
    1. Count whole calendar months first.
    2. Add a final fractional month as `(end - anchor) / (next_anchor - anchor)`.
    3. If `start` is month‑end and `eom_rule=True`, month steps land on month‑end.
    """
    s = pd.to_datetime(start)
    e = pd.to_datetime(end)

    sign = 1.0
    if e < s:
        if not allow_negative:
            raise ValueError("end < start and allow_negative=False")
        s, e = e, s
        sign = -1.0

    if e == s:
        return 0.0

    def is_eom(ts: pd.Timestamp) -> bool:
        return ts == ts + pd.offsets.MonthEnd(0)

    def add_months(ts: pd.Timestamp, n: int) -> pd.Timestamp:
        if eom_rule and is_eom(ts):
            return ts + pd.offsets.MonthEnd(n)
        return ts + pd.DateOffset(months=n)

    months_diff = (e.year - s.year) * 12 + (e.month - s.month)
    candidate = add_months(s, months_diff)
    whole = months_diff - 1 if candidate > e else months_diff

    anchor = add_months(s, whole)
    next_anchor = add_months(anchor, 1)

    if next_anchor <= anchor:
        frac = 0.0
    else:
        frac = float((e - anchor) / (next_anchor - anchor))
        if frac < 0.0:
            frac = 0.0
        elif frac > 1.0:
            frac = 1.0

    return sign * (whole + frac)