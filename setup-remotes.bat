@echo off
REM setup-remotes.bat

echo Adding origin and upstream remotes...

REM Удаляем существующие remotes (если есть)
git remote rm upstream
git remote add upstream https://github.com/ArduPilot/MissionPlanner.git
git remote rm origin
git remote add origin https://github.com/inertiallabs/MissionPlanner.git

echo Fetching origin and upstream...
git fetch origin
git fetch upstream

echo Done.
pause
