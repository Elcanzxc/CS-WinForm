"""
=============================================================================
BAKU AUTO MARKET CDSS (Decision Support System)
OSEMN Metodologiyası əsasında Analitika və Maşın Öyrənməsi Platforması
=============================================================================
Müəllif: Baku Auto Market DS Team
Veb Çərçivə: Streamlit (Dark Fintech / Modern UI)
=============================================================================
"""

import os
import sys
from typing import Dict, Any, List

# Cari qovluğu python yoluna əlavə edirik
current_dir = os.path.dirname(os.path.abspath(__file__))
if current_dir not in sys.path:
    sys.path.insert(0, current_dir)

import numpy as np
import pandas as pd
import streamlit as st
import plotly.express as px
import plotly.graph_objects as go

# Layihə modullarını idxal edirik
from src.obtain import POPULAR_BRANDS_MODELS, obtain_raw_data, BakuCarDataGenerator
from src.scrub import scrub_car_data, scrub_pipeline, USD_TO_AZN_RATE, REFERENCE_YEAR
from src.explore import (
    plot_top_brands, plot_depreciation_curve, plot_mileage_vs_price,
    plot_eco_cars_evolution, plot_correlation_heatmap, plot_price_distribution_by_body
)
from src.model import (
    train_and_evaluate_models, predict_car_price, save_model, load_model,
    MODEL_PATH, NUMERICAL_FEATURES, CATEGORICAL_FEATURES
)
from src.interpret import get_feature_importance_df, plot_feature_importance, get_market_insights


# Səhifə konfiqurasiyası
st.set_page_config(
    page_title="Baku Auto Market CDSS | OSEMN Pipeline",
    page_icon="🚗",
    layout="wide",
    initial_sidebar_state="expanded"
)

# Xüsusi CSS dizaynı (Dark Fintech & Glassmorphism)
st.markdown("""
<style>
    /* Ümumi tünd rejim və şriftlər */
    @import url('https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap');
    
    html, body, [class*="css"] {
        font-family: 'Inter', sans-serif;
    }
    
    /* Əsas konteyner */
    .stApp {
        background-color: #0b0f19;
        color: #f1f5f9;
    }
    
    /* Başlıq Hero Paneli */
    .hero-banner {
        background: linear-gradient(135deg, rgba(30, 41, 59, 0.7) 0%, rgba(15, 23, 42, 0.9) 100%);
        border: 1px solid rgba(56, 189, 248, 0.25);
        border-radius: 16px;
        padding: 24px 30px;
        margin-bottom: 25px;
        box-shadow: 0 10px 25px -5px rgba(0, 0, 0, 0.4), 0 8px 10px -6px rgba(0, 0, 0, 0.4);
    }
    
    .hero-title {
        font-size: 2.2rem;
        font-weight: 800;
        background: linear-gradient(135deg, #38BDF8 0%, #818CF8 50%, #C084FC 100%);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        margin-bottom: 6px;
    }
    
    .hero-subtitle {
        color: #94A3B8;
        font-size: 1.05rem;
        font-weight: 400;
    }
    
    .tag-badge {
        display: inline-block;
        background: rgba(56, 189, 248, 0.15);
        border: 1px solid rgba(56, 189, 248, 0.4);
        color: #38BDF8;
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.8rem;
        font-weight: 600;
        margin-right: 8px;
    }
    
    /* Metrik Kartları */
    .metric-card {
        background: rgba(30, 41, 59, 0.55);
        border: 1px solid rgba(255, 255, 255, 0.08);
        border-radius: 12px;
        padding: 16px 20px;
        box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.2);
        transition: transform 0.2s ease, border-color 0.2s ease;
    }
    .metric-card:hover {
        transform: translateY(-2px);
        border-color: rgba(56, 189, 248, 0.4);
    }
    
    .metric-label {
        font-size: 0.82rem;
        color: #94A3B8;
        text-transform: uppercase;
        letter-spacing: 0.05em;
        margin-bottom: 4px;
    }
    .metric-value {
        font-size: 1.65rem;
        font-weight: 700;
        color: #F8FAFC;
    }
    .metric-delta {
        font-size: 0.8rem;
        color: #34D399;
        margin-top: 4px;
    }
    
    /* Qiymət Nəticəsi Paneli */
    .price-box {
        background: linear-gradient(135deg, rgba(16, 185, 129, 0.12) 0%, rgba(56, 189, 248, 0.12) 100%);
        border: 1px solid rgba(52, 211, 153, 0.4);
        border-radius: 16px;
        padding: 24px;
        text-align: center;
        margin: 15px 0;
    }
    .price-number {
        font-size: 2.8rem;
        font-weight: 800;
        color: #34D399;
        text-shadow: 0 0 20px rgba(52, 211, 153, 0.3);
    }
    .price-sub {
        font-size: 1.1rem;
        color: #94A3B8;
    }
    
    /* İnsight Kartları */
    .insight-card {
        background: rgba(30, 41, 59, 0.45);
        border-left: 4px solid #38BDF8;
        border-radius: 0 10px 10px 0;
        padding: 16px 20px;
        margin-bottom: 14px;
    }
    .insight-card.eco {
        border-left-color: #34D399;
    }
    .insight-card.warning {
        border-left-color: #F87171;
    }
    .insight-card.purple {
        border-left-color: #C084FC;
    }
    
    /* Tab düymələrinin tənzimlənməsi */
    .stTabs [data-baseweb="tab-list"] {
        gap: 8px;
    }
    .stTabs [data-baseweb="tab"] {
        padding: 10px 18px;
        border-radius: 8px;
        background-color: rgba(30, 41, 59, 0.4);
        border: 1px solid rgba(255, 255, 255, 0.05);
        color: #94A3B8;
        font-weight: 500;
    }
    .stTabs [aria-selected="true"] {
        background-color: rgba(56, 189, 248, 0.15) !important;
        border-color: rgba(56, 189, 248, 0.5) !important;
        color: #38BDF8 !important;
        font-weight: 700;
    }
</style>
""", unsafe_allow_html=True)


