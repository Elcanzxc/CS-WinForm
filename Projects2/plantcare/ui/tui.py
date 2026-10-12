"""PlantCare OS — Terminal İstifadəçi İnterfeysi (Rich TUI).

Azərbaycan dilində interaktiv kliniki terminal paneli:
- Bitki növünün seçimi və mikroiqlim pasportunun göstərilməsi
- İnformasiya nəzəriyyəsinə əsaslanan adaptiv diaqnostik sorğu dövrü
- 'WHY' izahları və 'Truth Maintenance' (faktların geri götürülməsi)
- MYCIN Əminlik Əmsalı (CF) şkalaları ilə differensial diaqnoz cədvəli
- 'HOW' deduktiv sübut ağacı (DAG)
- Üçmərhələli terapevtik müalicə protokolları
"""

from __future__ import annotations
import sys
from typing import Dict, List, Optional
from rich.console import Console
from rich.panel import Panel
from rich.table import Table
from rich.text import Text
from rich.tree import Tree
from rich.prompt import Prompt

from plantcare.domain.models import PlantProfile, Symptom, Pathology, Hypothesis
from plantcare.engine.memory import WorkingMemory
from plantcare.engine.inference import InferenceEngine
from plantcare.engine.questioner import AdaptiveQuestioner
from plantcare.engine.explanation import ExplanationFacility
from plantcare.repository.loader import KnowledgeBase


ORGAN_TRANSLATIONS: Dict[str, str] = {
    "leaves": "Yarpaqlar",
    "stem": "Gövdə / Budaqlar",
    "roots": "Köklər",
    "petiole": "Yarpaq saplağı",
    "substrate": "Substrat / Torpaq",
    "whole_plant": "Bütün bitki",
}


def format_cf_gauge(cf: float, width: int = 16) -> Text:
    """ASCII formatında rəngli Əminlik Əmsalı (CF) şkalasını [-1.0 ... +1.0] generasiya edir."""
    if cf >= 0:
        filled = int(round(cf * width))
        empty = width - filled
        bar = "█" * filled + "░" * empty
        if cf >= 0.75:
            color = "bold green"
        elif cf >= 0.4:
            color = "green"
        else:
            color = "yellow"
    else:
        abs_cf = abs(cf)
        filled = int(round(abs_cf * width))
        empty = width - filled
        bar = "█" * filled + "░" * empty
        color = "bold red"

    sign = "+" if cf > 0 else ""
    return Text(f"[{bar}] {sign}{cf:.2f}", style=color)


