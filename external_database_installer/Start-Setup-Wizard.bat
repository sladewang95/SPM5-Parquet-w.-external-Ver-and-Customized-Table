@echo off
REM ============================================================================
REM ATSPM 5.0 Interactive Database Setup Launcher
REM ============================================================================
REM Simple launcher for the interactive setup wizard
REM ============================================================================

echo.
echo Starting ATSPM Database Setup Wizard...
echo.
echo Developed by: ACTIONLAB at UT Arlington
echo Author: sladewang
echo.

REM Get the directory where this batch file is located
set "SCRIPT_DIR=%~dp0"

REM Try to run with PowerShell (full path to avoid PATH issues)
if exist "%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" (
    "%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -ExecutionPolicy Bypass -NoProfile -File "%SCRIPT_DIR%Setup-Interactive.ps1"
    exit /b %ERRORLEVEL%
)

REM Try PowerShell Core if Windows PowerShell not found
if exist "%ProgramFiles%\PowerShell\7\pwsh.exe" (
    "%ProgramFiles%\PowerShell\7\pwsh.exe" -ExecutionPolicy Bypass -NoProfile -File "%SCRIPT_DIR%Setup-Interactive.ps1"
    exit /b %ERRORLEVEL%
)

REM If neither found, show error
echo ============================================================================
echo ERROR: PowerShell not found!
echo ============================================================================
echo.
echo PowerShell is required to run this setup wizard.
echo.
echo Please try one of these options:
echo.
echo   1. Right-click on 'Setup-Interactive.ps1' and select 'Run with PowerShell'
echo.
echo   2. Or open PowerShell and run:
echo      cd "%SCRIPT_DIR%"
echo      .\Setup-Interactive.ps1
echo.
echo   3. Install PowerShell from: https://aka.ms/PSWindows
echo.
echo ============================================================================
echo.
pause
exit /b 1
