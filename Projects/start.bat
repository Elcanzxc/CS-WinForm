@echo off
chcp 65001 >nul
title MedTriage Pro — Klinik Qebul ve Tecili Heyecan Terminali
color 0B

echo ===============================================================================
echo            MEDTRIAGE PRO — ENTERPRISE KLINIK TRIAJ SISTEMI
echo               Baki Seher Tecili Tibbi Merkez Terminali
echo ===============================================================================
echo.

:: 1. Check Node.js
where node >nul 2>nul
if %errorlevel% neq 0 (
    echo [XETA] Node.js tapilmadi!
    echo Zehmet olmasa Node.js v18+ qurasdirin: https://nodejs.org/
    echo Flash kartda Node.js portativ versiyasi teleb olunur.
    echo.
    pause
    exit /b 1
)

echo [1/3] Node.js yoxlanildi:
node -v
echo.

:: 2. Check node_modules
if not exist "node_modules\" (
    echo [2/3] Paketler tapilmadi. Asililiqlar qurasdirilir (npm install)...
    call npm install
    if %errorlevel% neq 0 (
        echo [XETA] Asililiqlari yuklemek mumkun olmadi!
        pause
        exit /b 1
    )
) else (
    echo [2/3] Asililiqlar movcuddur (node_modules).
)
echo.

:: 3. Check SQLite database
if not exist "clinic.db" (
    echo [3/3] SQLite klinik veriler bazasi tapilmadi.
    echo Ilk ise salinmada 'clinic.db' avtomatik yaradilacaq ve tecrube melumatlari ile doldurulacaq.
) else (
    echo [3/3] SQLite veriler bazasi movcuddur: clinic.db
)
echo.

echo ===============================================================================
echo  Server basladilir: http://localhost:3000
echo  Brauzer 3 saniye erzinde avtomatik acilacaq...
echo  Terminali baglamaq ucun Ctrl + C basin.
echo ===============================================================================
echo.

:: Open browser after a brief delay
start "" http://localhost:3000

:: Start Next.js dev server
call npm run dev

pause
