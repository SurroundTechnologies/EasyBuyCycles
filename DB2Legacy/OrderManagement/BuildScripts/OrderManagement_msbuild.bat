@echo off
REM //Set Variables

REM // Get start time in milliseconds
for /F "tokens=1-4 delims=:.," %%a in ("%time%") do (set /A "startTime=(((%%a*60)+1%%b %% 100)*60+1%%c %% 100)*100+1%%d %% 100")

set buildScriptsLocation=%~dp0
REM// Remove trailing backslash
set buildScriptsLocation=%buildScriptsLocation:~0,-1%
@echo Build Scripts Location: %buildScriptsLocation%
pushd ..
set solutionLocation=%cd%
popd
@echo Solution Location: %solutionLocation%

set solutionSystemName=OrderManagement
set ExitBuildOnSuccess=N

if "%~1"=="Y" set ExitBuildOnSuccess=Y

REM //Get the Code Factory Path
for /f "tokens=3delims=<>    " %%i in ('type %solutionLocation%\A4DNReference.msbuild ^|find "A4DNCodeFactoryHintPath"') do set "dependToolLocation=%%i"


:KILLTASKS
REM //This will Kill Any Tasks that will stop building
@echo.
@echo Killing webserver tasks for %solutionSystemName%
taskkill /im webdev.webserver40.exe | SET ERRORLEVEL=0
taskkill /im WPF.%solutionSystemName%.vshost.exe /f | SET ERRORLEVEL=0

:CLEAROBJBIN
REM //This will Clear anything in the OBJ and BIN Folders
@echo.
@echo Clearing bin and obj folders in %solutionLocation%\
for /d /r "%solutionLocation%\" %%d in (bin,obj) do @if exist "%%d" rd /s/q "%%d"


:DEPENDTOOL
REM //This will Run the Dependency Tool and create a Build Script
@echo.
@echo Running Dependency Tool for %solutionSystemName%
"%dependToolLocation%A4DN.CF.DependencyTool.exe" rp="%solutionLocation%" op="%buildScriptsLocation%" pr="%solutionSystemName%" gd="N" us="N" md="4" bat="N"

:BUILDING
REM //This will Run the Build Script
@echo.
@echo Building %solutionSystemName%
@echo ON
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\current\bin\msbuild.exe" "%buildScriptsLocation%\%solutionSystemName%_gen.msbuild" /p:Configuration=Debug /p:VisualStudioVersion=17.0 /toolsversion:17.0 /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount /nr:false
@echo OFF

set BUILD_STATUS=%ERRORLEVEL%

REM //Get end time
for /F "tokens=1-4 delims=:.," %%a in ("%time%") do (set /A "endTime=(((%%a*60)+1%%b %% 100)*60+1%%c %% 100)*100+1%%d %% 100")
set /A elapsedTime=endTime-startTime

REM //Format elapsed time:
set /A hh=elapsedTime/(60*60*100), rest=elapsedTime%%(60*60*100), mm=rest/(60*100), rest%%=60*100, ss=rest/100, cc=rest%%100
if %mm% lss 10 set mm=0%mm%
if %ss% lss 10 set ss=0%ss%
if %cc% lss 10 set cc=0%cc%

if %BUILD_STATUS% EQU 0 @echo Build Succeeded, Build Time: %hh%:%mm%:%ss%.%cc%
GOTO DONE
if %BUILD_STATUS% NEQ 0 @echo Build Failed, Build Time: %hh%:%mm%:%ss%.%cc%
GOTO ERROR

:DONE
if %ExitBuildOnSuccess%==Y GOTO EXIT
pause
exit /B 0

:ERROR
pause
exit /B 1

:EXIT
exit /B 0