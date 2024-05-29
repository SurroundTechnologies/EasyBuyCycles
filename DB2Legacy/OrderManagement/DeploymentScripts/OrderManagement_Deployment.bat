@echo off
REM //Set Variables
set ApplicationVersion=1.0.0.0
set ToolsVersion=17.0
set PublisherName="EasyBuy Cycles Distributors"
set CompanyName="EasyBuy Cycles Distributors"
set SystemName="Order Management"
pushd ..
set SolutionFolder=%cd%
popd
@echo Solution Folder: %SolutionFolder%
set SolutionSystemName=OrderManagement
set BuildScriptLocation=..\BuildScripts\OrderManagement_msbuild.bat
set DeploymentFolder=C:\Deployments\OrderManagement\_Publish

set WPFAppConfigLocation=..\WPF.OrderManagement\App.config
set WPFProjectLocation=..\WPF.OrderManagement\WPF.OrderManagement.csproj
set BPServiceHostProjectLocation=..\ServiceHost.OrderManagement.BP\ServiceHost.OrderManagement.BP.csproj
set BPServiceHostPublishProfile=FileSystemRelease

set MagePath="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6 Tools\mage.exe"
set SevenZipPath="C:\Program Files\7-Zip\7z.exe"

set ClickOnceWebPageName=index.htm
set SigningManifestPassword=""
set ClickOnceIconName="EasyBuyv4Logo.ico"
set EnvID1Name=DEV
set EnvID1IconPath="%SolutionFolder%\WPF.OrderManagement\EasyBuyv4Logo.ico"
set EnvID1InstallationFolderURL="http://YourDomainName/YourEnvIDSystemFolderName/BPServices/installer/"
set EnvID2Name=TST
set EnvID2IconPath="%SolutionFolder%\WPF.OrderManagement\EasyBuyv4Logo.ico"
set EnvID2InstallationFolderURL=""
set EnvID3Name=PRO
set EnvID3IconPath="%SolutionFolder%\WPF.OrderManagement\EasyBuyv4Logo.ico"
set EnvID3InstallationFolderURL=""
set EnvID4Name=
set EnvID4IconPath=""
set EnvID4InstallationFolderURL=""
set EnvID5Name=
set EnvID5IconPath=""
set EnvID5InstallationFolderURL=""

set ExitOnSuccess=N

REM // Get start time in milliseconds
for /F "tokens=1-4 delims=:.," %%a in ("%time%") do (set /A "startTime=(((%%a*60)+1%%b %% 100)*60+1%%c %% 100)*100+1%%d %% 100")

:UpdateApplicationVersion
@echo.
@echo Updating Application Version to %ApplicationVersion%
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "UpdateApplicationVersion.msbuild" /p:ApplicationVersion=%ApplicationVersion% /p:WPFAppConfigLocation=%WPFAppConfigLocation% /p:WPFProjectLocation=%WPFProjectLocation%
@echo OFF
if errorlevel 1 GOTO ERROR


:RunBuildScript
@echo.
@echo Running Build Script %BuildScriptLocation%
@echo ====================================================
@echo ON
call "%BuildScriptLocation%" Y
@echo OFF
if errorlevel 1 GOTO ERROR


:ClearDeploymentFolder
@echo.
@echo Clearing Deployment Folder %DeploymentFolder%
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "ClearDeploymentFolder.msbuild" /p:DeploymentFolder=%DeploymentFolder% 
@echo OFF
if errorlevel 1 GOTO ERROR


:PublishBPServices
@echo.
@echo Publishing BP Services %BPServiceHostProjectLocation%
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "%BPServiceHostProjectLocation%" /p:VisualStudioVersion=%ToolsVersion% /p:DeployOnBuild=true /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount /p:PublishProfile=%BPServiceHostPublishProfile%  
@echo OFF
if errorlevel 1 GOTO ERROR


:PublishWPFClickOnce
@echo.
@echo Publishing WPF Click Once %WPFProjectLocation%
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "%WPFProjectLocation%"  /target:publish /p:VisualStudioVersion=%ToolsVersion% /p:OutputPath=%DeploymentFolder%\Installer\temp\ /p:DeployOnBuild=true  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR

@echo ON
xcopy /s /e /Q %DeploymentFolder%\Installer\temp\app.publish\* %DeploymentFolder%\Installer
rmdir %DeploymentFolder%\Installer\temp /S /Q
del "%DeploymentFolder%\ConnectionParms*.config"
del "%DeploymentFolder%\Services*.config"
@echo OFF

:CreateClickOnceInstallPage
@echo.
@echo Creating WPF Click Once Install Page 
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "CreateClickOnceWebpage.msbuild" /target:CreatePublishWebPage /p:ClickOnceInstallPageTitle=%SystemName% /p:Publisher=%PublisherName% /p:Company=%CompanyName% /p:ApplicationName=%SystemName% /p:ApplicationVersion=%ApplicationVersion% /p:DeploymentFolder=%DeploymentFolder% /p:ClickOnceWebPageName=%ClickOnceWebPageName% /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR

if "%EnvID1Name%" == "" GOTO FINALIZE


