"""PlantCare OS — Core Mathematical Engine: Shortliffe / MYCIN Certainty Calculus.

Provides exact implementation of Certainty Factors (CF) without simplifications:
- Conjunction: min(CF_1, ..., CF_n)
- Disjunction: max(CF_1, ..., CF_n)
- Rule Activation: CF_rule * max(0, CF_premise)
- Combination of independent evidence with robust numerical edge-case handling.
"""

from __future__ import annotations
import math
from typing import Sequence

EPSILON: float = 1e-9


def clamp_cf(val: float) -> float:
    """Clamps Certainty Factor strictly to the closed interval [-1.0, 1.0]."""
    if math.isnan(val):
        return 0.0
    if val > 1.0:
        return 1.0
    if val < -1.0:
        return -1.0
    return float(val)


def combine_conjunction(cfs: Sequence[float]) -> float:
    """Computes CF for a logical conjunction (AND) of premises:

    CF(e_1 and e_2 and ... and e_n) = min(CF(e_1), CF(e_2), ..., CF(e_n))
    """
    if not cfs:
        return 0.0
    return clamp_cf(min(clamp_cf(c) for c in cfs))


def combine_disjunction(cfs: Sequence[float]) -> float:
    """Computes CF for a logical disjunction (OR) of premises:

    CF(e_1 or e_2 or ... or e_n) = max(CF(e_1), CF(e_2), ..., CF(e_n))
    """
    if not cfs:
        return 0.0
    return clamp_cf(max(clamp_cf(c) for c in cfs))


def activate_rule(rule_cf: float, premise_cf: float) -> float:
    """Computes the hypothesis Certainty Factor yielded by rule activation:

    CF(h, e) = CF_rule * max(0, CF(e))

    If premise evidence is non-positive (CF <= 0), the rule does not contribute
    positive confirmation to the hypothesis.
    """
    clamped_rule = clamp_cf(rule_cf)
    effective_premise = max(0.0, clamp_cf(premise_cf))
    return clamp_cf(clamped_rule * effective_premise)


def combine_two_evidences(cf1: float, cf2: float) -> float:
    """Combines two independent Certainty Factors for the same hypothesis:

    1. Both positive (cf1 >= 0, cf2 >= 0):
       CF_comb = cf1 + cf2 * (1 - cf1)

    2. Both negative (cf1 <= 0, cf2 <= 0):
       CF_comb = cf1 + cf2 * (1 + cf1)

    3. Opposite signs (cf1 * cf2 < 0):
       CF_comb = (cf1 + cf2) / (1 - min(|cf1|, |cf2|))
       Edge case: if |cf1| == 1.0 and |cf2| == 1.0 (e.g. +1.0 and -1.0),
       a catastrophic contradiction occurs. Returns 0.0 to prevent division by zero.
    """
    c1 = clamp_cf(cf1)
    c2 = clamp_cf(cf2)

    # Both non-negative
    if c1 >= 0.0 and c2 >= 0.0:
        combined = c1 + c2 * (1.0 - c1)
        return clamp_cf(combined)

    # Both non-positive
    if c1 <= 0.0 and c2 <= 0.0:
        combined = c1 + c2 * (1.0 + c1)
        return clamp_cf(combined)

    # Opposite signs
    min_abs = min(abs(c1), abs(c2))
    denominator = 1.0 - min_abs

    if denominator <= EPSILON:
        # Contradiction: perfect proof vs perfect refutation (+1.0 and -1.0)
        return 0.0

    combined = (c1 + c2) / denominator
    return clamp_cf(combined)


def combine_multiple_evidences(cfs: Sequence[float]) -> float:
    """Iteratively combines an arbitrary sequence of independent Certainty Factors."""
    if not cfs:
        return 0.0
    accumulated = clamp_cf(cfs[0])
    for c in cfs[1:]:
        accumulated = combine_two_evidences(accumulated, c)
    return accumulated
