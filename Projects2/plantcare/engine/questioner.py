"""PlantCare OS — Adaptive Question Selector (Information-Theoretic Engine).

Implements Expected Information Gain (EIG) maximization over hypothesis entropy:
Q* = argmax_Q [ H(Hypotheses) - sum_a P(a | Q) * H(Hypotheses | Q = a) ]
"""

from __future__ import annotations
import math
from typing import Dict, List, Optional, Set
from plantcare.domain.models import Symptom, Rule, Hypothesis, Pathology
from plantcare.engine.memory import WorkingMemory
from plantcare.engine.inference import InferenceEngine


def calculate_entropy(probabilities: List[float]) -> float:
    """Computes Shannon entropy H in bits: -sum(p * log2(p))."""
    h = 0.0
    for p in probabilities:
        if p > 1e-9:
            h -= p * math.log2(p)
    return h


def get_hypothesis_distribution(hypotheses: Dict[str, Hypothesis]) -> Dict[str, float]:
    """Normalizes positive Certainty Factors into a proper probability distribution."""
    positive_mass: Dict[str, float] = {}
    total_pos = 0.0

    for pid, hyp in hypotheses.items():
        val = max(0.0, hyp.cf)
        positive_mass[pid] = val
        total_pos += val

    if total_pos > 1e-6:
        return {pid: val / total_pos for pid, val in positive_mass.items()}

    # Uniform prior across candidate pathologies when no positive confirmations exist
    n = len(hypotheses)
    if n == 0:
        return {}
    return {pid: 1.0 / n for pid in hypotheses.keys()}


class AdaptiveQuestioner:
    """Selects the next optimal diagnostic question by maximizing Information Gain."""

    def __init__(
        self,
        symptoms: Dict[str, Symptom],
        rules: List[Rule],
        pathologies: Dict[str, Pathology],
        inference_engine: InferenceEngine,
    ):
        self._symptoms = dict(symptoms)
        self._rules = list(rules)
        self._pathologies = dict(pathologies)
        self._engine = inference_engine

    def get_candidate_symptoms(self, memory: WorkingMemory) -> List[str]:
        """Finds all unasked symptoms relevant to currently applicable rules."""
        candidate_ids: Set[str] = set()

        for rule in self._rules:
            if not self._engine._is_rule_applicable_to_plant(rule, memory.plant_id):
                continue

            for ant in rule.antecedents:
                sid = ant.symptom_id
                # Check if symptom is defined and not yet asserted in memory
                if sid in self._symptoms and not memory.has_fact(sid):
                    candidate_ids.add(sid)

        return sorted(list(candidate_ids))

    def evaluate_question_information_gain(
        self,
        symptom_id: str,
        memory: WorkingMemory,
        current_distribution: Dict[str, float],
        current_entropy: float,
    ) -> float:
        """Calculates Expected Information Gain (EIG) for a hypothetical question."""
        # 1. Estimate P(yes | Q) from hypotheses expecting this symptom
        present_pathologies: Set[str] = set()
        for rule in self._rules:
            if not self._engine._is_rule_applicable_to_plant(rule, memory.plant_id):
                continue
            for ant in rule.antecedents:
                if ant.symptom_id == symptom_id and ant.sign == "present":
                    present_pathologies.add(rule.consequent_hypothesis)

        prior_yes = sum(
            current_distribution.get(pid, 0.0) for pid in present_pathologies
        )
        # Bayesian smoothing
        p_yes = max(0.15, min(0.85, 0.7 * prior_yes + 0.3 * 0.5))
        p_no = 1.0 - p_yes

        # 2. Simulate Q = Yes (+1.0)
        mem_yes = memory.clone()
        mem_yes.assert_fact(symptom_id, 1.0, source="user_input")
        hyp_yes = self._engine.run_inference(mem_yes)
        dist_yes = get_hypothesis_distribution(hyp_yes)
        h_yes = calculate_entropy(list(dist_yes.values()))

        # 3. Simulate Q = No (-1.0)
        mem_no = memory.clone()
        mem_no.assert_fact(symptom_id, -1.0, source="user_input")
        hyp_no = self._engine.run_inference(mem_no)
        dist_no = get_hypothesis_distribution(hyp_no)
        h_no = calculate_entropy(list(dist_no.values()))

        # 4. Expected conditional entropy
        expected_conditional_entropy = p_yes * h_yes + p_no * h_no

        # 5. Information Gain
        eig = current_entropy - expected_conditional_entropy
        return max(0.0, eig)

    def select_next_question(
        self,
        memory: WorkingMemory,
    ) -> Optional[tuple[Symptom, float]]:
        """Selects Q* that yields maximum Information Gain."""
        candidates = self.get_candidate_symptoms(memory)
        if not candidates:
            return None

        current_hyps = self._engine.run_inference(memory)
        dist = get_hypothesis_distribution(current_hyps)
        base_entropy = calculate_entropy(list(dist.values()))

        scored_questions = []
        for sid in candidates:
            eig = self.evaluate_question_information_gain(
                sid, memory, dist, base_entropy
            )
            symptom = self._symptoms[sid]
            # Tie breaker: count of rules referencing this symptom
            rule_count = sum(
                1 for r in self._rules
                if any(ant.symptom_id == sid for ant in r.antecedents)
            )
            scored_questions.append((eig, rule_count, symptom))

        if not scored_questions:
            return None

        # Sort descending by EIG, then by rule relevance
        scored_questions.sort(key=lambda item: (item[0], item[1]), reverse=True)
        best_eig, _, best_symptom = scored_questions[0]
        return best_symptom, best_eig
