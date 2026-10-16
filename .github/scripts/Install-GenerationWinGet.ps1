param(
    [Parameter(Mandatory)]
    [string] $Bundle,
    [Parameter(Mandatory)]
    [string] $DependencyRoot
)

$ErrorActionPreference = 'Stop'
$dependencyPaths = @(Get-ChildItem -LiteralPath (Join-Path $DependencyRoot 'x64') -File |
    Where-Object Extension -in '.appx', '.msix' | ForEach-Object FullName)
if ($dependencyPaths.Count -eq 0) {
    throw 'No x64 WinGet dependencies found in the verified archive.'
}

# Run in Windows PowerShell: the Appx compatibility proxy joins dependency paths.
Add-AppxPackage -Path $Bundle -DependencyPath $dependencyPaths -ForceUpdateFromAnyVersion
