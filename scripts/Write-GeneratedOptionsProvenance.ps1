[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$RepositoryRoot,
    [Parameter(Mandatory)][string]$Tool,
    [Parameter(Mandatory)][string]$PackageDirectory,
    [Parameter(Mandatory)][string]$NamespacePrefix,
    [Parameter(Mandatory)][string]$ChangeManifest
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'GeneratedOptionsProvenance.ps1')

Assert-GeneratedOptionsToken -Name Tool -Value $Tool
Assert-GeneratedOptionsToken -Name NamespacePrefix -Value $NamespacePrefix
$root = [IO.Path]::GetFullPath($RepositoryRoot)
$packagePath = [IO.Path]::GetFullPath((Join-Path $root $PackageDirectory))
$rootPrefix = $root.TrimEnd([IO.Path]::DirectorySeparatorChar) + [IO.Path]::DirectorySeparatorChar
if (-not $packagePath.StartsWith($rootPrefix, [StringComparison]::OrdinalIgnoreCase)) {
    throw "Package directory must stay within the repository: $PackageDirectory"
}

$coveragePath = Join-Path $packagePath "Generated/$NamespacePrefix.CommandCoverage.json"
if (-not (Test-Path -LiteralPath $coveragePath -PathType Leaf)) {
    throw "Command coverage manifest does not exist: $coveragePath"
}

$coverage = Get-Content -LiteralPath $coveragePath -Raw | ConvertFrom-Json
if (-not [string]::Equals(
        [string]$coverage.toolName,
        $Tool,
        [StringComparison]::OrdinalIgnoreCase)) {
    throw "Command coverage manifest belongs to '$($coverage.toolName)', not '$Tool'."
}
Assert-GeneratedOptionsCommandMetadata `
    -Name 'Command coverage manifest' `
    -ToolVersion ([string]$coverage.toolVersion) `
    -CommandTreeSha256 ([string]$coverage.commandTreeSha256)

$provenance = [ordered]@{
    formatVersion = 1
    toolName = $Tool
    toolVersion = $coverage.toolVersion
    commandTreeSha256 = $coverage.commandTreeSha256
    generatorSourceSha256 = Get-GeneratedOptionsSourceFingerprint -RepositoryRoot $root
}
$provenancePath = Join-Path $packagePath "Generated/$NamespacePrefix.Generation.json"
$provenance | ConvertTo-Json | Set-Content -LiteralPath $provenancePath -Encoding utf8

$relativePath = [IO.Path]::GetRelativePath($root, $provenancePath).Replace('\', '/')
Add-Content -LiteralPath $ChangeManifest -Value $relativePath
Write-Host "Recorded generated-options provenance: $relativePath"
