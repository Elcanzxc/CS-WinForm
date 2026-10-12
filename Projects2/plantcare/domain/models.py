"""PlantCare OS — Domain Layer Data Models.

Defines immutable Domain entities with strict Pydantic v2 validation:
- PlantProfile
- Symptom
- Pathology & TherapeuticProtocol
- Rule & Antecedent
- Fact & WorkingMemory Evidence
- Hypothesis & RuleActivationTrace
"""

from __future__ import annotations
import time
from typing import List, Literal, Optional, Dict
from pydantic import BaseModel, Field, model_validator


# ============================================================================
# PLANT PROFILE
# ============================================================================

class PlantProfile(BaseModel):
    """Biological profile of a plant species with optimal environmental tolerances."""
    id: str = Field(..., description="Unique slug identifier (e.g., monstera_deliciosa)")
    name: str = Field(..., description="Full botanical and common name")
    family: str = Field(..., description="Botanical family")
    description: str = Field(..., description="Short biological description")
    optimal_humidity_min: float = Field(..., ge=0.0, le=100.0, description="Min RH %")
    optimal_humidity_max: float = Field(..., ge=0.0, le=100.0, description="Max RH %")
    optimal_temp_min: float = Field(..., description="Min safe temperature C")
    optimal_temp_max: float = Field(..., description="Max safe temperature C")
    optimal_dli_min: float = Field(..., ge=0.0, description="Min Daily Light Integral mol/m2/d")
    optimal_dli_max: float = Field(..., ge=0.0, description="Max Daily Light Integral mol/m2/d")
    optimal_ph_min: float = Field(..., ge=0.0, le=14.0, description="Min substrate pH")
    optimal_ph_max: float = Field(..., ge=0.0, le=14.0, description="Max substrate pH")
    vulnerabilities: List[str] = Field(default_factory=list, description="Common physiological vulnerabilities")

    model_config = {"frozen": True}


# ============================================================================
# SYMPTOM / OBSERVABLE EVIDENCE
# ============================================================================

OrganType = Literal["leaves", "stem", "roots", "petiole", "substrate", "whole_plant"]


class Symptom(BaseModel):
    """Observable phenotypic or environmental indicator."""
    id: str = Field(..., description="Unique symptom code (e.g. leaf_lower_yellowing)")
    name: str = Field(..., description="Descriptive human-readable symptom name")
    organ: OrganType = Field(..., description="Affected vegetative or substrate organ")
    description: str = Field(..., description="Detailed morphological description")
    question_text: str = Field(..., description="Diagnostic prompt question presented to user")
    why_explanation: str = Field(..., description="Explanation for WHY this question is informative")

    model_config = {"frozen": True}


# ============================================================================
# THERAPEUTIC PROTOCOL & PATHOLOGY
# ============================================================================

CriticalityLevel = Literal["low", "medium", "high", "critical"]
PathologyCategory = Literal[
    "pathogen_fungal",
    "pathogen_bacterial",
    "pest",
    "physiological",
    "nutritional",
    "environmental",
]


class TherapeuticProtocol(BaseModel):
    """Clinical agrotechnical protocol for pathology management."""
    emergency_measures: List[str] = Field(
        default_factory=list,
        description="Immediate actions required within 24 hours to halt necrosis",
    )
    microclimate_adjustments: List[str] = Field(
        default_factory=list,
        description="VPD, lighting, temperature, and substrate moisture corrections",
    )
    pharmacotherapy: List[str] = Field(
        default_factory=list,
        description="Active medicinal/chemical treatments (fungicides, acaricides, chelates)",
    )
    prognosis: str = Field(..., description="Clinical vitality prognosis if treated promptly")

    model_config = {"frozen": True}


class Pathology(BaseModel):
    """Target nosological entity or agrotechnical error."""
    id: str = Field(..., description="Unique pathology key (e.g. root_rot)")
    name: str = Field(..., description="Clinical disease or condition name")
    latin_name: Optional[str] = Field(None, description="Botanical/pathological latin taxonomy")
    category: PathologyCategory = Field(..., description="Etiological classification")
    criticality: CriticalityLevel = Field(..., description="Threat severity level to plant vitality")
    pathogenesis_mechanism: str = Field(..., description="Detailed physiological mechanism of disease")
    therapeutic_protocol: TherapeuticProtocol = Field(..., description="Actionable care protocol")

    model_config = {"frozen": True}


