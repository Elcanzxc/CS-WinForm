@echo off
@chcp 65001 > nul
setlocal enabledelayedexpansion
title Baku Auto Market CDSS - OSEMN Platform

echo ==============================================================================
echo       BAKU AUTO MARKET CDSS (Decision Support System) - OSEMN
echo       Baki Ikinci El Avtomobil Bazari Analitikasi ve Qiymet Proqnozu
echo       Аналитика и Прогноз Цен Вторичного Авторынка Баку
echo ==============================================================================
echo.

:: Keçid edirik cari skriptin yerləşdiyi qovluğa (Fleşka və ya istənilən disk üçün)
cd /d "%~dp0"

:: 1. Sistemdə Python yoxlanışı
where py >nul 2>nul
if %errorlevel% equ 0 (
    set "PY_CMD=py"
) else (
    where python >nul 2>nul
    if %errorlevel% equ 0 (
        set "PY_CMD=python"
    ) else (
        echo [XƏTA / ОШИБКА] Sistemdə Python tapılmadı! / Python не найден в системе!
        echo Zehmet olmasa Python 3.10+ qurasdirin ve 'Add python.exe to PATH' secin.
        echo Скачайте и установите Python с python.org (обязательно отметьте галочку Add to PATH).
        echo.
        pause
        exit /b 1
    )
)

echo [INFO] Python icraedicisi / Интерпретатор: !PY_CMD!

:: 2. Fleşka daşınma yoxlanışı (Portativlik: başqa noutbukda köhnə venv yararsız ola bilər)
if exist ".venv\Scripts\python.exe" (
    ".venv\Scripts\python.exe" -c "exit()" >nul 2>nul
    if %errorlevel% neq 0 (
        echo [INFO] Layihə başqa kompüterdən və ya fleşkadan köçürülüb.
        echo [INFO] Проект перенесен с другого ПК/флешки. Пересоздаем .venv под текущую систему...
        rmdir /s /q .venv
    )
)

:: 3. Virtual Mühitin (.venv) Yaradılması
if not exist ".venv" (
    echo [INFO] Virtual mühit (.venv) yaradılır... / Создание виртуального окружения .venv...
    !PY_CMD! -m venv .venv
    if %errorlevel% neq 0 (
        echo [XƏBƏRDARLIQ] venv yaradıla bilmədi, qlobal Python mühiti istifadə olunacaq.
    ) else (
        echo [INFO] .venv mühiti uğurla yaradıldı.
    )
)

:: 4. Mühitin Aktivləşdirilməsi
if exist ".venv\Scripts\activate.bat" (
    call ".venv\Scripts\activate.bat"
    set "RUN_PY=python"
) else (
    set "RUN_PY=!PY_CMD!"
)

:: 5. Asılılıqların (requirements.txt) Yoxlanışı və Quraşdırılması
echo [INFO] Kitabxanalar yoxlanılır / Проверка и установка библиотек из requirements.txt...
!RUN_PY! -m pip install -q -r requirements.txt
if %errorlevel% neq 0 (
    echo [XƏBƏRDARLIQ] Pip quraşdırması zamanı xəbərdarlıq çıxdı, davam edilir.
)

:: 6. UTF-8 Konsol Dəstəyi
set PYTHONIOENCODING=utf-8

:: 7. İlkin Dataset və Modelin Hazırlanması (Əgər mövcud deyilsə)
if not exist "data\cleaned_cars_baku.csv" (
    echo [INFO] Ilkin data hazirlanir ve model telim edilir...
    echo [INFO] Генерация датасета и первичное обучение моделей...
    !RUN_PY! -c "from src.obtain import obtain_raw_data; from src.scrub import scrub_pipeline; from src.model import train_pipeline; obtain_raw_data('data/raw_cars_baku.csv', 4500); scrub_pipeline('data/raw_cars_baku.csv', 'data/cleaned_cars_baku.csv'); train_pipeline('data/cleaned_cars_baku.csv', 'models/baku_car_model.joblib')"
)

:: 8. Streamlit Veb Tətbiqinin İcra Edilməsi
echo.
echo ==============================================================================
echo [START] Streamlit Veb Tətbiqi işə salınır...
echo [START] Запуск веб-дашборда Streamlit...
echo URL: http://localhost:8501
echo Brauzer avtomatik açılacaq. Dayandırmaq üçün Ctrl+C sıxın.
echo Браузер откроется автоматически. Для остановки нажмите Ctrl+C.
echo ==============================================================================
echo.

!RUN_PY! -m streamlit run app.py --server.headless false --browser.gatherUsageStats false

pause
