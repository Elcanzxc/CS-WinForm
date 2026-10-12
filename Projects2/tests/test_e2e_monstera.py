"""Automated End-to-End Verification Test for Monstera Deliciosa Root Rot Case."""

import math
import unittest
from pathlib import Path

from plantcare.repository.loader import load_knowledge_base
from plantcare.engine.memory import WorkingMemory
from plantcare.engine.inference import InferenceEngine
from plantcare.engine.explanation import ExplanationFacility


class TestEndToEndMonsteraCase(unittest.TestCase):
    def setUp(self):
        self.kb_path = Path(__file__).resolve().parent.parent / "knowledge_base.yaml"
        self.kb = load_knowledge_base(self.kb_path)
        self.engine = InferenceEngine(self.kb.rules, self.kb.pathologies)
        self.explainer = ExplanationFacility(self.kb.symptoms, self.kb.rules, self.kb.pathologies)

    def test_monstera_root_rot_diagnostic_chain(self):
        # 1. Setup session with Monstera deliciosa
        plant_id = "monstera_deliciosa"
        memory = WorkingMemory(plant_id=plant_id)

        # 2. Enter symptoms: moist brown spots, constantly wet substrate, lower leaves yellowing
        memory.assert_fact("leaf_spots_brown_moist", 1.0)
        memory.assert_fact("substrate_constantly_wet", 1.0)
        memory.assert_fact("leaf_lower_yellowing", 1.0)

        # 3. Execute inference
        hyps = self.engine.run_inference(memory)
        ranked = self.engine.get_ranked_diagnoses(hyps)

        # 4. Verify Root Rot is Top-1 diagnosis with high certainty
        top_pathology, top_hyp = ranked[0]
        self.assertEqual(top_pathology.id, "root_rot")
        # Theoretical calculation:
        # RULE_RR_01 (0.85) and RULE_RR_02 (0.75 for Monstera)
        # CF = 0.85 + 0.75 * (1 - 0.85) = 0.9625
        self.assertGreaterEqual(top_hyp.cf, 0.95)
        self.assertEqual(len(top_hyp.traces), 2)

        # 5. Verify HOW explanation contents
        how_data = self.explainer.explain_how("root_rot", hyps, memory)
        self.assertEqual(how_data["final_cf"], top_hyp.cf)
        self.assertEqual(len(how_data["steps"]), 2)
        rule_ids = [step["rule_id"] for step in how_data["steps"]]
        self.assertIn("RULE_RR_01", rule_ids)
        self.assertIn("RULE_RR_02", rule_ids)

        # 6. Verify Non-monotonic Retraction of a symptom
        # Suppose wet substrate was retracted and asserted as dry cracked instead
        memory.retract_fact("substrate_constantly_wet")
        memory.assert_fact("substrate_dry_cracked", 1.0)

        hyps_after = self.engine.run_inference(memory)
        # With substrate_dry_cracked, RULE_RR_04_REFUTE fires with -0.80!
        root_rot_after = hyps_after["root_rot"].cf
        self.assertLess(root_rot_after, 0.0, "Root rot should be strongly refuted when substrate is dry cracked")


if __name__ == "__main__":
    unittest.main()