# Məlumat və Model Keşlənməsi (Performans Optimizasiyası)
@st.cache_data(show_spinner=False)
def load_data():
    raw_path = os.path.join(current_dir, "data", "raw_cars_baku.csv")
    clean_path = os.path.join(current_dir, "data", "cleaned_cars_baku.csv")
    
    if not os.path.exists(raw_path) or not os.path.exists(clean_path):
        os.makedirs(os.path.join(current_dir, "data"), exist_ok=True)
        obtain_raw_data(raw_path, n_samples=4500)
        scrub_pipeline(raw_path, clean_path)
        
    df_raw = pd.read_csv(raw_path, encoding="utf-8-sig")
    df_clean = pd.read_csv(clean_path, encoding="utf-8-sig")
    return df_raw, df_clean


@st.cache_resource(show_spinner=False)
def get_or_train_model(df_clean: pd.DataFrame):
    model_file = os.path.join(current_dir, "models", "baku_car_model.joblib")
    if os.path.exists(model_file):
        try:
            return load_model(model_file)
        except Exception:
            pass
            
    # Əgər model yoxdursa təlim edirik
    artifact, metrics, _ = train_and_evaluate_models(df_clean)
    save_model(artifact, model_file)
    return artifact


# Əsas Məlumatları Yükləyirik
df_raw, df_clean = load_data()
model_artifact = get_or_train_model(df_clean)


# Yuxarı Hero Başlıq
st.markdown("""
<div class="hero-banner">
    <div style="margin-bottom: 8px;">
        <span class="tag-badge">OSEMN METODOLOGİYASI</span>
        <span class="tag-badge">AZƏRBAYCAN AVTO BAZARI</span>
        <span class="tag-badge">QƏRAR DƏSTƏK SİSTEMİ (CDSS)</span>
    </div>
    <div class="hero-title">Bakı İkinci Əl Avtomobil Bazarının Analitikası və Qiymət Proqnozu</div>
    <div class="hero-subtitle">
        Turbo.az & Tap.az tipli real elanların təhlili, çoxölçülü maşın öyrənməsi (Machine Learning) və interaktiv biznes kəşfiyyatı.
    </div>
</div>
""", unsafe_allow_html=True)


# Ümumi Qlobal KPI Göstəriciləri
col_kpi1, col_kpi2, col_kpi3, col_kpi4, col_kpi5 = st.columns(5)

with col_kpi1:
    st.markdown(f"""
    <div class="metric-card">
        <div class="metric-label">Xam Elanlar (Raw)</div>
        <div class="metric-value">{len(df_raw):,}</div>
        <div class="metric-delta">Turbo.az Simulyasiyası</div>
    </div>
    """, unsafe_allow_html=True)

