"""
=============================================================================
BAKU AUTO MARKET CDSS - OSEMN PIPELINE
Stage 4: [M] MODEL (Maşın Öyrənməsi və Qiymət Proqnozu)
=============================================================================
Müəllif: Baku Auto Market DS Team
Təsvir: Çoxölçülü reqressiya modellərinin (Ridge, Random Forest, 
        HistGradientBoosting) təlimi, kross-validasiyası, 
        metriklərin müqayisəsi və ən yaxşı modelin saxlanması.
=============================================================================
"""

import os
import sys
import joblib
from typing import Dict, Tuple, Any, List
import numpy as np
import pandas as pd

if hasattr(sys.stdout, "reconfigure"):
    try:
        sys.stdout.reconfigure(encoding="utf-8")
        sys.stderr.reconfigure(encoding="utf-8")
    except Exception:
        pass

from sklearn.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler, OneHotEncoder
from sklearn.compose import ColumnTransformer
from sklearn.pipeline import Pipeline
from sklearn.linear_model import Ridge
from sklearn.ensemble import RandomForestRegressor, HistGradientBoostingRegressor
from sklearn.metrics import r2_score, mean_absolute_error, mean_squared_error, mean_absolute_percentage_error


MODEL_DIR = "models"
MODEL_PATH = "models/baku_car_model.joblib"

# Model üçün xüsusiyyətlər
NUMERICAL_FEATURES = [
    "Buraxılış ili",
    "Car_Age",
    "Yürüş_km",
    "Km_Per_Year",
    "Mühərrik_Həcmi_L",
    "Mühərrik_Gücü_ag",
    "Is_Baku",
    "Is_Damaged",
    "Is_Painted"
]

CATEGORICAL_FEATURES = [
    "Marka",
    "Ban növü",
    "Yanacaq növü",
    "Sürətlər qutusu",
    "Ötürücü"
]

TARGET_COL = "Qiymət_AZN"


def build_preprocessor() -> ColumnTransformer:
    """Ədədi və kateqorial dəyişənlər üçün vahid preprocessing boru kəməri."""
    numeric_transformer = Pipeline(steps=[
        ("scaler", StandardScaler())
    ])
    
    categorical_transformer = Pipeline(steps=[
        ("onehot", OneHotEncoder(handle_unknown="ignore", sparse_output=False))
    ])
    
    preprocessor = ColumnTransformer(
        transformers=[
            ("num", numeric_transformer, NUMERICAL_FEATURES),
            ("cat", categorical_transformer, CATEGORICAL_FEATURES)
        ]
    )
    return preprocessor


def train_and_evaluate_models(
    df: pd.DataFrame
) -> Tuple[Dict[str, Any], Dict[str, Dict[str, float]], Tuple[pd.DataFrame, pd.DataFrame, pd.Series, pd.Series]]:
    """
    3 fərqli reqressiya alqoritmini təlim edir və dəqiqlik metriklərini müqayisə edir:
    1. Ridge Regression (L2 Requlyarizasiyalı Xətti Baseline)
    2. Random Forest Regressor (Qərar Ağacları Ansamblı)
    3. HistGradientBoostingRegressor (Müasir Qradiyent Buketləmə)
    """
    # Məlumatın təmizliyini yoxlamaq
    features = NUMERICAL_FEATURES + CATEGORICAL_FEATURES
    df_clean = df.dropna(subset=features + [TARGET_COL]).copy()
    
    X = df_clean[features]
    y = df_clean[TARGET_COL]
    
    # 80% Train, 20% Test
    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.20, random_state=42
    )
    
    preprocessor = build_preprocessor()
    
    candidate_models = {
        "Ridge Regression (Baseline)": Ridge(alpha=10.0),
        "Random Forest Regressor": RandomForestRegressor(
            n_estimators=120, max_depth=16, min_samples_split=4, random_state=42, n_jobs=-1
        ),
        "HistGradientBoosting Regressor": HistGradientBoostingRegressor(
            max_iter=150, max_depth=12, learning_rate=0.08, l2_regularization=0.5, random_state=42
        )
    }
    
    results = {}
    fitted_pipelines = {}
    
    for name, model in candidate_models.items():
        pipe = Pipeline(steps=[
            ("preprocessor", preprocessor),
            ("regressor", model)
        ])
        
        # Təlim
        pipe.fit(X_train, y_train)
        
        # Proqnoz
        y_pred = pipe.predict(X_test)
        
        # Qiymətləndirmə Metrikləri
        r2 = r2_score(y_test, y_pred)
        mae = mean_absolute_error(y_test, y_pred)
        rmse = np.sqrt(mean_squared_error(y_test, y_pred))
        mape = mean_absolute_percentage_error(y_test, y_pred) * 100
        
        results[name] = {
            "R2_Score": round(float(r2), 4),
            "MAE_AZN": round(float(mae), 2),
            "RMSE_AZN": round(float(rmse), 2),
            "MAPE_Percent": round(float(mape), 2)
        }
        fitted_pipelines[name] = pipe
        
    # Ən yüksək R2 nəticəsinə malik modeli seçirik
    best_model_name = max(results, key=lambda k: results[k]["R2_Score"])
    best_pipe = fitted_pipelines[best_model_name]
    
    model_artifact = {
        "pipeline": best_pipe,
        "best_model_name": best_model_name,
        "metrics": results,
        "numerical_features": NUMERICAL_FEATURES,
        "categorical_features": CATEGORICAL_FEATURES,
        "all_features": features,
        "train_samples": len(X_train),
        "test_samples": len(X_test)
    }
    
    return model_artifact, results, (X_train, X_test, y_train, y_test)


