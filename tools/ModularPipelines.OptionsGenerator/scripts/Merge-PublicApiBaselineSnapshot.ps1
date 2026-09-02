[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $OriginalShippedPath,
    [Parameter(Mandatory)][string] $OriginalUnshippedPath,
    [Parameter(Mandatory)][string] $CurrentApiSnapshotPath,
    [Parameter(Mandatory)][string] $ShippedOutputPath,
    [Parameter(Mandatory)][string] $UnshippedOutputPath
)

$ErrorActionPreference = 'Stop'
$removedPrefix = '*REMOVED*'
$comparer = [System.StringComparer]::Ordinal

function Read-Baseline([string] $Path) {
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Public API baseline does not exist: $Path"
    }

    return @(Get-Content -LiteralPath $Path | Where-Object {
        -not [string]::IsNullOrWhiteSpace($_)
    })
}

function Get-Headers([string[]] $Lines) {
    return @($Lines | Where-Object { $_.StartsWith('#', [System.StringComparison]::Ordinal) })
}

function Get-ApiEntries([string[]] $Lines) {
    return @($Lines | Where-Object { -not $_.StartsWith('#', [System.StringComparison]::Ordinal) })
}

function Write-Baseline([string] $Path, [string[]] $Headers, [string[]] $Entries) {
    $uniqueEntries = [System.Collections.Generic.HashSet[string]]::new($Entries, $comparer)
    $sortedEntries = [string[]] @($uniqueEntries)
    [Array]::Sort($sortedEntries, $comparer)
    $lines = @($Headers) + $sortedEntries
    $existing = @(if (Test-Path -LiteralPath $Path -PathType Leaf) {
        Get-Content -LiteralPath $Path
    })

    if ([System.Linq.Enumerable]::SequenceEqual([string[]] $existing, [string[]] $lines, $comparer)) {
        return
    }

    [System.IO.File]::WriteAllLines(
        $Path,
        [string[]] $lines,
        [System.Text.UTF8Encoding]::new($false))
}

$originalShipped = Read-Baseline $OriginalShippedPath
$originalUnshipped = Read-Baseline $OriginalUnshippedPath
$currentSnapshot = Read-Baseline $CurrentApiSnapshotPath

$currentApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in Get-ApiEntries $currentSnapshot) {
    if (-not $entry.StartsWith($removedPrefix, [System.StringComparison]::Ordinal)) {
        $null = $currentApis.Add($entry)
    }
}

$shippedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in Get-ApiEntries $originalShipped) {
    if ($currentApis.Contains($entry)) {
        $null = $shippedApis.Add($entry)
    }
}

$unshippedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in $currentApis) {
    if (-not $shippedApis.Contains($entry)) {
        $null = $unshippedApis.Add($entry)
    }
}

foreach ($entry in Get-ApiEntries $originalShipped) {
    if (-not $currentApis.Contains($entry)) {
        $null = $unshippedApis.Add("$removedPrefix$entry")
    }
}

foreach ($entry in Get-ApiEntries $originalUnshipped) {
    if (-not $entry.StartsWith($removedPrefix, [System.StringComparison]::Ordinal)) {
        continue
    }

    $removedApi = $entry.Substring($removedPrefix.Length)
    if (-not $currentApis.Contains($removedApi)) {
        $null = $unshippedApis.Add($entry)
    }
}

Write-Baseline `
    -Path $ShippedOutputPath `
    -Headers (Get-Headers $originalShipped) `
    -Entries @($shippedApis)
Write-Baseline `
    -Path $UnshippedOutputPath `
    -Headers (Get-Headers $originalUnshipped) `
    -Entries @($unshippedApis)

Write-Output "Synchronized $($currentApis.Count) current public API entries."