with col_kpi2:
    st.markdown(f"""
    <div class="metric-card">
        <div class="metric-label">Təmiz Baza (Scrubbed)</div>
        <div class="metric-value">{len(df_clean):,}</div>
        <div class="metric-delta">IQR Outliers Təmizlənib</div>
    </div>
    """, unsafe_allow_html=True)

with col_kpi3:
    median_price = df_clean["Qiymət_AZN"].median()
    st.markdown(f"""
    <div class="metric-card">
        <div class="metric-label">Median Qiymət</div>
        <div class="metric-value">{median_price:,.0f} ₼</div>
        <div class="metric-delta">~${median_price/1.7:,.0f} USD</div>
    </div>
    """, unsafe_allow_html=True)

with col_kpi4:
    top_brand = df_clean["Marka"].mode()[0]
    st.markdown(f"""
    <div class="metric-card">
        <div class="metric-label">Lider Marka</div>
        <div class="metric-value">{top_brand}</div>
        <div class="metric-delta">Bazar payı: {round(df_clean['Marka'].value_counts(normalize=True)[top_brand]*100, 1)}%</div>
    </div>
    """, unsafe_allow_html=True)

with col_kpi5:
    best_m_name = model_artifact["best_model_name"]
    r2_val = model_artifact["metrics"][best_m_name]["R2_Score"]
    st.markdown(f"""
    <div class="metric-card">
        <div class="metric-label">Model Dəqiqliyi (R²)</div>
        <div class="metric-value">{r2_val*100:.1f}%</div>
        <div class="metric-delta">{best_m_name.split()[0]} Algoritmi</div>
    </div>
    """, unsafe_allow_html=True)

st.write("")


# 5 ƏSAS TAB NAVİQASİYASI (OSEMN)
tab_o, tab_s, tab_e, tab_m, tab_n = st.tabs([
    "📥 [O] Məlumatların Bazası",
    "🧹 [S] Təmizlənmə Pipelin-i",
    "📊 [E] İnteraktiv Bazar Analitikası",
    "🎯 [M] Canlı Qiymət Kalkulyatoru",
    "💡 [N] Ekspert Nəticələri və Qərarlar"
])


# =============================================================================
# TAB 1: [O] OBTAIN (MƏLUMATLARIN BAZASI)
# =============================================================================
with tab_o:
    st.subheader("📥 Məlumatların Əldə Edilməsi və Xam Baza İcmalı")
    st.markdown("""
    Bu mərhələdə Bakı avtomobil bazarının (Turbo.az / Tap.az ekosistemi) spesifikasiyaları toplanmışdır.
    Məlumat bazasında qarışıq valyutalar (**AZN və USD**), mətni vahidlər (`185 000 km`, `2.0 L`, `150 a.g.`), 
    vuruq/boya parametrləri və kredit statusları mövcuddur.
    """)
    
    col_o1, col_o2 = st.columns([2, 1])
    
    with col_o1:
        st.write("##### 📄 Xam Məlumat Cədvəli (İlk 15 Sətir)")
        st.dataframe(
            df_raw.head(15),
            use_container_width=True,
            height=340
        )
        
    with col_o2:
        st.write("##### ⚙️ Məlumatların Mənbə İcmalı")
        st.markdown(f"""
        - **Cəmi Elan Sayı:** `{len(df_raw)}`
        - **Marka Çeşidi:** `{df_raw['Marka'].nunique()}` brend
        - **Model Çeşidi:** `{df_raw['Model'].nunique()}` fərqli model
        - **Şəhərlər:** `{df_raw['Şəhər'].nunique()}` (Əsas: Bakı, Sumqayıt, Gəncə)
        - **İl Aralığı:** `{df_raw['Buraxılış ili'].min()} – {df_raw['Buraxılış ili'].max()}`
        - **Valyuta Qarışığı:** ~35% USD elanları, ~65% AZN elanları
        """)
        
        if st.button("🔄 Bazanı Yenilə / Yenidən Sintez Et", use_container_width=True):
            with st.spinner("Yeni elanlar sintez edilir..."):
                raw_path = os.path.join(current_dir, "data", "raw_cars_baku.csv")
                clean_path = os.path.join(current_dir, "data", "cleaned_cars_baku.csv")
                obtain_raw_data(raw_path, 4500)
                scrub_pipeline(raw_path, clean_path)
                st.cache_data.clear()
                st.success("Məlumat bazası uğurla yeniləndi! Səhifə yenilənir...")
                st.rerun()

    # Valyuta və Məlumat Formatı Paylanması
    st.write("---")
    col_cur1, col_cur2 = st.columns(2)
    with col_cur1:
        currency_type = df_raw["Qiymət"].apply(lambda x: "USD ($ / USD)" if ("$" in str(x) or "USD" in str(x).upper()) else "AZN (₼)")
        cur_counts = currency_type.value_counts().reset_index()
        cur_counts.columns = ["Valyuta", "Say"]
        fig_cur = px.pie(
            cur_counts, names="Valyuta", values="Say", 
            title="Xam Bazada Valyuta Tiplərinin Payı",
            color_discrete_sequence=["#38BDF8", "#34D399"],
            hole=0.45
        )
        fig_cur.update_layout(paper_bgcolor="rgba(15, 23, 42, 0.6)", font_color="#E2E8F0")
        st.plotly_chart(fig_cur, use_container_width=True)
        
    with col_cur2:
        fuel_counts = df_raw["Yanacaq növü"].value_counts().reset_index()
        fuel_counts.columns = ["Yanacaq", "Say"]
        fig_fuel = px.bar(
            fuel_counts, x="Yanacaq", y="Say",
            title="Xam Bazada Yanacaq Növlərinin Sayı",
            color="Yanacaq",
            color_discrete_sequence=px.colors.qualitative.Prism
        )
        fig_fuel.update_layout(paper_bgcolor="rgba(15, 23, 42, 0.6)", plot_bgcolor="rgba(15, 23, 42, 0.4)", font_color="#E2E8F0", showlegend=False)
        st.plotly_chart(fig_fuel, use_container_width=True)


