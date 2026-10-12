"""PlantCare OS — Explainable AI (XAI) İzah Alt-Sistemi.

İki əsas funksionallığı təmin edir:
- WHY: Sualın niyə verildiyini gözləyən qaydalar və aktiv hipotezlər əsasında əsaslandırır.
- HOW: Təsdiqlənmiş diaqnozun deduktiv sübut ağacını (DAG) qurur.
"""

from __future__ import annotations
from typing import Dict, List, Optional
from rich.tree import Tree
from plantcare.domain.models import (
    Symptom,
    Rule,
    Hypothesis,
    Pathology,
)
from plantcare.engine.memory import WorkingMemory


class ExplanationFacility:
    """XAI Audit və İzah Alt-Sistemi."""

    def __init__(
        self,
        symptoms: Dict[str, Symptom],
        rules: List[Rule],
        pathologies: Dict[str, Pathology],
    ):
        self._symptoms = dict(symptoms)
        self._rules = list(rules)
        self._pathologies = dict(pathologies)

    def explain_why(
        self,
        symptom_id: str,
        memory: WorkingMemory,
        current_hypotheses: Dict[str, Hypothesis],
    ) -> Dict[str, object]:
        """Sualın NİYƏ verildiyini izah edir."""
        symptom = self._symptoms.get(symptom_id)
        if not symptom:
            return {"error": f"Naməlum simptom '{symptom_id}'"}

        awaiting_rules = []
        target_pathology_ids = set()

        for rule in self._rules:
            if "all" not in rule.target_species and memory.plant_id and memory.plant_id not in rule.target_species:
                continue

            for ant in rule.antecedents:
                if ant.symptom_id == symptom_id:
                    awaiting_rules.append(rule)
                    target_pathology_ids.add(rule.consequent_hypothesis)

        active_leading = []
        for pid in target_pathology_ids:
            if pid in current_hypotheses:
                hyp = current_hypotheses[pid]
                pathology = self._pathologies.get(pid)
                name = pathology.name if pathology else pid
                active_leading.append({
                    "id": pid,
                    "name": name,
                    "cf": hyp.cf,
                })
        active_leading.sort(key=lambda x: x["cf"], reverse=True)

        return {
            "symptom_id": symptom_id,
            "symptom_name": symptom.name,
            "biological_rationale": symptom.why_explanation,
            "affected_hypotheses": active_leading,
            "awaiting_rules": [
                {
                    "rule_id": r.id,
                    "name": r.name,
                    "target": r.consequent_hypothesis,
                    "rule_cf": r.rule_cf,
                    "specificity": r.specificity,
                }
                for r in awaiting_rules
            ],
        }

    def explain_how(
        self,
        pathology_id: str,
        hypotheses: Dict[str, Hypothesis],
        memory: WorkingMemory,
    ) -> Dict[str, object]:
        """HOW çıxarışı: Diaqnozun necə əldə edildiyini addım-addım göstərir."""
        hyp = hypotheses.get(pathology_id)
        pathology = self._pathologies.get(pathology_id)

        if not hyp or not pathology:
            return {"error": f"'{pathology_id}' patologiyası aktiv hipotezlər arasında tapılmadı."}

        steps = []
        for trace in hyp.traces:
            facts_detail = []
            for ant_id, effective_cf in trace.antecedent_cfs.items():
                fact = memory.get_fact(ant_id)
                sym = self._symptoms.get(ant_id)
                facts_detail.append({
                    "symptom_id": ant_id,
                    "symptom_name": sym.name if sym else ant_id,
                    "stated_cf": fact.cf if fact else 0.0,
                    "effective_cf": effective_cf,
                    "source": fact.source if fact else "naməlum",
                })

            steps.append({
                "rule_id": trace.rule_id,
                "rule_cf": trace.rule_cf,
                "effective_premise_cf": trace.effective_premise_cf,
                "yielded_cf": trace.yielded_cf,
                "prior_cf": trace.prior_hypothesis_cf,
                "posterior_cf": trace.posterior_hypothesis_cf,
                "reasoning_mechanism": trace.explanation_how,
                "facts": facts_detail,
            })

        return {
            "pathology_id": pathology_id,
            "name": pathology.name,
            "latin_name": pathology.latin_name,
            "final_cf": hyp.cf,
            "criticality": pathology.criticality,
            "steps": steps,
        }

    def build_how_tree(
        self,
        pathology_id: str,
        hypotheses: Dict[str, Hypothesis],
        memory: WorkingMemory,
    ) -> Tree:
        """HOW sorğusu üçün Rich Tree formatında deduktiv sübut ağacı qurur."""
        how_data = self.explain_how(pathology_id, hypotheses, memory)
        if "error" in how_data:
            return Tree(f"[bold red]HOW Xətası: {how_data['error']}[/bold red]")

        final_cf = how_data["final_cf"]
        color = "green" if final_cf > 0.6 else "yellow" if final_cf > 0.2 else "red"

        root = Tree(
            f"[bold {color}]Diaqnoz: {how_data['name']} ({how_data.get('latin_name') or ''}) "
            f"[CF = {final_cf:+.2f}][/bold {color}]"
        )

        steps = how_data.get("steps", [])
        if not steps:
            root.add("[italic dim]Aktivləşmiş qayda yoxdur (aprior vəziyyət)[/italic dim]")
            return root

        for idx, step in enumerate(steps, start=1):
            rule_node = root.add(
                f"[bold cyan]Addım {idx}: Qayda {step['rule_id']} (CF_qayda = {step['rule_cf']:+.2f})[/bold cyan] "
                f"-> Artım: [yellow]{step['yielded_cf']:+.2f}[/yellow] "
                f"(Yekun toplanan CF: [bold]{step['posterior_cf']:+.2f}[/bold])"
            )

            premise_branch = rule_node.add(
                f"[dim]İlkin şərtlər (Effektiv konyunksiya CF = {step['effective_premise_cf']:.2f}):[/dim]"
            )
            for f in step["facts"]:
                premise_branch.add(
                    f"[green]✔[/green] {f['symptom_name']} "
                    f"([white]ilkin CF = {f['stated_cf']:+.2f}[/white], "
                    f"effektiv çəki = {f['effective_cf']:.2f}, mənbə: {f['source']})"
                )

            rule_node.add(f"[italic magenta]Bioloji mexanizm:[/italic magenta] {step['reasoning_mechanism']}")

        return root
