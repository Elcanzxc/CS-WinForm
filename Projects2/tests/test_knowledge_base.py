"""Test suite for Knowledge Base loading and static linting."""

import unittest
from pathlib import Path
from plantcare.repository.loader import load_knowledge_base


class TestKnowledgeBaseIntegrity(unittest.TestCase):
    def setUp(self):
        self.kb_path = Path(__file__).resolve().parent.parent / "knowledge_base.yaml"
        self.kb = load_knowledge_base(self.kb_path)

    def test_plants_loaded(self):
        self.assertGreaterEqual(len(self.kb.plants), 5)
        self.assertIn("monstera_deliciosa", self.kb.plants)
        self.assertIn("ficus_benjamina", self.kb.plants)
        self.assertIn("sansevieria_trifasciata", self.kb.plants)
        self.assertIn("spathiphyllum_wallisii", self.kb.plants)
        self.assertIn("phalaenopsis", self.kb.plants)

    def test_pathologies_loaded(self):
        # All 15 required pathologies must be present
        required_pathologies = [
            "root_rot",
            "acute_water_deficit",
            "etiolation",
            "photodestruction",
            "spider_mite",
            "mealybug",
            "thrips",
            "scale_insect",
            "iron_chlorosis",
            "nitrogen_deficiency",
            "osmotic_root_burn",
            "bacterial_spot",
            "low_vpd_dry_air",
            "temperature_shock",
            "edema",
        ]
        for pid in required_pathologies:
            self.assertIn(pid, self.kb.pathologies, f"Missing required pathology: {pid}")
            pathology = self.kb.pathologies[pid]
            # Ensure complete therapeutic protocols
            self.assertTrue(len(pathology.therapeutic_protocol.emergency_measures) > 0)
            self.assertTrue(len(pathology.therapeutic_protocol.microclimate_adjustments) > 0)
            self.assertTrue(len(pathology.therapeutic_protocol.pharmacotherapy) > 0)
            self.assertTrue(len(pathology.therapeutic_protocol.prognosis) > 0)

    def test_rules_and_linter(self):
        self.assertGreaterEqual(len(self.kb.rules), 15)
        issues = self.kb.lint()
        # Ensure zero ERROR-level issues (Warnings like unreferenced symptoms are ok, but no errors)
        errors = [issue for issue in issues if issue.severity == "ERROR"]
        self.assertEqual(len(errors), 0, f"Lint errors found in knowledge base: {[str(e) for e in errors]}")


if __name__ == "__main__":
    unittest.main()