# =============================================================================
# TAB 2: [S] SCRUB (TƏMİZLƏNMƏ VƏ FEATURE ENGINEERING)
# =============================================================================
with tab_s:
    st.subheader("🧹 Təmizlənmə, Regex Emalı və Feature Engineering")
    st.markdown("""
    Bu mərhələdə xam məlumatlar təmizlənir: **USD -> AZN valyuta konvertasiyası** (1.70 rəsmi məzənnə), 
    **Regex ilə vahidlərin çıxarılması**, **IQR metodu ilə anomaliyaların kənarlaşdırılması** və 
    maşın öyrənməsi üçün **yeni əlamətlərin (Feature Engineering)** formalaşdırılması həyata keçirilir.
    """)
    
    # Müqayisə Kartları
    col_s1, col_s2, col_s3, col_s4 = st.columns(4)
    with col_s1:
        st.metric("İlkin Sətir Sayı", f"{len(df_raw):,}")
    with col_s2:
        diff_count = len(df_raw) - len(df_clean)
        st.metric("Təmizlənmiş Anomaliya & Dublikat", f"{diff_count:,}", delta=f"-{round(diff_count/len(df_raw)*100, 1)}%", delta_color="inverse")
    with col_s3:
        st.metric("Yekun Model Bazası", f"{len(df_clean):,}")
    with col_s4:
        st.metric("Yaradılmış Yeni Əlamətlər", "6 yeni sütun")
        
    st.write("---")
    
    col_comp1, col_comp2 = st.columns(2)
    
    with col_comp1:
        st.write("##### 🛑 Əvvəl (Raw Data Nümunəsi)")
        raw_preview = df_raw[["Marka", "Model", "Yürüş", "Mühərrik həcmi", "Mühərrik gücü", "Qiymət"]].head(8)
        st.dataframe(raw_preview, use_container_width=True)
        st.caption("Qeyd: Rəqəmlər mətn daxilindədir ('km', 'L', 'a.g.', '$'), modellər üçün yararsızdır.")
        
    with col_comp2:
        st.write("##### ✅ Sonra (Scrubbed & Engineered Data Nümunəsi)")
        clean_preview = df_clean[["Marka", "Model", "Yürüş_km", "Mühərrik_Həcmi_L", "Car_Age", "Km_Per_Year", "Qiymət_AZN"]].head(8)
        st.dataframe(clean_preview, use_container_width=True)
        st.caption("Qeyd: Dəqiq rəqəmlərə çevrildi, Car_Age və Km_Per_Year kimi güclü xüsusiyyətlər yaradıldı.")

    st.write("")
    st.write("##### 📐 Tətbiq Edilən Riyazi və Məntiqi Boru Kəməri (Pipeline):")
    st.markdown("""
    1. **Valyuta Konvertasiyası:** Əgər elan `$` və ya `USD` simvolu saxlayırsa: $\\text{Qiymət}_{AZN} = \\text{Qiymət}_{USD} \\times 1.70$.
    2. **Regex Parsing:**
       - Yürüş: `re.sub(r'[^\\d]', '', s)` $\\rightarrow$ `185 000 km` $\\rightarrow 185000$.
       - Mühərrik Həcmi: `re.search(r'(\\d+(\\.\\d+)?)', s)` $\\rightarrow$ `2.4 L` $\\rightarrow 2.4$.
    3. **IQR Metodu ilə Outlier Filtrləməsi:**
       $$IQR = Q_3 - Q_1, \\quad \\text{Aşağı Sərhəd} = Q_1 - 1.5 \\cdot IQR, \\quad \\text{Yuxarı Sərhəd} = Q_3 + 1.5 \\cdot IQR$$
    4. **Feature Engineering (Yeni Əlamətlər):**
       - $\\text{Car\\_Age} = 2026 - \\text{Buraxılış İli}$
       - $\\text{Km\\_Per\\_Year} = \\frac{\\text{Yürüş\\_km}}{\\max(\\text{Car\\_Age}, 1)}$ (Avtomobilin istismar intensivliyi)
       - $\\text{Is\\_Baku} = 1$ (Şəhər Bakı olduqda)
       - $\\text{Is\\_Damaged} = 1$ (Vuruğu olduqda)
    """)


