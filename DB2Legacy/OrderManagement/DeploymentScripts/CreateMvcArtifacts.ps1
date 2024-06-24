# If you get an error running this script run the following command:
# > Set-ExecutionPolicy -Scope CurrentUser unrestricted

param($ApplicationVersion)
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
$ApplicationVersion="1.0.0.3"
$PublisherName="EasyBuy Cycles Distributors"
$SolutionSystemName="OrderManagement"
$MagePath = "C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\mage.exe"
$SevenZipPath="C:\Program Files\7-Zip\7z.exe"
$MvcLocation="..\MVC.OrderManagement\MVC.OrderManagement.csproj"
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

$PublishArgs = @(
    # "/p:EnvID=$EnvID1Name",
    # "/p:DeploymentFolder=$DeploymentFolder",
    "/p:SolutionSystemName=$SolutionSystemName",
    "/p:SolutionFolder=$SolutionFolder",
    "/p:PublisherName=$PublisherName"
    "/p:ApplicationVersion=$ApplicationVersion",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/maxcpucount",
    "/p:ToolsVersion=$ToolsVersion",
    "/p:DeployOnBuild=true",
    "/p:PublishProfile=FolderProfile"
);

Write-Output "Publishing MVC"
& $msbuild (@($MvcLocation) + $PublishArgs)

Write-Output "MVC published successfully"