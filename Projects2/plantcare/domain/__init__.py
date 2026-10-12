"""PlantCare OS — Domain Layer Exports."""

from plantcare.domain.models import (
    PlantProfile,
    Symptom,
    Pathology,
    TherapeuticProtocol,
    Antecedent,
    Rule,
    Fact,
    RuleActivationTrace,
    Hypothesis,
    OrganType,
    CriticalityLevel,
    PathologyCategory,
)
from plantcare.domain.certainty import (
    clamp_cf,
    combine_conjunction,
    combine_disjunction,
    activate_rule,
    combine_two_evidences,
    combine_multiple_evidences,
)

__all__ = [
    "PlantProfile",
    "Symptom",
    "Pathology",
    "TherapeuticProtocol",
    "Antecedent",
    "Rule",
    "Fact",
    "RuleActivationTrace",
    "Hypothesis",
    "OrganType",
    "CriticalityLevel",
    "PathologyCategory",
    "clamp_cf",
    "combine_conjunction",
    "combine_disjunction",
    "activate_rule",
    "combine_two_evidences",
    "combine_multiple_evidences",
]