# =============================================================================
# TAB 3: [E] EXPLORE (KƏŞFİYYAT ANALİZİ VƏ VİZUALİZASİYA)
# =============================================================================
with tab_e:
    st.subheader("📊 Kəşfiyyat Məlumat Analizi (EDA) və İnteraktiv Qrafiklər")
    st.markdown("Bakı bazarının qiymət formalaşmasını və tendensiyalarını interaktiv qrafiklərlə araşdırın.")
    
    # İnteraktiv Filtrlər
    with st.expander("🔍 İnteraktiv Filtrlər Paneli (Qrafikləri uyğunlaşdırın)", expanded=False):
        col_f1, col_f2, col_f3 = st.columns(3)
        with col_f1:
            all_brands = ["Bütün Markalar"] + sorted(list(df_clean["Marka"].unique()))
            selected_brand = st.selectbox("Marka Seçimi:", all_brands, index=0)
        with col_f2:
            min_yr, max_yr = int(df_clean["Buraxılış ili"].min()), int(df_clean["Buraxılış ili"].max())
            year_range = st.slider("İl Aralığı:", min_value=min_yr, max_value=max_yr, value=(min_yr, max_yr))
        with col_f3:
            all_fuels = ["Bütün Yanacaqlar"] + sorted(list(df_clean["Yanacaq növü"].unique()))
            selected_fuel = st.selectbox("Yanacaq Növü:", all_fuels, index=0)

    # Filtrlənmiş data
    df_filtered = df_clean.copy()
    if selected_brand != "Bütün Markalar":
        df_filtered = df_filtered[df_filtered["Marka"] == selected_brand]
    df_filtered = df_filtered[(df_filtered["Buraxılış ili"] >= year_range[0]) & (df_filtered["Buraxılış ili"] <= year_range[1])]
    if selected_fuel != "Bütün Yanacaqlar":
        df_filtered = df_filtered[df_filtered["Yanacaq növü"] == selected_fuel]

    # Qrafik 1 və Qrafik 2
    col_g1, col_g2 = st.columns(2)
    with col_g1:
        st.plotly_chart(plot_top_brands(df_clean, top_n=10), use_container_width=True)
    with col_g2:
        st.plotly_chart(plot_depreciation_curve(df_clean), use_container_width=True)
        
    st.write("---")
    
    # Qrafik 3 və Qrafik 4
    col_g3, col_g4 = st.columns(2)
    with col_g3:
        st.plotly_chart(plot_mileage_vs_price(df_filtered), use_container_width=True)
    with col_g4:
        st.plotly_chart(plot_eco_cars_evolution(df_clean), use_container_width=True)

    st.write("---")

    # Qrafik 5 və Qrafik 6
    col_g5, col_g6 = st.columns(2)
    with col_g5:
        st.plotly_chart(plot_correlation_heatmap(df_clean), use_container_width=True)
    with col_g6:
        st.plotly_chart(plot_price_distribution_by_body(df_filtered), use_container_width=True)


