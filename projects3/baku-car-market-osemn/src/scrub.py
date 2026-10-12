"""
=============================================================================
BAKU AUTO MARKET CDSS - OSEMN PIPELINE
Stage 2: [S] SCRUB (Təmizlənmə, Regex Emalı və Feature Engineering)
=============================================================================
Müəllif: Baku Auto Market DS Team
Təsvir: Valyuta konvertasiyası (USD -> AZN: 1.70), mətni vahidlərdən təmizləmə,
        IQR metodu ilə kənar qiymətlərin (Outliers) və dublikatların aradan 
        qaldırılması, yeni maşın öyrənməsi xüsusiyyətlərinin yaradılması.
=============================================================================
"""

import os
import re
import sys
from typing import Dict, Tuple, Any
import numpy as np
import pandas as pd

if hasattr(sys.stdout, "reconfigure"):
    try:
        sys.stdout.reconfigure(encoding="utf-8")
        sys.stderr.reconfigure(encoding="utf-8")
    except Exception:
        pass


USD_TO_AZN_RATE = 1.70
REFERENCE_YEAR = 2026


def parse_price(val: Any) -> float:
    """
    Qarışıq valyutalı qiymət sətrini AZN ekvivalentinə çevirir.
    Nümunə: '$18 000' -> 30600.0 AZN, '24 500 AZN' -> 24500.0 AZN.
    """
    if pd.isna(val):
        return np.nan
    s = str(val).strip()
    is_usd = ("$" in s) or ("USD" in s.upper())
    
    # Rəqəmləri çıxarırıq
    digits = re.sub(r"[^\d.]", "", s.replace(" ", "").replace(",", ""))
    if not digits:
        return np.nan
    
    try:
        numeric_price = float(digits)
    except ValueError:
        return np.nan
        
    if is_usd:
        numeric_price = numeric_price * USD_TO_AZN_RATE
        
    return round(numeric_price, 2)


def parse_mileage(val: Any) -> float:
    """
    Yürüş sətrini rəqəmsal km formatına çevirir.
    Nümunə: '185 000 km' -> 185000.0.
    """
    if pd.isna(val):
        return np.nan
    s = str(val).strip()
    digits = re.sub(r"[^\d]", "", s.replace(" ", ""))
    if not digits:
        return np.nan
    return float(digits)


def parse_engine_volume(val: Any) -> float:
    """
    Mühərrik həcmini litrə çevirir.
    Nümunə: '2.0 L' -> 2.0, 'Elektro' -> 0.0.
    """
    if pd.isna(val):
        return 0.0
    s = str(val).strip()
    match = re.search(r"(\d+(\.\d+)?)", s)
    if match:
        return float(match.group(1))
    return 0.0


def parse_engine_power(val: Any) -> float:
    """
    Mühərrik gücünü at gücünə (a.g.) çevirir.
    Nümunə: '150 a.g.' -> 150.0.
    """
    if pd.isna(val):
        return np.nan
    s = str(val).strip()
    match = re.search(r"(\d+)", s)
    if match:
        return float(match.group(1))
    return np.nan


def remove_iqr_outliers(df: pd.DataFrame, column: str, factor: float = 1.5) -> Tuple[pd.DataFrame, Dict[str, float]]:
    """
    IQR (Interquartile Range) statistik metodologiyası ilə 
    anomal kənar qiymətləri filtrləyir.
    """
    q1 = df[column].quantile(0.25)
    q3 = df[column].quantile(0.75)
    iqr = q3 - q1
    lower_bound = max(0, q1 - factor * iqr)
    upper_bound = q3 + factor * iqr
    
    filtered_df = df[(df[column] >= lower_bound) & (df[column] <= upper_bound)]
    stats = {
        "column": column,
        "q1": round(float(q1), 2),
        "q3": round(float(q3), 2),
        "iqr": round(float(iqr), 2),
        "lower_bound": round(float(lower_bound), 2),
        "upper_bound": round(float(upper_bound), 2),
        "removed_count": int(len(df) - len(filtered_df))
    }
    return filtered_df, stats


