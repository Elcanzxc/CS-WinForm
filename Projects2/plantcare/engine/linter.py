"""PlantCare OS — Knowledge Base Linter & Static Integrity Verifier.

Analyzes declarative knowledge bases at startup to guarantee:
- Acyclicity in intermediate inference paths (Graph cycle detection via DFS)
- Absence of direct logical contradictions (A -> B and A -> ~B)
- Referential integrity (no orphaned antecedents or dead-end symbols)
- Bound sanity (Certainty Factors strictly in [-1.0, 1.0])
"""

from __future__ import annotations
from typing import Dict, List, Set
from plantcare.domain.models import Rule, Symptom, Pathology, PlantProfile


class LintIssue:
    def __init__(self, severity: str, code: str, message: str, context: str):
        self.severity = severity  # "ERROR", "WARNING", "INFO"
        self.code = code
        self.message = message
        self.context = context

    def __str__(self) -> str:
        return f"[{self.severity}] {self.code}: {self.message} (Context: {self.context})"


class KnowledgeLinter:
    """Static verification engine for expert system production rules."""

    def __init__(
        self,
        plants: Dict[str, PlantProfile],
        symptoms: Dict[str, Symptom],
        pathologies: Dict[str, Pathology],
        rules: List[Rule],
    ):
        self._plants = plants
        self._symptoms = symptoms
        self._pathologies = pathologies
        self._rules = rules

    def lint(self) -> List[LintIssue]:
        """Runs full static analysis suite over knowledge base."""
        issues: List[LintIssue] = []
        issues.extend(self._check_referential_integrity())
        issues.extend(self._check_rule_bounds())
        issues.extend(self._check_cycles())
        issues.extend(self._check_semantic_contradictions())
        return issues

    def _check_referential_integrity(self) -> List[LintIssue]:
        issues: List[LintIssue] = []
        known_plants = set(self._plants.keys())
        known_symptoms = set(self._symptoms.keys())
        known_pathologies = set(self._pathologies.keys())

        # Collect all intermediate consequents
        derived_symbols = {r.consequent_hypothesis for r in self._rules}

        for rule in self._rules:
            # Check target species
            for sp in rule.target_species:
                if sp != "all" and sp not in known_plants:
                    issues.append(
                        LintIssue(
                            "ERROR",
                            "E001_UNKNOWN_SPECIES",
                            f"Rule targets unknown plant species '{sp}'",
                            rule.id,
                        )
                    )

            # Check antecedents
            for ant in rule.antecedents:
                sid = ant.symptom_id
                if sid not in known_symptoms and sid not in derived_symbols:
                    issues.append(
                        LintIssue(
                            "ERROR",
                            "E002_UNRESOLVABLE_ANTECEDENT",
                            f"Antecedent '{sid}' is neither an observable symptom nor derived by any rule",
                            rule.id,
                        )
                    )

            # Check consequent
            target = rule.consequent_hypothesis
            if target not in known_pathologies and target not in known_symptoms:
                issues.append(
                    LintIssue(
                        "WARNING",
                        "W001_ORPHAN_CONSEQUENT",
                        f"Consequent '{target}' is not in pathology catalog nor used as intermediate symptom",
                        rule.id,
                    )
                )

        # Check for unreferenced pathologies
        covered_pathologies = {
            r.consequent_hypothesis
            for r in self._rules
            if r.consequent_hypothesis in known_pathologies
        }
        for pid in known_pathologies:
            if pid not in covered_pathologies:
                issues.append(
                    LintIssue(
                        "WARNING",
                        "W002_UNREACHABLE_PATHOLOGY",
                        f"Pathology '{pid}' has no production rules confirming it",
                        pid,
                    )
                )

        return issues

    def _check_rule_bounds(self) -> List[LintIssue]:
        issues: List[LintIssue] = []
        for rule in self._rules:
            if not (-1.0 <= rule.rule_cf <= 1.0):
                issues.append(
                    LintIssue(
                        "ERROR",
                        "E003_INVALID_CF_RANGE",
                        f"Rule CF {rule.rule_cf} is out of bounds [-1.0, 1.0]",
                        rule.id,
                    )
                )
            for ant in rule.antecedents:
                if not (0.0 <= ant.threshold <= 1.0):
                    issues.append(
                        LintIssue(
                            "ERROR",
                            "E004_INVALID_THRESHOLD",
                            f"Antecedent threshold {ant.threshold} out of bounds [0.0, 1.0]",
                            rule.id,
                        )
                    )
        return issues

    def _check_cycles(self) -> List[LintIssue]:
        """Detects cyclic dependencies in intermediate rules using DFS."""
        issues: List[LintIssue] = []
        # Build dependency graph: antecedent -> consequent
        adj: Dict[str, Set[str]] = {}
        for rule in self._rules:
            consequent = rule.consequent_hypothesis
            for ant in rule.antecedents:
                sid = ant.symptom_id
                if sid not in adj:
                    adj[sid] = set()
                adj[sid].add(consequent)

        visited: Dict[str, int] = {}  # 0: unvisited, 1: visiting (in stack), 2: finished

        def dfs(node: str, path: List[str]):
            visited[node] = 1
            path.append(node)
            for neighbor in adj.get(node, ()):
                state = visited.get(neighbor, 0)
                if state == 1:
                    cycle_repr = " -> ".join(path + [neighbor])
                    issues.append(
                        LintIssue(
                            "ERROR",
                            "E005_CYCLIC_DEPENDENCY",
                            f"Cycle detected in intermediate deduction rules: {cycle_repr}",
                            node,
                        )
                    )
                elif state == 0:
                    dfs(neighbor, path)
            path.pop()
            visited[node] = 2

        for start_node in list(adj.keys()):
            if visited.get(start_node, 0) == 0:
                dfs(start_node, [])

        return issues

    def _check_semantic_contradictions(self) -> List[LintIssue]:
        """Detects identical premise sets leading to opposing conclusions without differential conditions."""
        issues: List[LintIssue] = []
        rule_signatures: Dict[str, List[Rule]] = {}

        for rule in self._rules:
            # Create canonical sorted antecedent key
            sig = tuple(
                sorted(
                    (ant.symptom_id, ant.sign) for ant in rule.antecedents
                )
            )
            key = f"{rule.target_species}:{rule.consequent_hypothesis}:{sig}"
            if key not in rule_signatures:
                rule_signatures[key] = []
            rule_signatures[key].append(rule)

        for key, matching_rules in rule_signatures.items():
            if len(matching_rules) > 1:
                cfs = [r.rule_cf for r in matching_rules]
                has_positive = any(c > 0 for c in cfs)
                has_negative = any(c < 0 for c in cfs)
                if has_positive and has_negative:
                    rule_ids = ", ".join(r.id for r in matching_rules)
                    issues.append(
                        LintIssue(
                            "ERROR",
                            "E006_SEMANTIC_CONTRADICTION",
                            f"Contradictory rules with identical premises and opposing signs ({rule_ids})",
                            key,
                        )
                    )

        return issues
