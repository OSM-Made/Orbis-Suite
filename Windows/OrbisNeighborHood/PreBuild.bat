REM echo 3.0.$([MSBuild]::Add($(BuildVersion), 1) $(ConfigurationName) Build $(CurrentDate) &gt; &quot;$(ProjectDir)\Resources\BuildDate.txt"

set ProjectDir=%1
set CurrentDate=%~2
set ConfigurationName=%~3

REM Increments build number.
set /p BuildNumber=< %ProjectDir%\Resources\BuildNumber.txt
set /a "BuildNumber=%BuildNumber%+1"
echo %BuildNumber%> %ProjectDir%\Resources\BuildNumber.txt

REM Build String.
echo Version 3.0.%BuildNumber% %ConfigurationName% Build %CurrentDate% > %ProjectDir%\Resources\BuildString.txt