def scrub_car_data(raw_df: pd.DataFrame) -> Tuple[pd.DataFrame, Dict[str, Any]]:
    """
    Xam məlumatları təmizləyən, çevirən və yeni əlamətlər yaradan 
    əsas Scrub funksiyası.
    """
    report = {
        "initial_rows": len(raw_df),
        "duplicates_removed": 0,
        "usd_converted_count": 0,
        "price_outliers_removed": 0,
        "mileage_outliers_removed": 0,
        "final_clean_rows": 0
    }
    
    df = raw_df.copy()
    
    # 1. Dublikatların aradan qaldırılması
    initial_len = len(df)
    subset_cols = ["Marka", "Model", "Buraxılış ili", "Yürüş", "Mühərrik həcmi", "Qiymət", "Şəhər"]
    df = df.drop_duplicates(subset=subset_cols, keep="first").reset_index(drop=True)
    report["duplicates_removed"] = initial_len - len(df)
    
    # 2. USD qeydlərinin sayılması və valyuta konvertasiyası
    usd_mask = df["Qiymət"].astype(str).str.contains(r"\$|USD", case=False, regex=True)
    report["usd_converted_count"] = int(usd_mask.sum())
    
    df["Qiymət_AZN"] = df["Qiymət"].apply(parse_price)
    df["Yürüş_km"] = df["Yürüş"].apply(parse_mileage)
    df["Mühərrik_Həcmi_L"] = df["Mühərrik həcmi"].apply(parse_engine_volume)
    df["Mühərrik_Gücü_ag"] = df["Mühərrik gücü"].apply(parse_engine_power)
    
    # 3. Əsas məntiqi filtrlər (Bazar sərhədləri)
    # Qiymət: 1500 AZN-dən az (beh dələduzluğu) və ya 500,000 AZN-dən çox olan anomaliyalar
    df = df.dropna(subset=["Qiymət_AZN", "Yürüş_km", "Mühərrik_Həcmi_L", "Buraxılış ili"])
    df = df[(df["Qiymət_AZN"] >= 1800) & (df["Qiymət_AZN"] <= 500000)]
    df = df[(df["Yürüş_km"] >= 50) & (df["Yürüş_km"] <= 1200000)]
    
    # 4. IQR metodu ilə Outlier təmizlənməsi
    df, price_stats = remove_iqr_outliers(df, "Qiymət_AZN", factor=2.2)
    report["price_outliers_removed"] = price_stats["removed_count"]
    report["price_iqr_stats"] = price_stats
    
    df, mileage_stats = remove_iqr_outliers(df, "Yürüş_km", factor=2.0)
    report["mileage_outliers_removed"] = mileage_stats["removed_count"]
    report["mileage_iqr_stats"] = mileage_stats
    
    # 5. Feature Engineering (Yeni əlamətlərin yaradılması)
    # Avtomobilin yaşı
    df["Car_Age"] = REFERENCE_YEAR - df["Buraxılış ili"]
    df["Car_Age"] = df["Car_Age"].apply(lambda x: max(0, x))
    
    # İllik orta yürüş (km/il)
    df["Km_Per_Year"] = df["Yürüş_km"] / df["Car_Age"].apply(lambda a: max(a, 0.5))
    df["Km_Per_Year"] = df["Km_Per_Year"].round(1)
    
    # Bakı şəhəri bayrağı
    df["Is_Baku"] = (df["Şəhər"].astype(str).str.strip() == "Bakı").astype(int)
    
    # Zədə və boya statusları
    df["Is_Damaged"] = (df["Vuruğu"].astype(str).str.contains("var", case=False)).astype(int)
    df["Is_Painted"] = (df["Rənglənmiş"].astype(str).str.strip() == "Rənglənib").astype(int)
    
    # Kredit və Barter
    df["Has_Credit"] = (df["Kredit"].astype(str).str.strip() == "Bəli").astype(int)
    df["Has_Barter"] = (df["Barter"].astype(str).str.contains("mümkündür", case=False)).astype(int)
    
    # Qiymət seqmenti kateqoriyası
    def categorize_price(p: float) -> str:
        if p < 15000:
            return "Büdcə (<15k)"
        elif p < 35000:
            return "Orta (15k-35k)"
        elif p < 75000:
            return "Biznes (35k-75k)"
        else:
            return "Premium & Lüks (>75k)"
            
    df["Price_Category"] = df["Qiymət_AZN"].apply(categorize_price)
    
    # İndeksi yeniləmək
    df = df.reset_index(drop=True)
    report["final_clean_rows"] = len(df)
    
    return df, report


def scrub_pipeline(
    raw_path: str = "data/raw_cars_baku.csv", 
    clean_path: str = "data/cleaned_cars_baku.csv"
) -> Tuple[pd.DataFrame, Dict[str, Any]]:
    """Scrub mərhələsini fayldan oxuyub təmizlənmiş fayla yazan tam axın."""
    print(f"[SCRUB] Təmizlənmə mərhələsi başladı: {raw_path}")
    raw_df = pd.read_csv(raw_path, encoding="utf-8-sig")
    cleaned_df, report = scrub_car_data(raw_df)
    
    os.makedirs(os.path.dirname(os.path.abspath(clean_path)), exist_ok=True)
    cleaned_df.to_csv(clean_path, index=False, encoding="utf-8-sig")
    print(f"[SCRUB] Təmizlənmiş məlumat saxlanıldı: {clean_path}")
    print(f"[SCRUB] Hesabat: İlkin: {report['initial_rows']} -> Təmiz: {report['final_clean_rows']} sətir.")
    return cleaned_df, report


if __name__ == "__main__":
    scrub_pipeline()
