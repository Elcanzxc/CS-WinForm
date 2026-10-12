@echo off
chcp 65001 > nul
title PlantCare OS - Hibrid Ekspert Sistemi
cd /d "%~dp0"

if exist "main.exe" (
    main.exe
) else if exist "dist\main.exe" (
    dist\main.exe
) else if exist "PlantCareOS.exe" (
    PlantCareOS.exe
) else (
    py main.py
)
pause