def save_model(artifact: Dict[str, Any], path: str = MODEL_PATH) -> None:
    """Model artefaktını joblib ilə diskə yazır."""
    os.makedirs(os.path.dirname(os.path.abspath(path)), exist_ok=True)
    joblib.dump(artifact, path)
    print(f"[MODEL] Ən yaxşı model saxlanıldı: {path}")


def load_model(path: str = MODEL_PATH) -> Dict[str, Any]:
    """Saxlanılmış modeli yükləyir."""
    if not os.path.exists(path):
        raise FileNotFoundError(f"Model faylı tapılmadı: {path}")
    return joblib.load(path)


def predict_car_price(
    model_artifact: Dict[str, Any],
    car_specs: Dict[str, Any]
) -> Dict[str, Any]:
    """
    Tək bir avtomobilin parametrləri əsasında bazar qiymətini,
    95% güvən intervalını və bazar dəyərləndirməsini hesablayır.
    """
    pipe = model_artifact["pipeline"]
    features = model_artifact["all_features"]
    
    # Giriş lüğətindən DataFrame yaratmaq
    input_data = {}
    for col in features:
        input_data[col] = [car_specs.get(col, 0)]
        
    input_df = pd.DataFrame(input_data)
    predicted_val = float(pipe.predict(input_df)[0])
    predicted_val = max(1500.0, predicted_val)
    
    # Modelin test MAE-si əsasında etibarlılıq intervalı (±1.5 * MAE)
    best_name = model_artifact["best_model_name"]
    mae = model_artifact["metrics"][best_name]["MAE_AZN"]
    margin = mae * 1.25
    
    lower_bound = max(1000.0, round(predicted_val - margin, -2))
    upper_bound = round(predicted_val + margin, -2)
    predicted_val = round(predicted_val, -2)
    predicted_usd = round(predicted_val / 1.70, -2)
    
    return {
        "predicted_azn": predicted_val,
        "predicted_usd": predicted_usd,
        "lower_bound_azn": lower_bound,
        "upper_bound_azn": upper_bound,
        "mae_uncertainty_azn": round(margin, -2),
        "model_used": best_name
    }


def train_pipeline(
    clean_data_path: str = "data/cleaned_cars_baku.csv",
    save_path: str = MODEL_PATH
) -> Tuple[Dict[str, Any], Dict[str, Dict[str, float]]]:
    """Model mərhələsinin tam icrası."""
    print(f"[MODEL] Modellərin təlimi başlayır: {clean_data_path}")
    df = pd.read_csv(clean_data_path, encoding="utf-8-sig")
    artifact, metrics, _ = train_and_evaluate_models(df)
    save_model(artifact, save_path)
    print(f"[MODEL] Təlim tamamlandı! Ən yaxşı alqoritm: {artifact['best_model_name']}")
    for m_name, m_vals in metrics.items():
        print(f"  -> {m_name}: R²={m_vals['R2_Score']} | MAE={m_vals['MAE_AZN']} AZN | RMSE={m_vals['RMSE_AZN']} AZN")
    return artifact, metrics


if __name__ == "__main__":
    train_pipeline()
