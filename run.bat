@echo off
setlocal

set EXE_PATH=AssetStudioGUI\bin\Debug\net472\AssetStudioGUI.exe

if not exist "%EXE_PATH%" (
    echo Executable not found: %EXE_PATH%
    echo Please run build.bat first.
    exit /b 1
)

echo Starting AssetStudioGUI...
start "" "%EXE_PATH%"

exit /b 0
