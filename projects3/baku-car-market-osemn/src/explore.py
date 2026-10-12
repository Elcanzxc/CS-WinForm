"""
=============================================================================
BAKU AUTO MARKET CDSS - OSEMN PIPELINE
Stage 3: [E] EXPLORE (Kəşfiyyat Məlumat Analizi - EDA və İnteraktiv Qrafiklər)
=============================================================================
Müəllif: Baku Auto Market DS Team
Təsvir: Plotly əsaslı zərif, tünd temalı (Dark Mode / Fintech) interaktiv 
        analitik qrafiklər və bazar paylanmaları.
=============================================================================
"""

from typing import Dict, Any, Optional
import numpy as np
import pandas as pd
import plotly.express as px
import plotly.graph_objects as go


# Standart zərif vizual mövzu parametrləri
DARK_LAYOUT = dict(
    template="plotly_dark",
    paper_bgcolor="rgba(15, 23, 42, 0.9)",
    plot_bgcolor="rgba(15, 23, 42, 0.6)",
    font=dict(family="Inter, Segoe UI, Roboto, sans-serif", color="#E2E8F0", size=12),
    hoverlabel=dict(bgcolor="#1E293B", font_size=12, font_family="Inter, sans-serif"),
    margin=dict(l=40, r=40, t=50, b=40)
)

ACCENT_COLORS = ["#38BDF8", "#34D399", "#F472B6", "#FBBF24", "#A78BFA", "#FB7185", "#818CF8", "#2DD4BF"]


def plot_top_brands(df: pd.DataFrame, top_n: int = 10) -> go.Figure:
    """
    1. Top-N Ən Populyar Avtomobil Markaları və Orta Bazar Qiyməti
    """
    brand_counts = df["Marka"].value_counts().head(top_n).reset_index()
    brand_counts.columns = ["Marka", "Elan_Sayı"]
    
    # Orta qiyməti də əlavə edirik
    avg_price = df[df["Marka"].isin(brand_counts["Marka"])].groupby("Marka")["Qiymət_AZN"].median().reset_index()
    merged = pd.merge(brand_counts, avg_price, on="Marka").sort_values(by="Elan_Sayı", ascending=True)
    
    fig = go.Figure()
    fig.add_trace(go.Bar(
        y=merged["Marka"],
        x=merged["Elan_Sayı"],
        orientation="h",
        marker=dict(
            color=merged["Qiymət_AZN"],
            colorscale="Viridis",
            colorbar=dict(title=dict(text="Median Qiymət (AZN)", font=dict(color="#E2E8F0")), tickfont=dict(color="#E2E8F0")),
            line=dict(color="rgba(255,255,255,0.2)", width=1)
        ),
        text=[f"{count:,} elan ({price:,.0f} ₼)" for count, price in zip(merged["Elan_Sayı"], merged["Qiymət_AZN"])],
        textposition="inside",
        hoverinfo="y+x+text"
    ))
    
    fig.update_layout(
        title=f"🏆 Bakı Bazarında Top-{top_n} Ən Populyar Marka (Tələbat və Qiymət)",
        xaxis_title="Elanların Sayı",
        yaxis_title="Avtomobil Markası",
        **DARK_LAYOUT
    )
    return fig


def plot_depreciation_curve(df: pd.DataFrame) -> go.Figure:
    """
    2. Amortizasiya Əyrisi: Qiymətin avtomobilin yaşından asılılığı (Yanacaq növləri üzrə)
    """
    filtered = df[df["Yanacaq növü"].isin(["Benzin", "Hibrid", "Dizel", "Elektro"])].copy()
    grouped = filtered.groupby(["Car_Age", "Yanacaq növü"])["Qiymət_AZN"].median().reset_index()
    grouped = grouped[grouped["Car_Age"] <= 20]
    
    fig = px.line(
        grouped,
        x="Car_Age",
        y="Qiymət_AZN",
        color="Yanacaq növü",
        markers=True,
        color_discrete_map={
            "Benzin": "#38BDF8",
            "Hibrid": "#34D399",
            "Dizel": "#FBBF24",
            "Elektro": "#A78BFA"
        },
        labels={"Car_Age": "Avtomobilin Yaşı (İl)", "Qiymət_AZN": "Median Bazar Qiyməti (AZN)", "Yanacaq növü": "Yanacaq Növü"}
    )
    
    fig.update_traces(line=dict(width=3), marker=dict(size=7))
    fig.update_layout(
        title="📉 Avtomobilin Dəyər İtirmə (Amortizasiya) Dinamikası: Benzin vs Hibrid vs Dizel",
        xaxis=dict(gridcolor="rgba(255, 255, 255, 0.1)"),
        yaxis=dict(gridcolor="rgba(255, 255, 255, 0.1)"),
        **DARK_LAYOUT
    )
    return fig