class PlantCareTUI:
    """İnteraktiv Kliniki Diaqnostika Paneli."""

    def __init__(self, kb: KnowledgeBase, console: Optional[Console] = None):
        self.kb = kb
        self.console = console or Console()
        self.engine = InferenceEngine(kb.rules, kb.pathologies)
        self.questioner = AdaptiveQuestioner(kb.symptoms, kb.rules, kb.pathologies, self.engine)
        self.explainer = ExplanationFacility(kb.symptoms, kb.rules, kb.pathologies)

    def print_banner(self) -> None:
        """Stilizə olunmuş başlıq panelini göstərir."""
        header_text = Text(
            "🌱 PlantCare OS: Hibrid İntellektual Agent və Simvolik Ekspert Sistemi\n"
            "   [Shortliffe / MYCIN Əminlik Cəbri • İnformasiya Nəzəriyyəsi ilə Fəal Sensorlaşma • DDD]",
            style="bold cyan",
            justify="center",
        )
        self.console.print(Panel(header_text, border_style="bright_green", expand=False))

    def select_plant(self, auto_plant_id: Optional[str] = None) -> PlantProfile:
        """İstifadəçiyə diaqnostika aparılacaq bitki növünü seçdirir."""
        if auto_plant_id and auto_plant_id in self.kb.plants:
            chosen = self.kb.plants[auto_plant_id]
            self._display_plant_card(chosen)
            return chosen

        table = Table(title="🌿 Diaqnostika üçün bitki növünü seçin", border_style="cyan")
        table.add_column("№", style="bold yellow", justify="center")
        table.add_column("Növ / Takson", style="bold white")
        table.add_column("Fəsilə", style="italic magenta")
        table.add_column("Optimal rütubət (RH)", justify="center")
        table.add_column("DLI (İşıq inteqralı)", justify="center")

        plant_list = list(self.kb.plants.values())
        for idx, p in enumerate(plant_list, start=1):
            table.add_row(
                str(idx),
                p.name,
                p.family,
                f"{p.optimal_humidity_min:.0f} - {p.optimal_humidity_max:.0f}%",
                f"{p.optimal_dli_min:.0f} - {p.optimal_dli_max:.0f} mol/m²/gün",
            )

        self.console.print(table)

        while True:
            choice = Prompt.ask(
                "[bold green]Bitkinin nömrəsini daxil edin [1-5][/bold green]",
                default="1",
                console=self.console,
            )
            try:
                c_idx = int(choice) - 1
                if 0 <= c_idx < len(plant_list):
                    selected = plant_list[c_idx]
                    self._display_plant_card(selected)
                    return selected
            except ValueError:
                pass
            self.console.print("[red]Yanlış seçim! Zəhmət olmasa cədvəldəki nömrələrdən birini seçin.[/red]")

    def _display_plant_card(self, plant: PlantProfile) -> None:
        """Bitkinin mikroiqlim pasportunu və bioloji xüsusiyyətlərini göstərir."""
        vuln_text = "\n".join(f" • [yellow]{v}[/yellow]" for v in plant.vulnerabilities)
        card_content = (
            f"[bold green]{plant.name}[/bold green] ([italic]{plant.family}[/italic])\n\n"
            f"[white]{plant.description}[/white]\n\n"
            f"[bold cyan]Mikroiqlim optimumu:[/bold cyan]\n"
            f" • Temperatur: {plant.optimal_temp_min}°C – {plant.optimal_temp_max}°C\n"
            f" • Havanın nisbi rütubəti (RH): {plant.optimal_humidity_min:.0f}% – {plant.optimal_humidity_max:.0f}%\n"
            f" • İşıq inteqralı (DLI): {plant.optimal_dli_min:.0f} – {plant.optimal_dli_max:.0f} mol/m²/gün\n"
            f" • Torpağın pH göstəricisi: {plant.optimal_ph_min:.1f} – {plant.optimal_ph_max:.1f}\n\n"
            f"[bold red]Növün spesifik riskləri və həssaslıqları:[/bold red]\n{vuln_text}"
        )
        self.console.print(Panel(card_content, title="📋 Bitkinin Pasportu", border_style="cyan"))

    def run_consultation(
        self,
        plant: PlantProfile,
        max_questions: int = 12,
        confidence_stopping_threshold: float = 0.90,
        mock_answers: Optional[Dict[str, float]] = None,
    ) -> tuple[Dict[str, Hypothesis], WorkingMemory]:
        """Aktiv sensorlaşma əks-əlaqə dövrünü (PEAS Agent Cycle) icra edir."""
        memory = WorkingMemory(plant_id=plant.id)
        step = 0

        self.console.print(
            "\n[bold bright_yellow]🔍 Adaptiv diaqnostik sorğu dövrü başladılır...[/bold bright_yellow]"
        )
        self.console.print(
            "[dim]Əmrlər: 'why' (sualın bioloji əsası), 'retract <id>' (faktın geri götürülməsi), "
            "'status' (cari hipotezlər), 'finish' (sorğunu yekunlaşdır)[/dim]\n"
        )

        while step < max_questions:
            current_hyps = self.engine.run_inference(memory)
            ranked = self.engine.get_ranked_diagnoses(current_hyps)
            if ranked and ranked[0][1].cf >= confidence_stopping_threshold:
                top_p, top_h = ranked[0]
                self.console.print(
                    f"\n[bold green]🎯 Diaqnostik əminlik həddinə çatıldı: "
                    f"'{top_p.name}' (CF = {top_h.cf:+.2f}). Sorğu tamamlandı.[/bold green]"
                )
                break

            # Adaptiv sual seçimi (Maksimum İnformasiya Qazancı - EIG)
            next_q = self.questioner.select_next_question(memory)
            if not next_q:
                self.console.print("[dim]Bütün əlaqədar simptomlar araşdırıldı.[/dim]")
                break

            symptom, eig = next_q
            step += 1
            organ_az = ORGAN_TRANSLATIONS.get(symptom.organ, symptom.organ)

            self.console.print(
                Panel(
                    f"[bold white]{symptom.question_text}[/bold white]\n\n"
                    f"[dim]Tədqiq olunan orqan: [cyan]{organ_az}[/cyan] | İnformasiya qazancı (EIG): [yellow]{eig:.3f} bit[/yellow][/dim]",
                    title=f"❓ Sual {step} / {max_questions} [Kod: {symptom.id}]",
                    border_style="bright_blue",
                )
            )

            # Avtomatlaşdırılmış rejim üçün
            if mock_answers is not None and symptom.id in mock_answers:
                chosen_cf = mock_answers[symptom.id]
                self.console.print(f"[bold cyan]> Avto-cavab: {chosen_cf:+.2f}[/bold cyan]")
                memory.assert_fact(symptom.id, chosen_cf, source="user_input")
                continue

            # İnteraktiv sorğu
            while True:
                self.console.print(
                    "[bold]1[/bold]: Bəli, dəqiq (+1.0)  |  "
                    "[bold]2[/bold]: Çox güman (+0.6)  |  "
                    "[bold]3[/bold]: Əmin deyiləm (0.0)\n"
                    "[bold]4[/bold]: Çox güman xeyr (-0.6) | "
                    "[bold]5[/bold]: Qətiyyən xeyr (-1.0) | "
                    "[bold]why[/bold]: Niyə bu sual? |  "
                    "[bold]skip[/bold]: Keç"
                )
                user_in = Prompt.ask("[bold green]Cavabınız[/bold green]", default="1", console=self.console).strip().lower()

                if user_in in ("why", "?"):
                    why_data = self.explainer.explain_why(symptom.id, memory, current_hyps)
                    self._display_why(why_data)
                    continue

                if user_in == "status":
                    self.display_hypotheses_table(current_hyps)
                    continue

                if user_in.startswith("retract "):
                    parts = user_in.split(maxsplit=1)
                    if len(parts) == 2:
                        target_sid = parts[1].strip()
                        retracted = memory.retract_fact(target_sid)
                        if retracted:
                            self.console.print(f"[bold yellow]↺ '{target_sid}' faktı işçi yaddaşdan ləğv edildi (geri götürüldü).[/bold yellow]")
                        else:
                            self.console.print(f"[red]'{target_sid}' faktı aktiv yaddaşda tapılmadı.[/red]")
                    continue

                if user_in == "finish":
                    self.console.print("[yellow]İstifadəçinin tələbi ilə sorğu yekunlaşdırıldı.[/yellow]")
                    return self.engine.run_inference(memory), memory

                if user_in == "skip":
                    memory.assert_fact(symptom.id, 0.0, source="user_input")
                    break

                cf_mapping = {
                    "1": 1.0,
                    "beli": 1.0,
                    "bəli": 1.0,
                    "yes": 1.0,
                    "y": 1.0,
                    "2": 0.6,
                    "3": 0.0,
                    "0": 0.0,
                    "bilmirem": 0.0,
                    "bilmirəm": 0.0,
                    "unknown": 0.0,
                    "4": -0.6,
                    "5": -1.0,
                    "xeyr": -1.0,
                    "yox": -1.0,
                    "no": -1.0,
                    "n": -1.0,
                }

                if user_in in cf_mapping:
                    chosen_cf = cf_mapping[user_in]
                    memory.assert_fact(symptom.id, chosen_cf, source="user_input")
                    break

                try:
                    val = float(user_in)
                    if -1.0 <= val <= 1.0:
                        memory.assert_fact(symptom.id, val, source="user_input")
                        break
                except ValueError:
                    pass

                self.console.print("[red]Yanlış daxiletmə! 1-5 rəqəmlərindən birini seçin və ya 'why' yazın.[/red]")

        final_hyps = self.engine.run_inference(memory)
        return final_hyps, memory

    def _display_why(self, why_data: Dict[str, object]) -> None:
        """WHY izah kartını göstərir."""
        bio_why = why_data.get("biological_rationale", "")
        hyp_list = why_data.get("affected_hypotheses", [])
        rules_list = why_data.get("awaiting_rules", [])

        hyp_lines = "\n".join(
            f" • [white]{h['name']}[/white]: [bold]{h['cf']:+.2f}[/bold]"
            for h in hyp_list
        ) or " • [dim]Aktiv hipotez yoxdur[/dim]"

        rule_lines = "\n".join(
            f" • [cyan]{r['rule_id']}[/cyan] ({r['name']}) -> [yellow]{r['target']}[/yellow] [CF_qayda={r['rule_cf']:+.2f}]"
            for r in rules_list
        ) or " • [dim]Gözləyən qayda yoxdur[/dim]"

        content = (
            f"[bold magenta]Bioloji əsaslandırma:[/bold magenta]\n{bio_why}\n\n"
            f"[bold cyan]Cari aparıcı hipotezlər:[/bold cyan]\n{hyp_lines}\n\n"
            f"[bold yellow]Bu simptomun cavabını gözləyən qaydalar:[/bold yellow]\n{rule_lines}"
        )
        self.console.print(Panel(content, title="💡 İzah Modulu: NİYƏ bu sual verilir? (WHY-FACILITY)", border_style="magenta"))

    def display_hypotheses_table(self, hypotheses: Dict[str, Hypothesis]) -> None:
        """Differensial diaqnoz cədvəlini çıxarır."""
        ranked = self.engine.get_ranked_diagnoses(hypotheses)

        table = Table(title="📊 Differensial Diaqnoz (MYCIN Əminlik Əmsalları)", border_style="green")
        table.add_column("Ranq", justify="center", style="bold yellow")
        table.add_column("Patologiya / Aqrotexniki xəstəlik", style="bold white")
        table.add_column("Kateqoriya", style="italic cyan")
        table.add_column("Təhlükə səviyyəsi", justify="center")
        table.add_column("Əminlik dərəcəsi (CF)", justify="left")

        crit_colors = {
            "critical": "[bold red]KRİTİK[/bold red]",
            "high": "[bold orange1]YÜKSƏK[/bold orange1]",
            "medium": "[yellow]ORTA[/yellow]",
            "low": "[blue]AŞAĞI[/blue]",
        }

        for idx, (pathology, hyp) in enumerate(ranked[:8], start=1):
            crit_badge = crit_colors.get(pathology.criticality, pathology.criticality)
            gauge = format_cf_gauge(hyp.cf)
            table.add_row(
                str(idx),
                pathology.name,
                pathology.category,
                crit_badge,
                gauge,
            )

        self.console.print(table)

    def display_full_report(
        self,
        plant: PlantProfile,
        hypotheses: Dict[str, Hypothesis],
        memory: WorkingMemory,
    ) -> None:
        """Kliniki yekun hesabatı ekrana çıxarır."""
        self.console.print("\n" + "=" * 80)
        self.console.print(f"[bold green]🏁 YEKUN KLİNİKİ HESABAT: {plant.name}[/bold green]", justify="center")
        self.console.print("=" * 80 + "\n")

        self.display_hypotheses_table(hypotheses)

        ranked = self.engine.get_ranked_diagnoses(hypotheses)
        if not ranked or ranked[0][1].cf <= 0.2:
            self.console.print(
                Panel(
                    "[bold green]Ağır patologiya əlamətləri aşkar edilmədi.[/bold green]\n"
                    "Bitkinin növ pasportuna uyğun standart mikroiqlim qayğısına davam etmək tövsiyə olunur.",
                    title="🟢 Diaqnostika Nəticəsi",
                    border_style="green",
                )
            )
            return

        top_pathology, top_hyp = ranked[0]

        # 1. HOW deduktiv sübut ağacı
        self.console.print("\n[bold cyan]🔎 DEDUKTİV SÜBUT AĞACI (HOW-FACILITY):[/bold cyan]")
        how_tree = self.explainer.build_how_tree(top_pathology.id, hypotheses, memory)
        self.console.print(how_tree)

        # 2. Terapevtik Protokol
        proto = top_pathology.therapeutic_protocol
        emergency_text = "\n".join(f" [bold red]•[/bold red] {m}" for m in proto.emergency_measures)
        micro_text = "\n".join(f" [bold cyan]•[/bold cyan] {m}" for m in proto.microclimate_adjustments)
        pharm_text = "\n".join(f" [bold green]•[/bold green] {m}" for m in proto.pharmacotherapy)

        protocol_card = (
            f"[bold red]🚨 TƏCİLİ TƏDBİRLƏR (İLK 24 SAAT):[/bold red]\n{emergency_text}\n\n"
            f"[bold cyan]🌡 MİKROİQLİM VƏ SUBSTRAT KORREKSİYASI:[/bold cyan]\n{micro_text}\n\n"
            f"[bold green]💊 FARMAKOTERAPİYA VƏ AQROKİMYA:[/bold green]\n{pharm_text}\n\n"
            f"[bold yellow]📈 KLİNİKİ PROQNOZ:[/bold yellow] {proto.prognosis}"
        )

        self.console.print(
            Panel(
                protocol_card,
                title=f"📋 Terapevtik Protokol: {top_pathology.name}",
                border_style="red" if top_pathology.criticality in ("critical", "high") else "yellow",
            )
        )
