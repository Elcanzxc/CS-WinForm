"""PlantCare OS — Working Memory & Truth Maintenance System (TMS).

Implements session state storage with non-monotonic belief revision,
fact versioning, audit logging, and state rollback/retraction.
"""

from __future__ import annotations
import copy
import time
from typing import Dict, List, Optional, Tuple
from plantcare.domain.models import Fact, FactSource


class MemorySnapshot:
    """Immutable state snapshot for rollback and transactional auditing."""
    def __init__(self, step: int, facts: Dict[str, Fact], plant_id: Optional[str]):
        self.step = step
        self.facts = dict(facts)
        self.plant_id = plant_id
        self.created_at = time.time()


class WorkingMemory:
    """Session working memory with full truth maintenance and non-monotonic revision."""

    def __init__(self, plant_id: Optional[str] = None):
        self._facts: Dict[str, Fact] = {}
        self._history: List[MemorySnapshot] = []
        self._step_counter: int = 0
        self._plant_id: Optional[str] = plant_id
        self._retracted_log: List[Tuple[int, str, Fact]] = []

    @property
    def plant_id(self) -> Optional[str]:
        return self._plant_id

    @plant_id.setter
    def plant_id(self, val: Optional[str]) -> None:
        self._plant_id = val

    @property
    def current_step(self) -> int:
        return self._step_counter

    def _save_snapshot(self) -> None:
        """Saves current state to history stack prior to mutation."""
        self._history.append(
            MemorySnapshot(
                step=self._step_counter,
                facts=self._facts,
                plant_id=self._plant_id,
            )
        )

    def assert_fact(
        self,
        symptom_id: str,
        cf: float,
        source: FactSource = "user_input",
    ) -> Fact:
        """Asserts or updates a belief about a symptom or environmental factor.

        Supports non-monotonic belief revision: overwriting a previously asserted
        symptom with a new certainty factor records the prior state for rollback.
        """
        self._save_snapshot()
        self._step_counter += 1

        fact = Fact(
            symptom_id=symptom_id,
            cf=cf,
            source=source,
            timestamp=time.time(),
            step_number=self._step_counter,
        )
        self._facts[symptom_id] = fact
        return fact

    def retract_fact(self, symptom_id: str) -> Optional[Fact]:
        """Retracts an asserted fact from working memory (Truth Maintenance).

        Removes the assertion and archives it into the retraction audit log.
        Returns the removed Fact if it existed, otherwise None.
        """
        if symptom_id not in self._facts:
            return None

        self._save_snapshot()
        self._step_counter += 1

        removed_fact = self._facts.pop(symptom_id)
        self._retracted_log.append((self._step_counter, symptom_id, removed_fact))
        return removed_fact

    def rollback(self) -> bool:
        """Rolls back working memory to the preceding snapshot.

        Returns True if rollback was successful, False if at genesis state.
        """
        if not self._history:
            return False

        last_snapshot = self._history.pop()
        self._step_counter = last_snapshot.step
        self._facts = dict(last_snapshot.facts)
        self._plant_id = last_snapshot.plant_id
        return True

    def get_fact(self, symptom_id: str) -> Optional[Fact]:
        """Retrieves fact by symptom ID if present in active memory."""
        return self._facts.get(symptom_id)

    def get_cf(self, symptom_id: str, default: float = 0.0) -> float:
        """Returns CF of symptom if asserted, else default."""
        fact = self._facts.get(symptom_id)
        return fact.cf if fact else default

    def has_fact(self, symptom_id: str) -> bool:
        """Checks if symptom has been affirmed or refuted."""
        return symptom_id in self._facts

    def all_facts(self) -> List[Fact]:
        """Returns ordered list of all asserted facts."""
        return sorted(self._facts.values(), key=lambda f: f.step_number)

    def clear(self) -> None:
        """Resets working memory to pristine empty state."""
        self._save_snapshot()
        self._facts.clear()
        self._step_counter = 0

    def clone(self) -> WorkingMemory:
        """Creates an independent deep-copy for hypothetical reasoning / simulation."""
        new_mem = WorkingMemory(plant_id=self._plant_id)
        new_mem._facts = copy.deepcopy(self._facts)
        new_mem._step_counter = self._step_counter
        return new_mem
