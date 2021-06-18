mkdir "%AppData%\SpaceEngineers\Mods" 2>&1 >NUL

del /f /s /q "%AppData%\SpaceEngineers\Mods\Clang Slayer" 2>&1 >NUL
rd /s /q "%AppData%\SpaceEngineers\Mods\Clang Slayer" 2>&1 >NUL
mkdir "%AppData%\SpaceEngineers\Mods\Clang Slayer" 2>&1 >NUL
xcopy /s /e /y Mod\ "%AppData%\SpaceEngineers\Mods\Clang Slayer\"