:PublishEnvID1
@echo.
@echo %EnvID1Name% - SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=%EnvID1Name% /p:ClickOnceIconName=%ClickOnceIconName% /p:ClickOnceIconPath=%EnvID1IconPath% /p:InstallationFolderURL=%EnvID1InstallationFolderURL% /p:DeploymentFolder=%DeploymentFolder% /p:SolutionSystemName=%SolutionSystemName% /p:SolutionFolder=%SolutionFolder%  /p:SigningManifestPassword=%SigningManifestPassword% /p:MagePath=%MagePath% /p:SevenZipPath=%SevenZipPath% /p:PublisherName=%PublisherName% /p:ApplicationVersion=%ApplicationVersion%  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR


if "%EnvID2Name%" == "" GOTO FINALIZE

 :PublishEnvID2
@echo.
@echo %EnvID2Name% - SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=%EnvID2Name% /p:ClickOnceIconName=%ClickOnceIconName% /p:ClickOnceIconPath=%EnvID2IconPath% /p:InstallationFolderURL=%EnvID2InstallationFolderURL% /p:DeploymentFolder=%DeploymentFolder% /p:SolutionSystemName=%SolutionSystemName% /p:SolutionFolder=%SolutionFolder%  /p:SigningManifestPassword=%SigningManifestPassword% /p:MagePath=%MagePath% /p:SevenZipPath=%SevenZipPath% /p:PublisherName=%PublisherName% /p:ApplicationVersion=%ApplicationVersion%  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR


if "%EnvID3Name%" == "" GOTO FINALIZE
 
 
 :PublishEnvID3
@echo.
@echo %EnvID3Name% - SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=%EnvID3Name% /p:ClickOnceIconName=%ClickOnceIconName% /p:ClickOnceIconPath=%EnvID3IconPath% /p:InstallationFolderURL=%EnvID3InstallationFolderURL% /p:DeploymentFolder=%DeploymentFolder% /p:SolutionSystemName=%SolutionSystemName% /p:SolutionFolder=%SolutionFolder%  /p:SigningManifestPassword=%SigningManifestPassword% /p:MagePath=%MagePath% /p:SevenZipPath=%SevenZipPath% /p:PublisherName=%PublisherName% /p:ApplicationVersion=%ApplicationVersion%  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR


if "%EnvID4Name%" == "" GOTO FINALIZE
 
:PublishEnvID4
@echo.
@echo %EnvID4Name% - SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=%EnvID4Name% /p:ClickOnceIconName=%ClickOnceIconName% /p:ClickOnceIconPath=%EnvID4IconPath% /p:InstallationFolderURL=%EnvID4InstallationFolderURL% /p:DeploymentFolder=%DeploymentFolder% /p:SolutionSystemName=%SolutionSystemName% /p:SolutionFolder=%SolutionFolder%  /p:SigningManifestPassword=%SigningManifestPassword% /p:MagePath=%MagePath% /p:SevenZipPath=%SevenZipPath% /p:PublisherName=%PublisherName% /p:ApplicationVersion=%ApplicationVersion%  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR

if "%EnvID5Name%" == "" GOTO FINALIZE

 
:PublishEnvID5
@echo.
@echo %EnvID5Name% - SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput
@echo ====================================================
@echo ON
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild" "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=%EnvID5Name% /p:ClickOnceIconName=%ClickOnceIconName% /p:ClickOnceIconPath=%EnvID5IconPath% /p:InstallationFolderURL=%EnvID5InstallationFolderURL% /p:DeploymentFolder=%DeploymentFolder% /p:SolutionSystemName=%SolutionSystemName% /p:SolutionFolder=%SolutionFolder%  /p:SigningManifestPassword=%SigningManifestPassword% /p:MagePath=%MagePath% /p:SevenZipPath=%SevenZipPath% /p:PublisherName=%PublisherName% /p:ApplicationVersion=%ApplicationVersion%  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
@echo OFF
if errorlevel 1 GOTO ERROR


:FINALIZE
REM //Get end time
for /F "tokens=1-4 delims=:.," %%a in ("%time%") do (set /A "endTime=(((%%a*60)+1%%b %% 100)*60+1%%c %% 100)*100+1%%d %% 100")
set /A elapsedTime=endTime-startTime

REM //Format elapsed time:
set /A hh=elapsedTime/(60*60*100), rest=elapsedTime%%(60*60*100), mm=rest/(60*100), rest%%=60*100, ss=rest/100, cc=rest%%100
if %mm% lss 10 set mm=0%mm%
if %ss% lss 10 set ss=0%ss%
if %cc% lss 10 set cc=0%cc%

if %BUILD_STATUS% EQU 0 @echo Deployment Succeeded, Deployment Time: %hh%:%mm%:%ss%.%cc%
GOTO DONE

if %BUILD_STATUS% NEQ 0 @echo Deployment Failed, Deployment Time: %hh%:%mm%:%ss%.%cc%
GOTO ERROR

:DONE
if %ExitOnSuccess%==Y GOTO EXIT
pause
exit /B 0

:ERROR
pause
exit /B 1

:EXIT
exit /B 0