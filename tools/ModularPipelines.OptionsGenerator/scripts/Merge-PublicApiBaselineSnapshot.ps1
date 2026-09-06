[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $OriginalShippedPath,
    [Parameter(Mandatory)][string] $OriginalUnshippedPath,
    [Parameter(Mandatory)][string] $CurrentApiSnapshotPath,
    [Parameter(Mandatory)][string] $ConfirmedRemovedApiPath,
    [Parameter(Mandatory)][string] $ShippedOutputPath,
    [Parameter(Mandatory)][string] $UnshippedOutputPath
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot '../../../scripts/PublicApiRemovedMarker.ps1')
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
$confirmedRemovedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in Get-ApiEntries (Read-Baseline $ConfirmedRemovedApiPath)) {
    $null = $confirmedRemovedApis.Add($entry)
}

$currentApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in Get-ApiEntries $currentSnapshot) {
    if (-not (Test-RemovedMarker $entry)) {
        $null = $currentApis.Add($entry)
    }
}

# Shipped entries are never dropped here: a *REMOVED* marker retires the entry
# until a release ships the unshipped baseline and collapses the pair.
$shippedApis = [System.Collections.Generic.HashSet[string]]::new(
    [string[]] @(Get-ApiEntries $originalShipped),
    $comparer)

$unshippedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in $currentApis) {
    if (-not $shippedApis.Contains($entry)) {
        $null = $unshippedApis.Add($entry)
    }
}

foreach ($entry in $shippedApis) {
    if (-not $currentApis.Contains($entry) -and $confirmedRemovedApis.Contains($entry)) {
        $null = $unshippedApis.Add((New-RemovedMarker $entry))
    }
}

# A marker only means something while its entry is still shipped and still absent.
foreach ($entry in Get-ApiEntries $originalUnshipped) {
    if (-not (Test-RemovedMarker $entry)) {
        continue
    }

    $removedApi = Get-RemovedMarkerEntry $entry
    if ($shippedApis.Contains($removedApi) -and -not $currentApis.Contains($removedApi)) {
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
