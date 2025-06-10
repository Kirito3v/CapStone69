@echo off
setlocal EnableDelayedExpansion

set "SCRIPT_DIR=%~dp0"

rem 1) Check for existing python in PATH
where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [!] Python not found. Installing from local installer…
    if exist "%SCRIPT_DIR%python-install.exe" (
        "%SCRIPT_DIR%python-install.exe" /quiet InstallAllUsers=1 PrependPath=1 Include_test=0
    ) else (
        echo [!] python-install.exe not found next to run.bat. Aborting.
        pause
        exit /b 1
    )
    
    rem 2) Attempt to locate installed Python via registry
    set "PYDIR="
    for /f "tokens=2,*" %%A in (
        'reg query "HKLM\SOFTWARE\Python\PythonCore\3.11\InstallPath" /ve 2^>nul'
    ) do set "PYDIR=%%B"
    if not defined PYDIR (
        for /f "tokens=2,*" %%A in (
            'reg query "HKLM\SOFTWARE\WOW6432Node\Python\PythonCore\3.11\InstallPath" /ve 2^>nul'
        ) do set "PYDIR=%%B"
    )
    if defined PYDIR (
        echo [*] Found Python install at "!PYDIR!"
        echo [*] Adding to PATH for this session...
        set "PATH=!PYDIR!;!PYDIR!\Scripts;!PATH!"
    ) else (
        echo [!] Could not locate installed python directory via registry.
        echo     Please verify installer or adjust version number in script.
        pause
        exit /b 1
    )
)

rem 3) Install requirements if present
if exist "%SCRIPT_DIR%requirementsTens.txt" (
    echo [*] Installing/upgrading pip and requirements...
    python -m pip install --upgrade pip
    python -m pip install -r "%SCRIPT_DIR%requirementsTens.txt"
) else (
    echo [!] requirementsTens.txt not found, skipping requirements install.
)

rem 4) Launch the specified script
if "%~1"=="" (
    echo [!] No Python script specified. Usage: run.bat your_script.py
) else (
    echo [*] Launching "%~1"…
    python "%~1"
)

endlocal