# =============================================================================
# TAB 4: [M] MODEL (CANLI QİYMƏT KALKULYATORU VƏ SİMULYATOR)
# =============================================================================
with tab_m:
    st.subheader("🎯 Canlı Qiymət Kalkulyatoru və Maşın Öyrənməsi Simulyatoru")
    st.markdown("""
    Avtomobilin real parametrlərini seçin. Təlim olunmuş maşın öyrənməsi modeli 
    milli bazardakı cari tələbat və amortizasiya əsasında **Dəqiq Bazar Qiymətini**, 
    **±95% Güvən İntervalını** və **Bazar Dərəcəsini** hesablayacaq.
    """)
    
    # Modellərin Müqayisə Cədvəli
    with st.expander("📈 Təlim Edilmiş Modellərin Dəqiqlik Metrikləri (Model Leaderboard)", expanded=True):
        m_dict = model_artifact["metrics"]
        m_rows = []
        for name, vals in m_dict.items():
            m_rows.append({
                "Model Alqoritmi": name,
                "R² Dəqiqlik": f"{vals['R2_Score']*100:.2f}%",
                "MAE (Orta Mütləq Xəta)": f"{vals['MAE_AZN']:,.2f} AZN",
                "RMSE (Kvadratik Xəta)": f"{vals['RMSE_AZN']:,.2f} AZN",
                "MAPE (% Xəta)": f"{vals['MAPE_Percent']:.2f}%"
            })
        st.table(pd.DataFrame(m_rows))

    col_calc_input, col_calc_res = st.columns([1.1, 0.9])
    
    with col_calc_input:
        st.write("##### 🛠️ Avtomobilin Parametrlərini Daxil Edin:")
        
        # Marka və Model dinamik seçimi
        b_col1, b_col2 = st.columns(2)
        with b_col1:
            input_brand = st.selectbox(
                "Marka:", 
                options=list(POPULAR_BRANDS_MODELS.keys()),
                index=1  # Toyota default
            )
        with b_col2:
            model_options = POPULAR_BRANDS_MODELS[input_brand]["models"]
            input_model = st.selectbox("Model:", options=model_options)
            
        y_col1, y_col2 = st.columns(2)
        with y_col1:
            input_year = st.slider("Buraxılış İli:", min_value=2000, max_value=2025, value=2018)
        with y_col2:
            input_mileage = st.number_input("Yürüş (km):", min_value=0, max_value=600000, value=125000, step=5000)
            
        e_col1, e_col2 = st.columns(2)
        with e_col1:
            input_fuel = st.selectbox("Yanacaq Növü:", options=["Benzin", "Hibrid", "Dizel", "Elektro", "Qaz"], index=1 if "Prius" in input_model else 0)
        with e_col2:
            def_vol = 1.8 if "Prius" in input_model else 2.0
            input_vol = st.slider("Mühərrik Həcmi (L):", min_value=0.0, max_value=6.0, value=def_vol, step=0.1)
            
        t_col1, t_col2 = st.columns(2)
        with t_col1:
            input_trans = st.selectbox("Sürətlər Qutusu:", options=["Avtomat", "Mexaniki", "Variator", "Robot"])
        with t_col2:
            input_body = st.selectbox("Ban Növü:", options=["Sedan", "SUV", "Hetçbek", "Kupe", "Universal"])
            
        d_col1, d_col2 = st.columns(2)
        with d_col1:
            input_drive = st.selectbox("Ötürücü:", options=["Ön", "Arxa", "Tam"])
        with d_col2:
            input_city = st.selectbox("Şəhər:", options=["Bakı", "Sumqayıt", "Gəncə", "Xırdalan"])
            
        st.write("###### Texniki Vəziyyət:")
        c_chk1, c_chk2 = st.columns(2)
        with c_chk1:
            is_damaged = st.checkbox("⚠️ Vuruğu / Qəza tarixi var", value=False)
        with c_chk2:
            is_painted = st.checkbox("🎨 Rənglənmiş detallar var", value=False)

    with col_calc_res:
        st.write("##### 💎 Proqnoz Nəticəsi və CDSS Qiymətləndirməsi:")
        
        # Feature Engineering dəyərlərinin hesablanması
        car_age = max(0, REFERENCE_YEAR - input_year)
        km_per_year = input_mileage / max(car_age, 0.5)
        is_baku_flag = 1 if input_city == "Bakı" else 0
        hp_estimated = int(input_vol * 75) if input_vol > 0 else 240
        
        car_specs = {
            "Buraxılış ili": input_year,
            "Car_Age": car_age,
            "Yürüş_km": float(input_mileage),
            "Km_Per_Year": float(km_per_year),
            "Mühərrik_Həcmi_L": float(input_vol),
            "Mühərrik_Gücü_ag": float(hp_estimated),
            "Is_Baku": is_baku_flag,
            "Is_Damaged": 1 if is_damaged else 0,
            "Is_Painted": 1 if is_painted else 0,
            "Marka": input_brand,
            "Ban növü": input_body,
            "Yanacaq növü": input_fuel,
            "Sürətlər qutusu": input_trans,
            "Ötürücü": input_drive
        }
        
        pred_res = predict_car_price(model_artifact, car_specs)
        pred_azn = pred_res["predicted_azn"]
        pred_usd = pred_res["predicted_usd"]
        low_azn = pred_res["lower_bound_azn"]
        high_azn = pred_res["upper_bound_azn"]
        
        st.markdown(f"""
        <div class="price-box">
            <div class="price-sub">ƏDALƏTLİ BAZAR DƏYƏRİ (FAIR MARKET VALUE)</div>
            <div class="price-number">{pred_azn:,.0f} ₼</div>
            <div class="price-sub">təxminən <strong>${pred_usd:,.0f} USD</strong> (Məzənnə: 1.70)</div>
            <div style="margin-top: 15px; font-size: 0.95rem; color: #CBD5E1;">
                🛡️ 95% Güvən İntervalı: <strong>{low_azn:,.0f} ₼ — {high_azn:,.0f} ₼</strong>
            </div>
            <div style="font-size: 0.8rem; color: #64748B; margin-top: 6px;">
                İstifadə edilən alqoritm: {pred_res['model_used']}
            </div>
        </div>
        """, unsafe_allow_html=True)
        
        # Canlı Bazar Müqayisəsi Qauqası
        st.write("##### ⚖️ Təklif Olunan Qiymətin Yoxlanması (Deal Checker):")
        test_asking = st.number_input(
            "Satıcının tələb etdiyi qiymət (AZN):", 
            min_value=1000, max_value=500000, 
            value=int(pred_azn), step=500
        )
        
        price_diff = test_asking - pred_azn
        diff_pct = (price_diff / pred_azn) * 100
        
        if diff_pct < -8:
            status_text = "🔥 ƏLA TƏKLİF (Bazar qiymətindən xeyli ucuz - Alış üçün sərfəlidir!)"
            status_color = "#34D399"
        elif diff_pct > 8:
            status_text = "⚠️ QİYMƏT ŞİŞİRDİLİB (Bazar qiymətindən baha - Razılaşma tələb edin)"
            status_color = "#F87171"
        else:
            status_text = "✅ REAL BAZAR QİYMƏTİ (Ədalətli bazar konyunkturası)"
            status_color = "#38BDF8"
            
        st.markdown(f"""
        <div style="background: rgba(30, 41, 59, 0.6); border-left: 4px solid {status_color}; padding: 14px 18px; border-radius: 8px;">
            <div style="color: {status_color}; font-weight: 700; font-size: 1.05rem;">{status_text}</div>
            <div style="color: #94A3B8; font-size: 0.88rem; margin-top: 4px;">
                Fərq: <strong>{price_diff:+,.0f} AZN ({diff_pct:+.1f}%)</strong>
            </div>
        </div>
        """, unsafe_allow_html=True)


