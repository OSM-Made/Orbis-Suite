set SolutionDir=%1

set /p BuildNumber=<"%SolutionDir%\Windows\OrbisNeighborHood\Resources\BuildNumber.txt"

rem Remove previous versions
del "%SolutionDir%\Windows\Installer\BootstrapperSetup\bin\Release\Orbis Suite Setup 3.0.*.exe"

rem Rename to the current build version.
rename "%SolutionDir%\Windows\Installer\BootstrapperSetup\bin\Release\Orbis Suite Setup.exe" "Orbis Suite Setup 3.0.%BuildNumber%.exe"