def plot_mileage_vs_price(df: pd.DataFrame, sample_size: int = 1500) -> go.Figure:
    """
    3. Scatter Plot: Yürüş (km) vs Qiymət (AZN) trend xətti ilə
    """
    sample_df = df.sample(min(len(df), sample_size), random_state=42).copy()
    
    fig = px.scatter(
        sample_df,
        x="Yürüş_km",
        y="Qiymət_AZN",
        color="Ban növü",
        size="Mühərrik_Həcmi_L",
        hover_data=["Marka", "Model", "Buraxılış ili", "Mühərrik həcmi", "Yanacaq növü"],
        color_discrete_sequence=ACCENT_COLORS,
        opacity=0.75,
        labels={"Yürüş_km": "Yürüş (km)", "Qiymət_AZN": "Qiymət (AZN)", "Ban növü": "Ban Növü"}
    )
    
    fig.update_layout(
        title="⚡ Yürüş (km) və Satış Qiyməti (AZN) Korrelyasiyası",
        xaxis=dict(gridcolor="rgba(255, 255, 255, 0.1)"),
        yaxis=dict(gridcolor="rgba(255, 255, 255, 0.1)"),
        **DARK_LAYOUT
    )
    return fig


def plot_eco_cars_evolution(df: pd.DataFrame) -> go.Figure:
    """
    4. Ekoloji Nəqliyyatın Dinamikası: Hibrid və Elektromobillərin İllər üzrə Artımı
    """
    df_recent = df[df["Buraxılış ili"] >= 2012].copy()
    df_recent["Is_Eco"] = df_recent["Yanacaq növü"].apply(
        lambda x: "Hibrid / Elektro" if x in ["Hibrid", "Elektro"] else "Ənənəvi (Benzin/Dizel/Qaz)"
    )
    
    pivot = pd.crosstab(df_recent["Buraxılış ili"], df_recent["Is_Eco"], normalize="index") * 100
    pivot = pivot.reset_index()
    
    fig = go.Figure()
    if "Hibrid / Elektro" in pivot.columns:
        fig.add_trace(go.Bar(
            x=pivot["Buraxılış ili"],
            y=pivot["Hibrid / Elektro"],
            name="Hibrid & Elektro (%)",
            marker_color="#10B981"
        ))
    if "Ənənəvi (Benzin/Dizel/Qaz)" in pivot.columns:
        fig.add_trace(go.Bar(
            x=pivot["Buraxılış ili"],
            y=pivot["Ənənəvi (Benzin/Dizel/Qaz)"],
            name="Ənənəvi Mühərriklər (%)",
            marker_color="#475569"
        ))
        
    fig.update_layout(
        barmode="stack",
        title="🌱 Bakıda Eko-Nəqliyyatın İnqilabı: Hibrid və Elektromobillərin Bazar Payı (%)",
        xaxis=dict(title="Buraxılış İli", dtick=1),
        yaxis=dict(title="Bazar Payı (%)", range=[0, 100]),
        legend=dict(orientation="h", yanchor="bottom", y=1.02, xanchor="right", x=1),
        **DARK_LAYOUT
    )
    return fig


def plot_correlation_heatmap(df: pd.DataFrame) -> go.Figure:
    """
    5. Korrelyasiya Matrisi (Heatmap): Ədədi dəyişənlər arasındakı asılılıq
    """
    numeric_cols = ["Qiymət_AZN", "Buraxılış ili", "Car_Age", "Yürüş_km", "Km_Per_Year", "Mühərrik_Həcmi_L", "Mühərrik_Gücü_ag"]
    valid_cols = [c for c in numeric_cols if c in df.columns]
    
    corr = df[valid_cols].corr().round(2)
    
    # Adların oxunaqlı tərcüməsi
    display_names = {
        "Qiymət_AZN": "Qiymət (AZN)",
        "Buraxılış ili": "Buraxılış İli",
        "Car_Age": "Yaş (İl)",
        "Yürüş_km": "Yürüş (km)",
        "Km_Per_Year": "İllik Yürüş",
        "Mühərrik_Həcmi_L": "Mühərrik (L)",
        "Mühərrik_Gücü_ag": "Güc (a.g.)"
    }
    x_labels = [display_names.get(c, c) for c in valid_cols]
    y_labels = x_labels
    
    fig = px.imshow(
        corr,
        text_auto=True,
        x=x_labels,
        y=y_labels,
        color_continuous_scale="RdBu_r",
        zmin=-1,
        zmax=1,
        aspect="auto"
    )
    
    fig.update_layout(
        title="🔥 Korrelyasiya Matrisi (Pearson R Heatmap)",
        **DARK_LAYOUT
    )
    return fig


def plot_price_distribution_by_body(df: pd.DataFrame) -> go.Figure:
    """
    6. Ban Növləri üzrə Qiymət Paylanması (Box plot)
    """
    top_bodies = df["Ban növü"].value_counts().head(5).index
    df_body = df[df["Ban növü"].isin(top_bodies)]
    
    fig = px.box(
        df_body,
        x="Ban növü",
        y="Qiymət_AZN",
        color="Ban növü",
        color_discrete_sequence=ACCENT_COLORS,
        points="outliers",
        labels={"Ban növü": "Ban Növü", "Qiymət_AZN": "Qiymət (AZN)"}
    )
    
    fig.update_layout(
        title="📦 Ban Növləri üzrə Qiymət Paylanması və Mediana",
        showlegend=False,
        **DARK_LAYOUT
    )
    return fig
