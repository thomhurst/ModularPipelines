<#
.SYNOPSIS
Builds local v4 packages and validates TUnit's migrated consumer pipeline without publishing.
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$ConsumerPath,
    [Parameter(Mandatory)]
    [string]$EvidenceDirectory
)

$ErrorActionPreference = 'Stop'
$frameworkRoot = Split-Path $PSScriptRoot -Parent
$ConsumerPath = (Resolve-Path -LiteralPath $ConsumerPath).Path
$EvidenceDirectory = [IO.Path]::GetFullPath($EvidenceDirectory)
$packages = Join-Path $EvidenceDirectory 'packages'
$guard = Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'
$version = "4.0.0-local-validation-$([Guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Force $packages | Out-Null

function Invoke-CheckedDotNet {
    param([string]$Name, [string[]]$Arguments)
    & $guard -SingleNode -TimeoutSeconds 600 -LogPath (Join-Path $EvidenceDirectory "$Name.log") -DotNetArguments $Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Validation '$Name' failed with exit code $LASTEXITCODE. See $EvidenceDirectory/$Name.log."
    }
}

Push-Location $frameworkRoot
try {
    # GitHub builds core and Git. DotNet is the only other integration this consumer uses.
    foreach ($tool in @('GitHub', 'DotNet')) {
        Invoke-CheckedDotNet "pack-$tool" @('pack', "src/ModularPipelines.$tool/ModularPipelines.$tool.csproj", '-c', 'Release', '-o', $packages, "-p:PackageVersion=$version", "-p:Version=$version")
    }
    foreach ($project in @('ModularPipelines', 'ModularPipelines.Git')) {
        Invoke-CheckedDotNet "pack-$project" @('pack', "src/$project/$project.csproj", '--no-build', '-c', 'Release', '-o', $packages, "-p:PackageVersion=$version", "-p:Version=$version")
    }
} finally {
    Pop-Location
}

Push-Location $ConsumerPath
try {
    $pipeline = 'tools/TUnit.Pipeline/TUnit.Pipeline.csproj'
    $validation = 'tools/TUnit.Pipeline.Validation/TUnit.Pipeline.Validation.csproj'
    Invoke-CheckedDotNet 'consumer-build' @('build', $validation, '-c', 'Release', "-p:ModularPipelinesVersion=$version", "-p:RestoreAdditionalProjectSources=$packages")
    Invoke-CheckedDotNet 'consumer-graph' @('run', '--project', $pipeline, '-c', 'Release', '--framework', 'net11.0', '--no-build', '--', '--validate-only')
    Invoke-CheckedDotNet 'consumer-behavior' @('run', '--project', $validation, '-c', 'Release', '--framework', 'net11.0', '--no-build')

    # Exercise the real entry point's nonzero exit on invalid configuration.
    $invalidLog = Join-Path $EvidenceDirectory 'consumer-invalid-category.log'
    & $guard -SingleNode -TimeoutSeconds 60 -LogPath $invalidLog -DotNetArguments @('run', '--project', $pipeline, '-c', 'Release', '--framework', 'net11.0', '--no-build', '--', '--categories', 'MigrationMissingCategory', '--validate-only')
    if ($LASTEXITCODE -ne 1 -or (Get-Content $invalidLog -Raw) -notmatch 'MigrationMissingCategory') {
        throw "The consumer entry point must reject an unknown category with exit code 1. See $invalidLog."
    }
} finally {
    Pop-Location
}
Write-Host "TUnit v4 migration validation passed. Logs: $EvidenceDirectory"
exit 0
