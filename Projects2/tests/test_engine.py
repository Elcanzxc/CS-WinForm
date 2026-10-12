"""Unit tests for WorkingMemory, InferenceEngine, AdaptiveQuestioner, and ExplanationFacility."""

import math
import unittest
from pathlib import Path

from plantcare.repository.loader import load_knowledge_base
from plantcare.engine.memory import WorkingMemory
from plantcare.engine.inference import InferenceEngine
from plantcare.engine.questioner import AdaptiveQuestioner, calculate_entropy
from plantcare.engine.explanation import ExplanationFacility


class TestEngineComponents(unittest.TestCase):
    def setUp(self):
        self.kb_path = Path(__file__).resolve().parent.parent / "knowledge_base.yaml"
        self.kb = load_knowledge_base(self.kb_path)
        self.engine = InferenceEngine(self.kb.rules, self.kb.pathologies)
        self.questioner = AdaptiveQuestioner(
            self.kb.symptoms, self.kb.rules, self.kb.pathologies, self.engine
        )
        self.explainer = ExplanationFacility(
            self.kb.symptoms, self.kb.rules, self.kb.pathologies
        )

    def test_working_memory_and_retraction(self):
        mem = WorkingMemory(plant_id="monstera_deliciosa")
        mem.assert_fact("substrate_constantly_wet", 1.0)
        self.assertTrue(mem.has_fact("substrate_constantly_wet"))
        self.assertEqual(mem.get_cf("substrate_constantly_wet"), 1.0)

        # Belief revision: update certainty
        mem.assert_fact("substrate_constantly_wet", -0.5)
        self.assertEqual(mem.get_cf("substrate_constantly_wet"), -0.5)

        # Rollback
        self.assertTrue(mem.rollback())
        self.assertEqual(mem.get_cf("substrate_constantly_wet"), 1.0)

        # Retract
        retracted = mem.retract_fact("substrate_constantly_wet")
        self.assertIsNotNone(retracted)
        self.assertFalse(mem.has_fact("substrate_constantly_wet"))

    def test_inference_forward_chaining(self):
        mem = WorkingMemory(plant_id="monstera_deliciosa")
        # Assert facts for Monstera root rot
        mem.assert_fact("substrate_constantly_wet", 1.0)
        mem.assert_fact("leaf_spots_brown_moist", 1.0)
        mem.assert_fact("leaf_lower_yellowing", 1.0)

        hyps = self.engine.run_inference(mem)
        self.assertIn("root_rot", hyps)
        root_rot_cf = hyps["root_rot"].cf
        self.assertGreater(root_rot_cf, 0.8)
        self.assertTrue(len(hyps["root_rot"].traces) >= 2)

        # Diagnoses ranking
        ranked = self.engine.get_ranked_diagnoses(hyps)
        self.assertEqual(ranked[0][0].id, "root_rot")

    def test_adaptive_questioner_information_gain(self):
        mem = WorkingMemory(plant_id="monstera_deliciosa")
        next_q = self.questioner.select_next_question(mem)
        self.assertIsNotNone(next_q)
        symptom, eig = next_q
        self.assertIsNotNone(symptom.id)
        self.assertGreaterEqual(eig, 0.0)

    def test_explanation_why_and_how(self):
        mem = WorkingMemory(plant_id="monstera_deliciosa")
        mem.assert_fact("substrate_constantly_wet", 1.0)
        mem.assert_fact("leaf_spots_brown_moist", 1.0)

        hyps = self.engine.run_inference(mem)

        # Test WHY for unasked symptom
        why_res = self.explainer.explain_why("leaf_lower_yellowing", mem, hyps)
        self.assertIn("biological_rationale", why_res)
        self.assertTrue(len(why_res["awaiting_rules"]) > 0)

        # Test HOW for root_rot
        how_res = self.explainer.explain_how("root_rot", hyps, mem)
        self.assertEqual(how_res["pathology_id"], "root_rot")
        self.assertTrue(len(how_res["steps"]) > 0)

        # Test HOW Tree generation
        tree = self.explainer.build_how_tree("root_rot", hyps, mem)
        self.assertIsNotNone(tree)


if __name__ == "__main__":
    unittest.main()
