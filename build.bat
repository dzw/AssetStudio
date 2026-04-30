@echo off
setlocal

echo Building AssetStudio Solution...
echo.

dotnet build AssetStudio.sln -c Debug

if %errorlevel% neq 0 (
    echo.
    echo Build failed with error code %errorlevel%
    exit /b %errorlevel%
)

echo.
echo Build succeeded!
exit /b 0
