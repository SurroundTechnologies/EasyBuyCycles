# If you get an error running this script run the following command:
# > Set-ExecutionPolicy -Scope CurrentUser unrestricted

# Script is configured to build with full symbos for debuging both release and debug folders
function BuildSolution    
{
   param(
    [string] $msbuild,
    [string] $solution,
    [string[]]$MSBuildArgs
   )  
    Write-Output "Building $solution"
    & $msbuild (@($solution) + $MSBuildArgs)
}

$msbuild = (&"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe)
$VisualStudioVersion = "17.0"
$ToolsVersion = "17.0"
$CommitHash = (git rev-parse HEAD).Substring(0,16)
$ApplicationVersion="1.0.0.1"
$PublisherName="EasyBuy Cycles Distributors"
$CompanyName="EasyBuy Cycles Distributors"
$SystemName="Order Management"
$SolutionSystemName="OrderManagement"
$MagePath = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\mage.exe"
$SevenZipPath="C:\Program Files\7-Zip\7z.exe" 
$WPFAppConfigLocation= "..\WPF.OrderManagement\App.config"
$WPFProjectLocation="..\WPF.OrderManagement\WPF.OrderManagement.csproj"
$DeploymentFolder="C:\Deployments\OrderManagement\_Publish"
$ClickOnceWebPageName="index.htm"
$SigningManifestPassword=""
$ClickOnceIconName="EasyBuyv4Logo.ico"
$EnvID1Name="DEV"
$EnvID1IconPath="..\WPF.OrderManagement\EasyBuyv4Logo.ico"
$EnvID1InstallationFolderURL="http://services.surroundtech.com/EasyBuyCyclesDb2Dev/BPServices/installer/"
$BPServiceHostProjectLocation="..\ServiceHost.OrderManagement.BP\ServiceHost.OrderManagement.BP.csproj"
$BPServiceHostPublishProfile="FileSystemRelease"
$ClickOnceLocation="CreateClickOnceWebpage.msbuild"
$PublishLocation="Publish.msbuild"
$SolutionFolder="C:\Surround\EasyBuyCycles\DB2Legacy\OrderManagement"

Write-Output "========================================================================"
Write-Output "=                                                                      ="
Write-Output "= ALERT: This is a dev artifact only do not use to deploy Production      ="
Write-Output "=                                                                      ="
Write-Output "========================================================================"
Write-Output " "
Write-Output "MSBuild: $msbuild"
Write-Output "Visual Studio Version: $VisualStudioVersion"
Write-Output "ToolsVersion: $ToolsVersion"
Write-Output "Commit Hash: $CommitHash"

$BpPublishArgs = @(
    "/p:DeployOnBuild=true",
    "/maxcpucount",
    "/p:PublishProfile=$BPServiceHostPublishProfile"
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp"
);

$WpfPublishArgs = @(
    "/target:publish",
    "/p:ToolsVersion=$ToolsVersion",
    "/p:OutputPath=$DeploymentFolder\Installer\temp\",
    "/p:DeployOnBuild=true",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/maxcpucount"
);

$CreatePublishWebPageArgs = @(
    "/target:CreatePublishWebPage",
    "/p:ClickOnceInstallPageTitle=$SystemName",
    "/p:Publisher=$PublisherName",
    "/p:Company=$CompanyName",
    "/p:ApplicationName=$SystemName",
    "/p:ApplicationVersion=$ApplicationVersion",
    "/p:DeploymentFolder=$DeploymentFolder",
    "/p:ClickOnceWebPageName=$ClickOnceWebPageName",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/maxcpucount"
);

$PublishArgs = @(
    "/target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput",
    "/p:EnvID=$EnvID1Name",
    "/p:ClickOnceIconName=$ClickOnceIconName",
    "/p:ClickOnceIconPath=$EnvID1IconPath",
    "/p:InstallationFolderURL=$EnvID1InstallationFolderURL",
    "/p:DeploymentFolder=$DeploymentFolder",
    "/p:SolutionSystemName=$SolutionSystemName",
    "/p:SolutionFolder=$SolutionFolder",
    "/p:SigningManifestPassword=$SigningManifestPassword"
    "/p:MagePath=$MagePath"
    "/p:SevenZipPath=$SevenZipPath"
    "/p:PublisherName=$PublisherName"
    "/p:ApplicationVersion=$ApplicationVersion",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/maxcpucount",
    "/p:ToolsVersion=$ToolsVersion"
);

# Write-Output "Cleaning up bin and obj folders"
# Get-ChildItem .. -include bin,obj -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

# BuildSolution $msbuild "..\OrderManagement.sln" $MSBuildArgs

& $msbuild UpdateApplicationVersion.msbuild /p:ApplicationVersion=$ApplicationVersion /p:WPFAppConfigLocation=$WPFAppConfigLocation /p:WPFProjectLocation=$WPFProjectLocation

& $msbuild ClearDeploymentFolder.msbuild /p:DeploymentFolder=$DeploymentFolder
Write-Output "Building BP Service Host"
# & $msbuild $BPServiceHostProjectLocation /p:VisualStudioVersion=$ToolsVersion /p:DeployOnBuild=true /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount /p:PublishProfile=$BPServiceHostPublishProfile 
& $msbuild (@($BPServiceHostProjectLocation) + $BpPublishArgs)

Write-Output "Publishing WPF"

& $msbuild (@($WPFProjectLocation) + $WpfPublishArgs)

Write-Output "Copying installer to $DeploymentFolder\Installer"

xcopy /s /e /Q $DeploymentFolder\Installer\temp\app.publish\* $DeploymentFolder\Installer

Write-Output "Deleting $DeploymentFolder\Installer\temp"

Remove-Item -Recurse -Force $DeploymentFolder\Installer\temp

Write-Output "Deleting Configs from root folder"
Remove-Item $DeploymentFolder\ConnectionParms*.config
Remove-Item $DeploymentFolder\Services*.config

& $msbuild (@($ClickOnceLocation) + $CreatePublishWebPageArgs)

& $msbuild (@($PublishLocation) + $PublishArgs)

# & $msbuild "Publish.msbuild" /target:SetConfigForEnvironment;UpdateAndResignManifests;CompressOutput /p:EnvID=$EnvID1Name /p:ClickOnceIconName=$ClickOnceIconName /p:ClickOnceIconPath=$EnvID1IconPath /p:InstallationFolderURL=$EnvID1InstallationFolderURL /p:DeploymentFolder=$DeploymentFolder /p:SolutionSystemName=$SolutionSystemName /p:SolutionFolder=$SolutionFolder  /p:SigningManifestPassword=$SigningManifestPassword /p:MagePath=$MagePath /p:SevenZipPath=$SevenZipPath /p:PublisherName=$PublisherName /p:ApplicationVersion=$ApplicationVersion  /consoleloggerparameters:ErrorsOnly;ShowTimeStamp /maxcpucount
Write-Output "Build Complete"