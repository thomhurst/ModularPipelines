[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$RepositoryRoot,
    [Parameter(Mandatory)][string]$Tool,
    [Parameter(Mandatory)][string]$Package,
    [Parameter(Mandatory)][string]$NamespacePrefix,
    [string]$CurrentBase = 'origin/main'
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'GeneratedOptionsProvenance.ps1')

Assert-GeneratedOptionsToken -Name Tool -Value $Tool
Assert-GeneratedOptionsToken -Name Package -Value $Package
Assert-GeneratedOptionsToken -Name NamespacePrefix -Value $NamespacePrefix
$provenancePath = Join-Path $RepositoryRoot "src/$Package/Generated/$NamespacePrefix.Generation.json"
if (-not (Test-Path -LiteralPath $provenancePath -PathType Leaf)) {
    throw "Generated snapshots for '$Tool' have no provenance. Rerun Generate CLI Options from current '$CurrentBase'; do not rebase old generated commits."
}

$provenance = Get-Content -LiteralPath $provenancePath -Raw | ConvertFrom-Json
if ($provenance.formatVersion -ne 1 -or
    -not [string]::Equals(
        [string]$provenance.toolName,
        $Tool,
        [StringComparison]::OrdinalIgnoreCase) -or
    $provenance.generatorSourceSha256 -notmatch '^[a-f0-9]{64}$') {
    throw "Generated snapshots for '$Tool' contain invalid provenance. Rerun Generate CLI Options from current '$CurrentBase'."
}

$coveragePath = Join-Path $RepositoryRoot "src/$Package/Generated/$NamespacePrefix.CommandCoverage.json"
if (-not (Test-Path -LiteralPath $coveragePath -PathType Leaf)) {
    throw "Command coverage manifest does not exist for '$Tool': $coveragePath"
}

$coverage = Get-Content -LiteralPath $coveragePath -Raw | ConvertFrom-Json
if ($provenance.toolVersion -ne $coverage.toolVersion -or
    $provenance.commandTreeSha256 -ne $coverage.commandTreeSha256) {
    throw "Generated snapshots for '$Tool' do not match their command coverage manifest. Rerun Generate CLI Options."
}

$currentFingerprint = Get-GeneratedOptionsSourceFingerprint `
    -RepositoryRoot $RepositoryRoot `
    -Revision $CurrentBase
if ($provenance.generatorSourceSha256 -ne $currentFingerprint) {
    throw "Generated snapshots for '$Tool' are stale. Generator inputs changed after this snapshot was produced. Rerun Generate CLI Options from current '$CurrentBase'; do not rebase old generated commits."
}

Write-Host "Generated snapshots for '$Tool' match the generator on '$CurrentBase'."
