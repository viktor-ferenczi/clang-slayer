@echo off

if "%~1"=="" (
    echo Argument %1 is empty. Exiting script.
    exit /b 1
)

mkdir "%AppData%\SpaceEngineers\Mods" 2>&1 >NUL

del /f /s /q "%AppData%\SpaceEngineers\Mods\%1" 2>&1 >NUL
rd /s /q "%AppData%\SpaceEngineers\Mods\%1" 2>&1 >NUL

mkdir "%AppData%\SpaceEngineers\Mods\%1" 2>&1

xcopy /s /e /y Mod\ "%AppData%\SpaceEngineers\Mods\%1\"

echo Done