# ============================================================================
# RULE & ANTECEDENT
# ============================================================================

ConditionSign = Literal["present", "absent"]
RuleOperator = Literal["AND", "OR"]


class Antecedent(BaseModel):
    """Premise condition evaluated against working memory."""
    symptom_id: str = Field(..., description="Fact or symptom identifier to match")
    sign: ConditionSign = Field(
        default="present",
        description="'present' checks positive CF, 'absent' checks negative CF",
    )
    threshold: float = Field(
        default=0.2,
        ge=0.0,
        le=1.0,
        description="Minimum absolute certainty required for premise activation",
    )
    weight: float = Field(
        default=1.0,
        ge=0.0,
        le=1.0,
        description="Weight multiplier of this antecedent in composite premise",
    )

    model_config = {"frozen": True}


class Rule(BaseModel):
    """Production rule encoding expert knowledge: IF (Antecedents) THEN Consequent (CF_rule)."""
    id: str = Field(..., description="Unique rule code (e.g. RULE_RR_01)")
    name: str = Field(..., description="Short title describing rule rationale")
    target_species: List[str] = Field(
        default_factory=lambda: ["all"],
        description="Species IDs for which rule is applicable, or ['all']",
    )
    antecedents: List[Antecedent] = Field(..., min_length=1, description="Premise conditions")
    logic_operator: RuleOperator = Field(default="AND", description="Combination mode for premises")
    consequent_hypothesis: str = Field(..., description="Target pathology or intermediate fact ID")
    rule_cf: float = Field(..., ge=-1.0, le=1.0, description="MYCIN attenuation weight CF_rule in [-1.0, 1.0]")
    priority: int = Field(default=10, ge=0, description="Rule execution priority for conflict resolution")
    specificity: int = Field(default=0, ge=0, description="Rule specificity rank (defaults to antecedent count)")
    explanation_how: str = Field(..., description="Clinical reasoning chain explanation for HOW trace")

    @model_validator(mode="after")
    def populate_specificity(self) -> Rule:
        if self.specificity == 0:
            object.__setattr__(self, "specificity", len(self.antecedents))
        return self

    model_config = {"frozen": True}


# ============================================================================
# WORKING MEMORY FACTS & HISTORICAL TRACES
# ============================================================================

FactSource = Literal["user_input", "sensor_reading", "inference_engine", "default"]


class Fact(BaseModel):
    """Single asserted atomic fact with certainty degree."""
    symptom_id: str = Field(..., description="Key of the symptom or intermediate fact")
    cf: float = Field(..., ge=-1.0, le=1.0, description="Certainty factor in [-1.0, 1.0]")
    source: FactSource = Field(default="user_input", description="Origin of assertion")
    timestamp: float = Field(default_factory=time.time, description="Unix timestamp of assertion")
    step_number: int = Field(default=0, ge=0, description="Sequential dialogue / inference step")

    model_config = {"frozen": True}


class RuleActivationTrace(BaseModel):
    """Audit trace documenting an individual rule firing."""
    rule_id: str
    consequent: str
    rule_cf: float
    antecedent_cfs: Dict[str, float]
    effective_premise_cf: float
    yielded_cf: float
    prior_hypothesis_cf: float
    posterior_hypothesis_cf: float
    explanation_how: str
    timestamp: float = Field(default_factory=time.time)

    model_config = {"frozen": True}


class Hypothesis(BaseModel):
    """Working diagnostic hypothesis under differential evaluation."""
    pathology_id: str
    cf: float = Field(default=0.0, ge=-1.0, le=1.0)
    support_rules: List[str] = Field(default_factory=list)
    refuting_rules: List[str] = Field(default_factory=list)
    traces: List[RuleActivationTrace] = Field(default_factory=list)

    model_config = {"frozen": True}
