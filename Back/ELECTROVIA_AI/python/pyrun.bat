@echo off
setlocal

call conda activate env

set "SCRIPT_DIR=%~dp0"
if exist "%SCRIPT_DIR%requirements.txt" (
    echo [*] Installing/upgrading pip and requirements...
    python -m pip install --upgrade pip
    python -m pip install -r "%SCRIPT_DIR%requirements.txt"
) else (
    echo [!] requirements.txt not found in "%SCRIPT_DIR%", skipping install.
)

if "%~1"=="" (
    echo [!] No Python script specified. Usage: run.bat your_script.py
) else (
    echo [*] Launching "%~1"…
    python "%~1"
)

endlocal
