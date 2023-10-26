set SolutionDir=%1

set /p BuildNumber=< %SolutionDir%\Windows\OrbisNeighborHood\Resources\BuildNumber.txt

echo ^<?xml version="1.0" encoding="utf-8"?^>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi
echo ^<Include Id="VersionNumberInclude"^>>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi
echo 	^<?define MajorVersion="3" ?^>>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi
echo 	^<?define MinorVersion="0" ?^>>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi
echo 	^<?define BuildVersion="%BuildNumber%" ?^>>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi
echo ^</Include^>>> %SolutionDir%\Windows\Installer\BootstrapperSetup\Version.wxi