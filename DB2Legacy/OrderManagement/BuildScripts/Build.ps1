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
    echo "Building $solution"
    & $msbuild (@($solution) + $MSBuildArgs)
}

$msbuild = (&"${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -prerelease -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe)
$VisualStudioVersion = "17.0"
$ToolsVersion = "Current"
$CommitHash = (git rev-parse HEAD).Substring(0,16)
$location = Get-Location

echo "========================================================================"
echo "=                                                                      ="
echo "= ALERT: This is a dev build only do not use to deploy Production      ="
echo "=                                                                      ="
echo "========================================================================"
echo " "
echo "MSBuild: $msbuild"
echo "Visual Studio Version: $VisualStudioVersion"
echo "ToolsVersion: $ToolsVersion"
echo "Commit Hash: $CommitHash"

$MSBuildArgs = @(
    "/maxcpucount",
    "/p:Configuration=Release",
    "/p:DebugSymbols=true",
    "/p:DebugType=Full",
    "/p:Platform=Any CPU",
    "/p:AllowUnsafeBlocks=true",
    "/nodeReuse:false",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/p:VisualStudioVersion=$VisualStudioVersion",
    "/p:ToolsVersion=$ToolsVersion",
    "/p:SourceRevisionId=$CommitHash"

);

$MSBuildArgsDebug = @(
    "/maxcpucount",
    "/p:Configuration=Debug",
    "/p:DebugSymbols=true",
    "/p:DebugType=Full",
    "/p:Platform=Any CPU",
    "/p:AllowUnsafeBlocks=true",
    "/nodeReuse:false",
    "/consoleloggerparameters:ErrorsOnly;ShowTimeStamp",
    "/p:VisualStudioVersion=$VisualStudioVersion",
    "/p:ToolsVersion=$ToolsVersion"
);

echo "Cleaning up bin and obj folders"
Get-ChildItem .. -include bin,obj -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }

BuildSolution $msbuild "..\OrderManagement.sln" $MSBuildArgs

echo "Build Complete"