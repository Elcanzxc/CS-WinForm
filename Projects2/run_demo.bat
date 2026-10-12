@echo off
chcp 65001 > nul
title PlantCare OS - Etalon Audit və Verifikasiya
cd /d "%~dp0"

if exist "main.exe" (
    main.exe --demo
) else if exist "dist\main.exe" (
    dist\main.exe --demo
) else if exist "PlantCareOS.exe" (
    PlantCareOS.exe --demo
) else (
    py main.py --demo
)
pause
