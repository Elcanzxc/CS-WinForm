"""Unit tests for Shortliffe / MYCIN Certainty Factors Algebra using standard unittest."""

import math
import unittest
from plantcare.domain.certainty import (
    combine_conjunction,
    combine_disjunction,
    activate_rule,
    combine_two_evidences,
    combine_multiple_evidences,
    clamp_cf,
)


class TestCertaintyFactors(unittest.TestCase):
    def test_conjunction_and_disjunction(self):
        cfs = [0.2, 0.7, 0.5]
        self.assertTrue(math.isclose(combine_conjunction(cfs), 0.2))
        self.assertTrue(math.isclose(combine_disjunction(cfs), 0.7))

        self.assertEqual(combine_conjunction([]), 0.0)
        self.assertEqual(combine_disjunction([]), 0.0)

    def test_rule_activation(self):
        self.assertTrue(math.isclose(activate_rule(0.8, 0.5), 0.4))
        self.assertEqual(activate_rule(0.8, -0.5), 0.0)
        self.assertTrue(math.isclose(activate_rule(-0.8, 0.5), -0.4))
        self.assertEqual(activate_rule(-0.8, -0.5), 0.0)

    def test_mycin_combining_positive(self):
        # CF1 = 0.8, CF2 = 0.5 => 0.8 + 0.5 * (1 - 0.8) = 0.9
        res = combine_two_evidences(0.8, 0.5)
        self.assertTrue(math.isclose(res, 0.9))

    def test_mycin_combining_negative(self):
        # CF1 = -0.8, CF2 = -0.5 => -0.8 + (-0.5) * (1 + (-0.8)) = -0.9
        res = combine_two_evidences(-0.8, -0.5)
        self.assertTrue(math.isclose(res, -0.9))

    def test_mycin_combining_opposite_signs(self):
        # CF1 = 0.8, CF2 = -0.5 => (0.8 - 0.5) / (1 - min(0.8, 0.5)) = 0.3 / 0.5 = 0.6
        res = combine_two_evidences(0.8, -0.5)
        self.assertTrue(math.isclose(res, 0.6))

        res_sym = combine_two_evidences(-0.5, 0.8)
        self.assertTrue(math.isclose(res_sym, 0.6))

    def test_mycin_contradiction_edge_case(self):
        # CF1 = 1.0, CF2 = -1.0 => Denominator 1 - 1 = 0. Handled gracefully without crash
        res = combine_two_evidences(1.0, -1.0)
        self.assertEqual(res, 0.0)

    def test_mycin_chain_combination(self):
        # 0.6 and 0.4 => 0.6 + 0.4 * 0.4 = 0.76
        # 0.76 and 0.5 => 0.76 + 0.5 * 0.24 = 0.88
        res = combine_multiple_evidences([0.6, 0.4, 0.5])
        self.assertTrue(math.isclose(res, 0.88))


if __name__ == "__main__":
    unittest.main()