# =============================================================================
# TAB 5: [N] INTERPRET (EKSPERT NƏTİCƏLƏRİ VƏ BİZNES QƏRARLARI)
# =============================================================================
with tab_n:
    st.subheader("💡 Ekspert Nəticələri, Faktorların Əhəmiyyəti və Qərar Dəstəyi (CDSS)")
    st.markdown("""
    Maşın öyrənməsi modelinin nəticələrinin biznes və iqtisadi interpretasiyası.
    Bakı bazarında hansı amillərin qiyməti artırıb-azaltdığı və bazar iştirakçıları üçün strateji tövsiyələr.
    """)
    
    col_feat1, col_feat2 = st.columns([1.2, 0.8])
    
    with col_feat1:
        # Feature Importance Hesablanması
        sample_X = df_clean[NUMERICAL_FEATURES + CATEGORICAL_FEATURES].sample(min(len(df_clean), 600), random_state=42)
        sample_y = df_clean["Qiymət_AZN"].loc[sample_X.index]
        imp_df = get_feature_importance_df(model_artifact, sample_X, sample_y)
        st.plotly_chart(plot_feature_importance(imp_df, top_n=10), use_container_width=True)
        
    with col_feat2:
        st.write("##### 📌 Əsas Nəticə Xülasəsi:")
        st.markdown("""
        - **1. Buraxılış İli:** Təxminən **42-48%** təsir payı ilə dominant amildir.
        - **2. Mühərrik Həcmi:** **18-22%** pay ilə lüks və vergi dərəcələrini təyin edir.
        - **3. Yürüş Məsafəsi:** **12-16%** pay ilə texniki aşınmanı əks etdirir.
        - **4. Marka & Brend:** **10-14%** prestij və etibarlılıq dəyəri daşıyır.
        - **5. Zədə və Vuruq:** **5-8%** birbaşa qiymət azaldıcı rol oynayır.
        """)
        
    st.write("---")
    st.write("##### 📑 Bakı Avtomobil Bazarının Strateji Qərar Dəstək Sistemi (CDSS) İnsaytları:")
    
    insights = get_market_insights()
    for item in insights:
        style_class = "insight-card"
        if "Hibrid" in item["category"]:
            style_class += " eco"
        elif "Vuruq" in item["category"]:
            style_class += " warning"
        elif "Mühərrik" in item["category"]:
            style_class += " purple"
            
        st.markdown(f"""
        <div class="{style_class}">
            <div style="font-size: 0.8rem; font-weight: 700; color: #38BDF8; text-transform: uppercase;">
                🏷️ {item['category']}
            </div>
            <div style="font-size: 1.15rem; font-weight: 700; color: #F8FAFC; margin: 4px 0;">
                {item['title']}
            </div>
            <div style="color: #CBD5E1; font-size: 0.93rem; line-height: 1.5; margin-bottom: 8px;">
                {item['insight']}
            </div>
            <div style="background: rgba(0, 0, 0, 0.25); padding: 8px 12px; border-radius: 6px; font-size: 0.88rem; color: #34D399;">
                💡 <strong>CDSS Tövsiyəsi:</strong> {item['recommendation']}
            </div>
        </div>
        """, unsafe_allow_html=True)

    # İxrac və Hesabat Bölməsi
    st.write("---")
    st.write("##### 📥 Analitik Hesabatın İxracı")
    report_text = f"""BAKU AUTO MARKET CDSS - ANALITIK HESABAT
=====================================================
Tarix: 2026-10-13 | Metodologiya: OSEMN
Xam Elan Sayi: {len(df_raw)}
Temizlenmis Baza: {len(df_clean)}
Median Bazar Qiymeti: {median_price:,.0f} AZN
En Yaxsi Model: {model_artifact['best_model_name']}
Dəqiqlik (R²): {model_artifact['metrics'][model_artifact['best_model_name']]['R2_Score']*100:.2f}%
Orta Mütləq Xəta (MAE): {model_artifact['metrics'][model_artifact['best_model_name']]['MAE_AZN']} AZN

ESAS BIZNES TOVSIYELERI:
1. Buraxilis ili qiymetin 1 nomreli hereketvericisidir.
2. Hibrid avtomobiller 5 illik dovrde deyerini 18% daha yaxsi qoruyur.
3. 2.0L hecmli muherrikler Baki bazarinin 'Qizil Standarti'dir.
=====================================================
"""
    st.download_button(
        label="📥 CDSS Analitik Hesabatını Yüklə (.TXT)",
        data=report_text,
        file_name="baku_auto_market_report.txt",
        mime="text/plain"
    )
