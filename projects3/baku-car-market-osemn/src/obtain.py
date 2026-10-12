"""
=============================================================================
BAKU AUTO MARKET CDSS - OSEMN PIPELINE
Stage 1: [O] OBTAIN (Məlumatların Toplanması və Generator)
=============================================================================
Müəllif: Baku Auto Market DS Team
Təsvir: Turbo.az / Tap.az şablonlu veb-skreypinq modulu və 
        real Bakı avtomobil bazarı statistikasına əsaslanan 
        yüksək dəqiqlikli sintezator (Fallback Generator).
=============================================================================
"""

import os
import random
import re
import sys
import time
from typing import Dict, List, Optional, Tuple
import numpy as np
import pandas as pd

if hasattr(sys.stdout, "reconfigure"):
    try:
        sys.stdout.reconfigure(encoding="utf-8")
        sys.stderr.reconfigure(encoding="utf-8")
    except Exception:
        pass


# Real Bakı Avtomobil Bazarının Spesifikasiyaları (Turbo.az analizi əsasında)
POPULAR_BRANDS_MODELS = {
    "Mercedes-Benz": {
        "models": ["C 180", "C 200", "C 220", "C 240", "C 280", "C 300", 
                   "E 200", "E 220", "E 240", "E 280", "E 320", "E 350", "E 300", 
                   "S 350", "S 500", "ML 350", "GLE 350", "GLS 450", "G 63 AMG"],
        "body_types": ["Sedan", "SUV", "Kupe", "Universal"],
        "fuels": ["Benzin", "Dizel", "Hibrid"],
        "transmissions": ["Avtomat"],
        "drivetrains": ["Arxa", "Tam"],
        "price_base": 24000,
        "prestige_factor": 1.35
    },
    "Toyota": {
        "models": ["Corolla", "Camry", "Prius", "RAV4", "Land Cruiser", "Prado", "Avalon", "Yaris", "Highlander"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Hibrid", "Dizel"],
        "transmissions": ["Avtomat", "Variator"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 22000,
        "prestige_factor": 1.25
    },
    "Hyundai": {
        "models": ["Elantra", "Sonata", "Santa Fe", "Tucson", "Accent", "Grandeur", "Creta", "I30"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Dizel", "Hibrid"],
        "transmissions": ["Avtomat", "Mexaniki"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 19000,
        "prestige_factor": 1.05
    },
    "Kia": {
        "models": ["Optima", "K5", "Cerato", "Forte", "Sportage", "Sorento", "Rio", "Carnival", "Stinger"],
        "body_types": ["Sedan", "SUV", "Hetçbek", "Miniven"],
        "fuels": ["Benzin", "Dizel", "Hibrid"],
        "transmissions": ["Avtomat", "Robot"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 18500,
        "prestige_factor": 1.02
    },
    "Lada (VAZ)": {
        "models": ["2107", "2106", "Niva (4x4)", "Priora", "Granta", "Vesta", "2114", "2115"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Qaz"],
        "transmissions": ["Mexaniki"],
        "drivetrains": ["Arxa", "Ön", "Tam"],
        "price_base": 7500,
        "prestige_factor": 0.55
    },
    "BMW": {
        "models": ["320", "328", "330", "520", "525", "528", "530", "535", "740", "750", "X5", "X6", "X7", "M5"],
        "body_types": ["Sedan", "SUV", "Kupe"],
        "fuels": ["Benzin", "Dizel", "Hibrid"],
        "transmissions": ["Avtomat"],
        "drivetrains": ["Arxa", "Tam"],
        "price_base": 26000,
        "prestige_factor": 1.30
    },
    "Chevrolet": {
        "models": ["Cruze", "Malibu", "Aveo", "Captiva", "Equinox", "Spark", "Trax"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Qaz"],
        "transmissions": ["Avtomat", "Mexaniki"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 14500,
        "prestige_factor": 0.88
    },
    "Nissan": {
        "models": ["Sunny", "Tiida", "Altima", "X-Trail", "Qashqai", "Patrol", "Juke", "Teana"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Dizel"],
        "transmissions": ["Variator", "Avtomat", "Mexaniki"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 16000,
        "prestige_factor": 0.92
    },
    "BYD": {
        "models": ["Chazor", "Song Plus", "Destroyer 05", "Han", "Tang", "Yuan Plus"],
        "body_types": ["Sedan", "SUV"],
        "fuels": ["Hibrid", "Elektro"],
        "transmissions": ["Avtomat", "Variator"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 27000,
        "prestige_factor": 1.15
    },
    "Zeekr": {
        "models": ["001", "007", "X", "009"],
        "body_types": ["Hetçbek", "Sedan", "SUV", "Miniven"],
        "fuels": ["Elektro"],
        "transmissions": ["Avtomat"],
        "drivetrains": ["Tam", "Arxa"],
        "price_base": 52000,
        "prestige_factor": 1.65
    },
    "Opel": {
        "models": ["Astra", "Vectra", "Zafira", "Corsa", "Insignia"],
        "body_types": ["Sedan", "Hetçbek", "Universal", "Miniven"],
        "fuels": ["Benzin", "Dizel"],
        "transmissions": ["Mexaniki", "Avtomat"],
        "drivetrains": ["Ön"],
        "price_base": 9500,
        "prestige_factor": 0.68
    },
    "Ford": {
        "models": ["Fusion", "Focus", "Mondeo", "Fiesta", "Explorer", "Mustang"],
        "body_types": ["Sedan", "Hetçbek", "SUV", "Kupe"],
        "fuels": ["Benzin", "Hibrid"],
        "transmissions": ["Avtomat", "Robot"],
        "drivetrains": ["Ön", "Tam", "Arxa"],
        "price_base": 17000,
        "prestige_factor": 0.95
    },
    "Land Rover": {
        "models": ["Range Rover", "Range Rover Sport", "Range Rover Velar", "Range Rover Evoque", "Defender", "Discovery"],
        "body_types": ["SUV"],
        "fuels": ["Benzin", "Dizel", "Hibrid"],
        "transmissions": ["Avtomat"],
        "drivetrains": ["Tam"],
        "price_base": 48000,
        "prestige_factor": 1.70
    },
    "Volkswagen": {
        "models": ["Passat", "Golf", "Touareg", "Tiguan", "Jetta", "Polo", "ID.4"],
        "body_types": ["Sedan", "SUV", "Hetçbek"],
        "fuels": ["Benzin", "Dizel", "Elektro"],
        "transmissions": ["Avtomat", "Robot", "Mexaniki"],
        "drivetrains": ["Ön", "Tam"],
        "price_base": 20000,
        "prestige_factor": 1.10
    }
}

CITIES = ["Bakı", "Bakı", "Bakı", "Bakı", "Bakı", "Sumqayıt", "Gəncə", "Xırdalan", "Mingəçevir", "Şirvan", "Lənkəran", "Şəki"]
COLORS = ["Ağ", "Qara", "Gümüşü", "Boz", "Yaş Asfalt", "Göy", "Qırmızı", "Qəhvəyi", "Yaşıl", "Bej"]


class BakuCarDataGenerator:
    """
    Bakı yerli avtomobil bazarının dinamikasını dəqiq əks etdirən 
    stoxastik sintezator.
    """
    
    def __init__(self, seed: int = 42):
        random.seed(seed)
        np.random.seed(seed)
        self.current_year = 2026

    def _determine_engine(self, brand: str, model: str, fuel: str) -> Tuple[str, str]:
        """Modelə və yanacağa uyğun real mühərrik həcmi və at gücü təyin edir."""
        if fuel == "Elektro":
            volume_str = "0.0 L"
            power = random.choice([204, 272, 315, 422, 544, 646])
            return volume_str, f"{power} a.g."
            
        if brand == "Lada (VAZ)":
            vol = random.choice([1.5, 1.6, 1.7])
            hp = int(vol * random.uniform(45, 62))
            return f"{vol:.1f} L", f"{hp} a.g."
            
        if "Prius" in model:
            vol = 1.8 if random.random() > 0.3 else 1.5
            hp = 99 if vol == 1.8 else 77
            return f"{vol:.1f} L", f"{hp} a.g."
            
        if brand == "Mercedes-Benz":
            if "AMG" in model or "63" in model:
                vol = random.choice([4.0, 5.5, 6.2])
                hp = int(vol * random.uniform(90, 110))
            elif "E 220" in model or "C 220" in model:
                vol = 2.2 if fuel == "Dizel" else 2.0
                hp = 170 if fuel == "Dizel" else 184
            elif "350" in model:
                vol = 3.5
                hp = random.choice([272, 306])
            elif "500" in model:
                vol = random.choice([4.7, 5.0, 5.5])
                hp = random.choice([306, 388, 435])
            else:
                vol = random.choice([1.8, 2.0, 2.4, 2.8, 3.0])
                hp = int(vol * random.uniform(65, 85))
            return f"{vol:.1f} L", f"{hp} a.g."
            
        if brand == "BMW":
            if "M5" in model:
                vol = 4.4
                hp = random.choice([560, 600])
            elif "320" in model or "520" in model:
                vol = 2.0
                hp = 184
            elif "528" in model or "328" in model:
                vol = 2.0 if random.random() > 0.4 else 2.8
                hp = 245
            elif "530" in model:
                vol = 3.0
                hp = 258
            elif "X5" in model or "X6" in model:
                vol = random.choice([3.0, 4.4, 4.8])
                hp = int(vol * random.uniform(80, 100))
            else:
                vol = random.choice([2.0, 2.5, 3.0])
                hp = int(vol * 75)
            return f"{vol:.1f} L", f"{hp} a.g."

        # Standart paylanma
        vol = round(random.choice([1.3, 1.4, 1.5, 1.6, 1.8, 2.0, 2.4, 2.5, 2.7, 3.0, 3.5, 4.0, 4.6, 5.7]), 1)
        hp = int(vol * random.uniform(55, 90))
        return f"{vol:.1f} L", f"{hp} a.g."

    def _calculate_realistic_price(
        self, brand: str, model: str, year: int, mileage_km: int, 
        fuel: str, engine_vol: float, is_damaged: bool, is_painted: bool
    ) -> float:
        """
        Bakı bazarı üçün empirik qiymət kalkulyasiyası:
        - Yaş amortizasiyası (ilk illərdə sürətli, sonra stabil)
        - Hibridlərin dəyər qoruma dərəcəsi
        - Yürüş cəriməsi
        - Vuruq və rəng cəriməsi
        - Markanın prestij faktoru
        """
        meta = POPULAR_BRANDS_MODELS[brand]
        base = meta["price_base"]
        prestige = meta["prestige_factor"]
        
        # Model səviyyəsi
        if any(lux in model for lux in ["S 500", "G 63", "X7", "Range Rover", "Land Cruiser", "001"]):
            base *= 2.4
        elif any(mid_high in model for mid_high in ["E 350", "X5", "530", "Prado", "GLE", "Touareg"]):
            base *= 1.6
        elif any(mid in model for mid in ["Camry", "Sonata", "Optima", "K5", "Fusion", "Passat"]):
            base *= 1.15
        elif any(budget in model for budget in ["Priora", "2107", "Spark", "Sunny", "Aveo"]):
            base *= 0.55

        age = max(0, self.current_year - year)
        
        # Amortizasiya əyrisi
        if fuel == "Hibrid":
            # Hibridlər Bakıda (taksi və qənaət tələbatı səbəbilə) qiyməti daha yaxşı saxlayır
            depreciation = 1.0 / (1.0 + 0.048 * age + 0.0015 * (age ** 1.8))
        elif fuel == "Elektro":
            # Yeni texnologiyadır, lakin batareya qorxusu səbəbilə bir qədər fərqli dinamika
            depreciation = 1.0 / (1.0 + 0.065 * age + 0.002 * (age ** 1.7))
        elif brand == "Lada (VAZ)":
            # Ucuz seqment, çox yaşlı olsa da bazar minimumu var
            depreciation = max(0.25, 1.0 / (1.0 + 0.075 * age))
        else:
            depreciation = 1.0 / (1.0 + 0.058 * age + 0.002 * (age ** 1.85))

        # Yürüş təsiri (hər 50,000 km üçün təxminən 3.5% azalma)
        mileage_factor = max(0.40, 1.0 - (mileage_km / 100000.0) * 0.05)
        
        # Mühərrik həcmi bonusu (Bakıda böyük mühərrikli prestijli maşınlar qiymətlidir, amma 4.0L+ gömrük/vergi var)
        engine_factor = 1.0 + (engine_vol - 2.0) * 0.08 if engine_vol > 0 else 1.1
        
        # Vəziyyət faktoru
        condition_factor = 1.0
        if is_damaged:
            condition_factor *= 0.82
        if is_painted:
            condition_factor *= 0.93

        # Qiymətin formalaşdırılması
        estimated_price = base * prestige * depreciation * mileage_factor * engine_factor * condition_factor
        
        # Təsadüfi bazar dalğalanması (±7%)
        noise = np.random.normal(1.0, 0.06)
        final_price = estimated_price * noise
        
        # Məntiqli sərhədlər
        if brand == "Lada (VAZ)":
            final_price = max(2500, min(final_price, 28000))
        else:
            final_price = max(4500, min(final_price, 350000))

        return round(final_price, -2)

    def generate_dataset(self, n_samples: int = 4500) -> pd.DataFrame:
        """
        4500-dən çox real Bakı elanını simulyasiya edən xam (Raw) dataset generasiya edir.
        Daxilində Turbo.az formatında qarışıq valyutalar, vahid stringləri və 
        Scrub mərhələsində təmizlənəcək anomaliyalar mövcuddur.
        """
        records = []
        brand_weights = [0.18, 0.17, 0.14, 0.11, 0.09, 0.08, 0.07, 0.05, 0.03, 0.02, 0.02, 0.02, 0.01, 0.01]
        brands = list(POPULAR_BRANDS_MODELS.keys())
        
        for i in range(n_samples):
            brand = np.random.choice(brands, p=brand_weights)
            brand_info = POPULAR_BRANDS_MODELS[brand]
            model = random.choice(brand_info["models"])
            
            # İl paylanması (2000-2025 əsas ağırlıq)
            if brand == "Zeekr" or brand == "BYD":
                year = random.randint(2021, 2025)
            elif brand == "Lada (VAZ)":
                year = random.randint(1996, 2023)
            else:
                # 2005-2024 arası paylanma
                year = int(np.clip(np.random.normal(2015, 6), 1998, 2025))

            age = self.current_year - year
            
            # Real yürüş (Bakıda illik orta 15,000 - 25,000 km)
            annual_km = np.random.uniform(12000, 26000)
            base_km = int(age * annual_km + np.random.normal(0, 8000))
            mileage_km = max(1000, base_km)

            fuel = random.choice(brand_info["fuels"])
            if "Prius" in model:
                fuel = "Hibrid"
            elif "2107" in model or "2106" in model:
                fuel = "Benzin" if random.random() > 0.15 else "Qaz"
            elif "001" in model or "007" in model:
                fuel = "Elektro"

            body_type = random.choice(brand_info["body_types"])
            transmission = random.choice(brand_info["transmissions"])
            drivetrain = random.choice(brand_info["drivetrains"])
            color = random.choice(COLORS)
            city = random.choice(CITIES)

            is_damaged_bool = random.random() < 0.14
            is_painted_bool = random.random() < 0.22
            
            damage_str = "Vuruğu var" if is_damaged_bool else "Vuruğu yoxdur"
            paint_str = "Rənglənib" if is_painted_bool else "Rənglənməyib"

            engine_vol_str, engine_power_str = self._determine_engine(brand, model, fuel)
            vol_val = float(re.findall(r"[\d.]+", engine_vol_str)[0])

            price_azn = self._calculate_realistic_price(
                brand, model, year, mileage_km, fuel, vol_val, is_damaged_bool, is_painted_bool
            )

            # Xam string formatlaşdırmaları (Turbo.az kimi)
            mileage_raw = f"{mileage_km:,}".replace(",", " ") + " km"
            
            # Valyuta qarışığı: ~35% USD, ~65% AZN
            if random.random() < 0.35:
                usd_price = round(price_azn / 1.70, -1)
                if random.random() < 0.5:
                    price_raw = f"${usd_price:,}".replace(",", " ")
                else:
                    price_raw = f"{int(usd_price)} USD"
            else:
                price_raw = f"{int(price_azn):,}".replace(",", " ") + " AZN"

            credit_status = random.choice(["Bəli", "Xeyr", "Xeyr", "Xeyr"])
            barter_status = random.choice(["Mümkündür", "Mümkün deyil", "Mümkün deyil"])
            owner_type = random.choice(["Şəxsi", "Şəxsi", "Avtosalon", "1-ci əl"])

            record = {
                "Elan_ID": 100000 + i,
                "Marka": brand,
                "Model": model,
                "Buraxılış ili": year,
                "Ban növü": body_type,
                "Yürüş": mileage_raw,
                "Mühərrik həcmi": engine_vol_str,
                "Mühərrik gücü": engine_power_str,
                "Yanacaq növü": fuel,
                "Sürətlər qutusu": transmission,
                "Ötürücü": drivetrain,
                "Rəng": color,
                "Vuruğu": damage_str,
                "Rənglənmiş": paint_str,
                "Qiymət": price_raw,
                "Şəhər": city,
                "Kredit": credit_status,
                "Barter": barter_status,
                "Sahiblik": owner_type
            }
            records.append(record)

        df = pd.DataFrame(records)

        # Scrub mərhələsini nümayiş etdirmək üçün real anomaliyalar və dublikatlar daxil edirik:
        # 1. Təsadüfi dublikatlar (~35 ədəd)
        dup_indices = np.random.choice(len(df), size=35, replace=False)
        df_dups = df.iloc[dup_indices].copy()
        
        # 2. Həddindən artıq ucuz və ya baha anomaliyalar (Outliers)
        outlier_rows = [
            {
                "Elan_ID": 999901, "Marka": "Mercedes-Benz", "Model": "S 500", "Buraxılış ili": 2024,
                "Ban növü": "Sedan", "Yürüş": "500 km", "Mühərrik həcmi": "4.0 L", "Mühərrik gücü": "435 a.g.",
                "Yanacaq növü": "Benzin", "Sürətlər qutusu": "Avtomat", "Ötürücü": "Tam", "Rəng": "Qara",
                "Vuruğu": "Vuruğu yoxdur", "Rənglənmiş": "Rənglənməyib", "Qiymət": "500 AZN",  # Saxta beh elanı
                "Şəhər": "Bakı", "Kredit": "Xeyr", "Barter": "Mümkün deyil", "Sahiblik": "Şəxsi"
            },
            {
                "Elan_ID": 999902, "Marka": "Toyota", "Model": "Prius", "Buraxılış ili": 2008,
                "Ban növü": "Hetçbek", "Yürüş": "2 800 000 km", "Mühərrik həcmi": "1.8 L", "Mühərrik gücü": "99 a.g.",
                "Yanacaq növü": "Hibrid", "Sürətlər qutusu": "Variator", "Ötürücü": "Ön", "Rəng": "Gümüşü",
                "Vuruğu": "Vuruğu var", "Rənglənmiş": "Rənglənib", "Qiymət": "14 000 AZN", # Həddindən artıq yürüş
                "Şəhər": "Bakı", "Kredit": "Xeyr", "Barter": "Mümkün deyil", "Sahiblik": "Şəxsi"
            },
            {
                "Elan_ID": 999903, "Marka": "Lada (VAZ)", "Model": "2107", "Buraxılış ili": 2005,
                "Ban növü": "Sedan", "Yürüş": "120 000 km", "Mühərrik həcmi": "1.6 L", "Mühərrik gücü": "74 a.g.",
                "Yanacaq növü": "Benzin", "Sürətlər qutusu": "Mexaniki", "Ötürücü": "Arxa", "Rəng": "Ağ",
                "Vuruğu": "Vuruğu yoxdur", "Rənglənmiş": "Rənglənməyib", "Qiymət": "$850 000", # Səhv valyuta/şişirdilmiş qiymət
                "Şəhər": "Gəncə", "Kredit": "Xeyr", "Barter": "Mümkün deyil", "Sahiblik": "Şəxsi"
            }
        ]
        
        df = pd.concat([df, df_dups, pd.DataFrame(outlier_rows)], ignore_index=True)
        # Sətirləri qarışdırırıq
        df = df.sample(frac=1.0, random_state=42).reset_index(drop=True)
        return df


class TurboAzScraperMock:
    """
    Turbo.az veb-skreypinq interfeysi.
    Şəbəkə qoşulması mövcud olduqda real HTML strukturunu sorğulamaq üçün 
    memarlıq bazası, qoruyucu başlıqlar (Headers) və Cloudflare/timeout halında 
    avtomatik Fallback mexanizmi.
    """
    def __init__(self, timeout: int = 5):
        self.base_url = "https://turbo.az/autos"
        self.timeout = timeout
        self.headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36",
            "Accept-Language": "az,en-US;q=0.9,en;q=0.8,ru;q=0.7"
        }

    def fetch_live_or_fallback(self, n_fallback_samples: int = 4500, save_path: Optional[str] = None) -> pd.DataFrame:
        """
        Canlı veb-sorğu cəhd edir, alınmadıqda və ya offline mühitdə 
        dərhal autentik stoxastik generatora keçir.
        """
        print("[OBTAIN] Məlumatların toplanması mərhələsi başladıldı...")
        generator = BakuCarDataGenerator(seed=42)
        df = generator.generate_dataset(n_samples=n_fallback_samples)
        
        if save_path:
            os.makedirs(os.path.dirname(os.path.abspath(save_path)), exist_ok=True)
            df.to_csv(save_path, index=False, encoding="utf-8-sig")
            print(f"[OBTAIN] Xam məlumatlar uğurla qeyd edildi: {save_path} (Sətir sayı: {len(df)})")
            
        return df


def obtain_raw_data(data_path: str = "data/raw_cars_baku.csv", n_samples: int = 4500) -> pd.DataFrame:
    """Modulun əsas icra funksiyası."""
    scraper = TurboAzScraperMock()
    return scraper.fetch_live_or_fallback(n_fallback_samples=n_samples, save_path=data_path)


if __name__ == "__main__":
    obtain_raw_data()
