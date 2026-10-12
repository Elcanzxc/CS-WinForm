"""PlantCare OS — Knowledge Base Repository & Resource Resolver.

Loads declarative YAML specifications and resolves file paths across:
- Standalone PyInstaller frozen binaries (via sys._MEIPASS)
- Direct Python module execution
- Local development file trees
"""

from __future__ import annotations
import os
import sys
from pathlib import Path
from typing import Dict, List, Tuple
import yaml

from plantcare.domain.models import PlantProfile, Symptom, Pathology, Rule
from plantcare.engine.linter import KnowledgeLinter, LintIssue


def resolve_resource_path(filename: str) -> Path:
    """Finds resource file supporting both PyInstaller bundled mode and normal Python."""
    # 1. PyInstaller temporary folder
    if hasattr(sys, "_MEIPASS"):
        candidate = Path(sys._MEIPASS) / filename
        if candidate.exists():
            return candidate

    # 2. Directory containing the running executable
    exe_dir = Path(sys.executable).parent / filename
    if exe_dir.exists():
        return exe_dir

    # 3. Current Working Directory
    cwd_path = Path.cwd() / filename
    if cwd_path.exists():
        return cwd_path

    # 4. Project root relative to this file
    repo_root = Path(__file__).resolve().parent.parent.parent / filename
    if repo_root.exists():
        return repo_root

    raise FileNotFoundError(
        f"Knowledge Base file '{filename}' could not be located in sys._MEIPASS, "
        f"executable directory, CWD, or project root."
    )


class KnowledgeBase:
    """In-memory domain catalog loaded from declarative specification."""

    def __init__(
        self,
        plants: Dict[str, PlantProfile],
        symptoms: Dict[str, Symptom],
        pathologies: Dict[str, Pathology],
        rules: List[Rule],
    ):
        self.plants = plants
        self.symptoms = symptoms
        self.pathologies = pathologies
        self.rules = rules

    def lint(self) -> List[LintIssue]:
        """Runs static analysis over knowledge base."""
        linter = KnowledgeLinter(self.plants, self.symptoms, self.pathologies, self.rules)
        return linter.lint()


def load_knowledge_base(file_path: Optional[str | Path] = None) -> KnowledgeBase:
    """Parses knowledge base YAML and returns validated domain objects."""
    if file_path is None:
        target_path = resolve_resource_path("knowledge_base.yaml")
    else:
        target_path = Path(file_path)

    if not target_path.exists():
        raise FileNotFoundError(f"Knowledge Base not found at {target_path}")

    with open(target_path, "r", encoding="utf-8") as f:
        raw_data = yaml.safe_load(f)

    # 1. Load Plants
    plants: Dict[str, PlantProfile] = {}
    for p_raw in raw_data.get("plants", []):
        p = PlantProfile(**p_raw)
        plants[p.id] = p

    # 2. Load Symptoms
    symptoms: Dict[str, Symptom] = {}
    for s_raw in raw_data.get("symptoms", []):
        s = Symptom(**s_raw)
        symptoms[s.id] = s

    # 3. Load Pathologies
    pathologies: Dict[str, Pathology] = {}
    for path_raw in raw_data.get("pathologies", []):
        pathology = Pathology(**path_raw)
        pathologies[pathology.id] = pathology

    # 4. Load Rules
    rules: List[Rule] = []
    for r_raw in raw_data.get("rules", []):
        r = Rule(**r_raw)
        rules.append(r)

    return KnowledgeBase(
        plants=plants,
        symptoms=symptoms,
        pathologies=pathologies,
        rules=rules,
    )
