"""PlantCare OS — Forward Chaining Inference Engine.

Implements classic expert system forward deduction with:
- Conflict Resolution Strategy: Specificity > Rule Priority > Recency
- Exact Shortliffe / MYCIN certainty propagation
- Non-monotonic recalculation and rule execution audit tracing
- Multi-pass inference with loop prevention
"""

from __future__ import annotations
from typing import Dict, List, Optional, Set
from plantcare.domain.models import (
    Rule,
    Hypothesis,
    RuleActivationTrace,
    Pathology,
)
from plantcare.domain.certainty import (
    combine_conjunction,
    combine_disjunction,
    activate_rule,
    combine_two_evidences,
    clamp_cf,
)
from plantcare.engine.memory import WorkingMemory


class InferenceEngine:
    """Forward Chaining Symbolic Deductive Engine."""

    def __init__(
        self,
        rules: List[Rule],
        pathologies: Dict[str, Pathology],
    ):
        self._rules = list(rules)
        self._pathologies = dict(pathologies)

    def _is_rule_applicable_to_plant(self, rule: Rule, plant_id: Optional[str]) -> bool:
        """Checks if rule scope includes the current plant species."""
        if "all" in rule.target_species:
            return True
        if plant_id is None:
            return True
        return plant_id in rule.target_species

    def _calculate_recency(self, rule: Rule, memory: WorkingMemory) -> float:
        """Computes rule recency score based on timestamps of matched facts."""
        timestamps: List[float] = []
        for ant in rule.antecedents:
            fact = memory.get_fact(ant.symptom_id)
            if fact:
                timestamps.append(fact.timestamp)
        return max(timestamps) if timestamps else 0.0

    def _evaluate_rule_premises(
        self,
        rule: Rule,
        memory: WorkingMemory,
    ) -> Optional[tuple[float, Dict[str, float]]]:
        """Evaluates antecedents against working memory.

        Returns (effective_premise_cf, antecedent_cfs_dict) if rule fires,
        or None if antecedents are unsatisfied.
        """
        ant_cfs: Dict[str, float] = {}
        for ant in rule.antecedents:
            fact = memory.get_fact(ant.symptom_id)
            if fact is None:
                return None  # Missing necessary information for this rule

            val = fact.cf
            if ant.sign == "present":
                if val < ant.threshold:
                    return None  # Insufficient positive certainty
                effective = val * ant.weight
            else:  # ant.sign == "absent"
                if val > -ant.threshold:
                    return None  # Symptom is not sufficiently refuted
                effective = abs(val) * ant.weight

            ant_cfs[ant.symptom_id] = effective

        if not ant_cfs:
            return None

        vals = list(ant_cfs.values())
        if rule.logic_operator == "AND":
            composite = combine_conjunction(vals)
        else:  # OR
            composite = combine_disjunction(vals)

        if composite <= 0.0:
            return None

        return composite, ant_cfs

    def run_inference(self, memory: WorkingMemory) -> Dict[str, Hypothesis]:
        """Executes forward chaining inference over current working memory.

        Returns dictionary of evaluated hypotheses indexed by pathology ID.
        """
        # Initialize candidate hypotheses for all known pathologies
        hypotheses: Dict[str, Hypothesis] = {
            pid: Hypothesis(pathology_id=pid, cf=0.0)
            for pid in self._pathologies.keys()
        }

        fired_rules: Set[str] = set()
        max_cycles = len(self._rules) + 5
        cycle = 0

        while cycle < max_cycles:
            cycle += 1
            # 1. Conflict Set identification
            candidate_firings = []

            for rule in self._rules:
                if rule.id in fired_rules:
                    continue
                if not self._is_rule_applicable_to_plant(rule, memory.plant_id):
                    continue

                eval_result = self._evaluate_rule_premises(rule, memory)
                if eval_result is not None:
                    premise_cf, ant_cfs = eval_result
                    recency = self._calculate_recency(rule, memory)
                    candidate_firings.append((rule, premise_cf, ant_cfs, recency))

            if not candidate_firings:
                break  # Fixed point reached: no more rules can fire

            # 2. Conflict Resolution: Specificity > Rule Priority > Recency
            candidate_firings.sort(
                key=lambda item: (
                    item[0].specificity,
                    item[0].priority,
                    item[3],
                ),
                reverse=True,
            )

            # Fire the highest priority rule from conflict set
            chosen_rule, premise_cf, ant_cfs, _ = candidate_firings[0]
            fired_rules.add(chosen_rule.id)

            yielded_cf = activate_rule(chosen_rule.rule_cf, premise_cf)
            target = chosen_rule.consequent_hypothesis

            # Check if consequent is a pathology hypothesis
            if target in hypotheses:
                hyp = hypotheses[target]
                prior_cf = hyp.cf
                posterior_cf = combine_two_evidences(prior_cf, yielded_cf)

                support = list(hyp.support_rules)
                refuting = list(hyp.refuting_rules)
                if yielded_cf > 0:
                    support.append(chosen_rule.id)
                elif yielded_cf < 0:
                    refuting.append(chosen_rule.id)

                trace = RuleActivationTrace(
                    rule_id=chosen_rule.id,
                    consequent=target,
                    rule_cf=chosen_rule.rule_cf,
                    antecedent_cfs=ant_cfs,
                    effective_premise_cf=premise_cf,
                    yielded_cf=yielded_cf,
                    prior_hypothesis_cf=prior_cf,
                    posterior_hypothesis_cf=posterior_cf,
                    explanation_how=chosen_rule.explanation_how,
                )

                hypotheses[target] = Hypothesis(
                    pathology_id=target,
                    cf=clamp_cf(posterior_cf),
                    support_rules=support,
                    refuting_rules=refuting,
                    traces=hyp.traces + [trace],
                )
            else:
                # Consequent is an intermediate derived fact!
                # Assert into working memory for further chaining passes
                if not memory.has_fact(target):
                    memory.assert_fact(
                        symptom_id=target,
                        cf=yielded_cf,
                        source="inference_engine",
                    )

        return hypotheses

    def get_ranked_diagnoses(
        self,
        hypotheses: Dict[str, Hypothesis],
        min_cf_threshold: float = -1.0,
    ) -> List[tuple[Pathology, Hypothesis]]:
        """Returns pathologies ranked by posterior Certainty Factor descending."""
        ranked = []
        for pid, hyp in hypotheses.items():
            if hyp.cf >= min_cf_threshold and pid in self._pathologies:
                pathology = self._pathologies[pid]
                ranked.append((pathology, hyp))

        ranked.sort(key=lambda item: item[1].cf, reverse=True)
        return ranked
