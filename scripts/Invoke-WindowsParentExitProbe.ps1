[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$OutputDirectory,
    [string]$DotNetPath = 'dotnet',
    [switch]$BuildOnly
)

$ErrorActionPreference = 'Stop'
if (-not $IsWindows -and -not $BuildOnly) {
    throw 'The process-lifetime probe requires Windows. Use -BuildOnly to check compilation elsewhere.'
}

$output = [IO.Path]::GetFullPath($OutputDirectory)
$project = Join-Path $output 'project'
$null = New-Item -ItemType Directory -Force $project
Copy-Item (Join-Path $PSScriptRoot 'WindowsParentExitProbe/Program.cs') $project
# Keep the GA experiment isolated from repository SDK pins, analyzers and packages.
'{"sdk":{"version":"11.0.100","rollForward":"disable","allowPrerelease":false}}' |
    Set-Content (Join-Path $project 'global.json')
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <OutputType>Exe</OutputType>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <UseAppHost>false</UseAppHost>
  </PropertyGroup>
</Project>
'@ | Set-Content (Join-Path $project 'WindowsParentExitProbe.csproj')

Push-Location $project
try {
    & (Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1') -DotNetPath $DotNetPath -SingleNode `
        -LogPath (Join-Path $output 'build.log') `
        -DotNetArguments @('build', 'WindowsParentExitProbe.csproj', '-c', 'Release')
    if ($LASTEXITCODE -ne 0) { throw "Probe build failed: $LASTEXITCODE (see build.log)" }
    if (-not $BuildOnly) {
        & (Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1') -DotNetPath $DotNetPath `
            -LogPath (Join-Path $output 'probe.log') `
            -DotNetArguments @((Join-Path $project 'bin/Release/net11.0/WindowsParentExitProbe.dll'), $output)
        if ($LASTEXITCODE -ne 0) { throw "Probe failed: $LASTEXITCODE (see report.json and probe.log)" }
    }
}
finally {
    Pop-Location
}
