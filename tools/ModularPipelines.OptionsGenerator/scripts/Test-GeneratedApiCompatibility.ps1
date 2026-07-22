[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string] $BaselineDirectory,

    [Parameter(Mandatory)]
    [string] $CurrentDirectory
)

$ErrorActionPreference = 'Stop'

function Get-SinglePackage([string] $Directory, [string] $Description) {
    $packages = @(Get-ChildItem -LiteralPath $Directory -Filter '*.nupkg' -File |
        Where-Object { $_.Name -notlike '*.symbols.nupkg' -and $_.Name -notlike '*.snupkg' })

    if ($packages.Count -ne 1) {
        throw "Expected exactly one $Description package in '$Directory'; found $($packages.Count)."
    }

    return $packages[0].FullName
}

$baselinePackage = Get-SinglePackage $BaselineDirectory 'baseline'
$currentPackage = Get-SinglePackage $CurrentDirectory 'current'
$repositoryRoot = Resolve-Path (Join-Path $PSScriptRoot '../../..')

Push-Location $repositoryRoot
try {
    dotnet tool restore
    if ($LASTEXITCODE -ne 0) {
        throw 'Failed to restore repository .NET tools.'
    }

    dotnet tool run apicompat -- package $currentPackage --baseline-package $baselinePackage
    if ($LASTEXITCODE -ne 0) {
        throw 'Generated API contains a breaking change. Additive API changes remain allowed.'
    }
}
finally {
    Pop-Location
}
