@echo off
chcp 65001 > nul
echo ======================================================================
echo   PlantCare OS: Сборка автономного исполняемого файла (.exe)
echo ======================================================================

echo [1/3] Проверка окружения Python и PyInstaller...
py -m pip install --quiet pydantic rich pyyaml pyinstaller

echo [2/3] Компиляция бинарного файла через PyInstaller...
py -m PyInstaller --clean --noconfirm --onefile --name "PlantCareOS" --add-data "knowledge_base.yaml;." main.py

echo [3/3] Копирование базы знаний в папку dist (для автономности)...
copy /Y knowledge_base.yaml dist\ > nul

echo ======================================================================
echo   СБОРКА УСПЕШНО ЗАВЕРШЕНА!
echo   Готовый исполняемый файл: dist\PlantCareOS.exe
echo   Вы можете скопировать файл dist\PlantCareOS.exe на флешку
echo   и запускать его на ЛЮБОМ компьютере с Windows БЕЗ установки Python!
echo ======================================================================
pause
