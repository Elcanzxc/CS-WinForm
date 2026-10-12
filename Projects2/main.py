"""PlantCare OS — Əsas Giriş Nöqtəsi (Main Entrypoint).

Dəstəklənən rejimlər:
- İnteraktiv kliniki məsləhət rejimi (Rich TUI)
- Avtomatlaşdırılmış etalon test və audit rejimi (--demo / --verify)
- Biliklər Bazasının statik yoxlanışı (--lint)
"""

from __future__ import annotations
import sys
import os
import argparse
from pathlib import Path

# Windows konsolunda UTF-8 kodlaşdırmasını təmin etmək
if sys.platform == "win32":
    try:
        sys.stdout.reconfigure(encoding="utf-8")
        sys.stderr.reconfigure(encoding="utf-8")
    except Exception:
        pass

from plantcare.repository.loader import load_knowledge_base
from plantcare.ui.tui import PlantCareTUI
from plantcare.engine.memory import WorkingMemory


def run_demo_trace(tui: PlantCareTUI) -> None:
    """Monstera Deliciosa Kök Çürüməsi etalon kliniki yoxlama ssenarisini icra edir."""
    tui.console.print("\n[bold cyan]🔬 ETALON VERİFİKASİYA SSENARİSİNİN İCRASI (E2E TEST TRACE)[/bold cyan]")
    tui.console.print(
        "[white]Kliniki keys: [bold yellow]Monstera deliciosa[/bold yellow] — aşağı yarpaqların saralması, "
        "qonur sulu ləkələr və daimi nəm substrat.[/white]\n"
    )

    plant = tui.kb.plants["monstera_deliciosa"]
    tui._display_plant_card(plant)

    memory = WorkingMemory(plant_id=plant.id)

    # 1. Addım 1: İlkin anamnezin daxil edilməsi
    tui.console.print("\n[bold yellow]─── ADDIM 1: İlkin anamnez və müşahidə ───[/bold yellow]")
    s1 = "leaf_spots_brown_moist"
    memory.assert_fact(s1, 1.0, source="user_input")
    tui.console.print(f"✔ Qeydə alınan simptom: [green]{tui.kb.symptoms[s1].name}[/green] (CF = +1.00)")

    # 2. Addım 2: İnformasiya nəzəriyyəsi ilə növbəti sual və WHY sorğusu
    tui.console.print("\n[bold yellow]─── ADDIM 2: İnformasiya qazancı ilə sual seçimi və WHY izahı ───[/bold yellow]")
    hyps_step1 = tui.engine.run_inference(memory)
    next_q = tui.questioner.select_next_question(memory)
    if next_q:
        sym, eig = next_q
        tui.console.print(f"❓ Seçilən sual: [bold white]{sym.name}[/bold white] (EIG = {eig:.3f} bit)")
        why_data = tui.explainer.explain_why(sym.id, memory, hyps_step1)
        tui._display_why(why_data)

    # 3. Addım 3: Kök çürüməsini təsdiqləyən əsas amillər
    tui.console.print("\n[bold yellow]─── ADDIM 3: Kliniki mənzərənin əsas amillərlə tamamlanması ───[/bold yellow]")
    s2 = "substrate_constantly_wet"
    s3 = "leaf_lower_yellowing"
    memory.assert_fact(s2, 1.0, source="user_input")
    tui.console.print(f"✔ Qeydə alınan simptom: [green]{tui.kb.symptoms[s2].name}[/green] (CF = +1.00)")
    memory.assert_fact(s3, 1.0, source="user_input")
    tui.console.print(f"✔ Qeydə alınan simptom: [green]{tui.kb.symptoms[s3].name}[/green] (CF = +1.00)")

    # 4. Addım 4: Truth Maintenance (Faktın ləğvi və inamın qeyri-monoton reviziyası)
    tui.console.print("\n[bold yellow]─── ADDIM 4: Truth Maintenance (İnamların qeyri-monoton reviziyası) ───[/bold yellow]")
    tui.console.print("Fərziyyəvi daxiletmə xətası: istifadəçi səhvən hörümçək toru gördüyünü bildirdi...")
    memory.assert_fact("webbing_microscopic", 1.0, source="user_input")
    hyps_with_mite = tui.engine.run_inference(memory)
    tui.console.print(f"Hörümçək gənəsinin müvəqqəti CF-i: [bold]{hyps_with_mite['spider_mite'].cf:+.2f}[/bold]")

    tui.console.print("İstifadəçi 'webbing_microscopic' faktını geri götürür (retract)...")
    memory.retract_fact("webbing_microscopic")
    hyps_after_retract = tui.engine.run_inference(memory)
    tui.console.print(f"Fakt geri götürüldükdən sonra gənənin CF-i: [bold]{hyps_after_retract['spider_mite'].cf:+.2f}[/bold] (Sıfırlandı)")

    # 5. Addım 5: Yekun Forward Chaining nəticəsi və HOW ağacı
    tui.console.print("\n[bold yellow]─── ADDIM 5: Yekun məntiqi nəticə və HOW deduktiv sübut ağacı ───[/bold yellow]")
    final_hyps = tui.engine.run_inference(memory)
    tui.display_full_report(plant, final_hyps, memory)


def main() -> None:
    parser = argparse.ArgumentParser(
        description="PlantCare OS: Hibrid İntellektual Agent və Simvolik Ekspert Sistemi"
    )
    parser.add_argument(
        "--demo", "--verify",
        action="store_true",
        dest="demo",
        help="Monstera keysi üzrə avtomatik etalon audit rejimini icra et",
    )
    parser.add_argument(
        "--lint",
        action="store_true",
        help="Biliklər Bazasının statik bütövlüyünü yoxla və çıx",
    )
    parser.add_argument(
        "--plant",
        type=str,
        default=None,
        help="Bitki növünü əvvəlcədən təyin et (məsələn: monstera_deliciosa)",
    )
    parser.add_argument(
        "--no-pause",
        action="store_true",
        help="Proqram bitdikdə Enter gözləmədən birbaşa çıx (skriptlər üçün)",
    )
    args = parser.parse_args()

    # Biliklər Bazasını yüklə
    kb = load_knowledge_base()

    if args.lint:
        issues = kb.lint()
        print(f"Biliklər Bazasının yoxlanış nəticəsi: {len(issues)} qeyd aşkar olundu.")
        has_error = False
        for issue in issues:
            print(str(issue))
            if issue.severity == "ERROR":
                has_error = True
        if not args.no_pause:
            input("\nÇıxmaq üçün Enter düyməsini basın... ")
        sys.exit(1 if has_error else 0)

    tui = PlantCareTUI(kb)
    tui.print_banner()

    try:
        if args.demo:
            run_demo_trace(tui)
        else:
            selected_plant = tui.select_plant(auto_plant_id=args.plant)
            final_hyps, memory = tui.run_consultation(selected_plant)
            tui.display_full_report(selected_plant, final_hyps, memory)
    except KeyboardInterrupt:
        tui.console.print("\n[yellow]Diaqnostika istifadəçi tərəfindən dayandırıldı.[/yellow]")
    except Exception as exc:
        tui.console.print(f"\n[bold red]Xəta baş verdi: {exc}[/bold red]")
    finally:
        # Konsol pəncərəsi birbaşa bağlanmasın deyə istifadəçidən Enter gözləyirik!
        if not args.no_pause:
            print()
            input("Proqramı bağlamaq üçün Enter düyməsini basın... [Press Enter to exit] ")


if __name__ == "__main__":
    main()
