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
    return @($Lines | Where-Object {
        -not [string]::IsNullOrEmpty($_) -and
        $_.StartsWith('#', [System.StringComparison]::Ordinal)
    })
}

function Get-ApiEntries([string[]] $Lines) {
    return @($Lines | Where-Object {
        -not [string]::IsNullOrEmpty($_) -and
        -not $_.StartsWith('#', [System.StringComparison]::Ordinal)
    })
}

function Write-Baseline([string] $Path, [string[]] $Headers, [string[]] $Entries) {
    $uniqueEntries = [System.Collections.Generic.HashSet[string]]::new($comparer)
    foreach ($entry in $Entries) {
        if (-not [string]::IsNullOrEmpty($entry)) {
            $null = $uniqueEntries.Add($entry)
        }
    }

    $sortedEntries = [System.Collections.Generic.List[string]]::new($uniqueEntries)
    $sortedEntries.Sort($comparer)
    $lines = [System.Collections.Generic.List[string]]::new()
    foreach ($header in $Headers) {
        if (-not [string]::IsNullOrEmpty($header)) {
            $lines.Add($header)
        }
    }
    $lines.AddRange($sortedEntries)

    # Skip the write only when the file already holds exactly this content; a target
    # that does not exist yet is always created, even when there is nothing to list.
    if (Test-Path -LiteralPath $Path -PathType Leaf) {
        $existing = [System.Collections.Generic.List[string]]::new()
        foreach ($line in Get-Content -LiteralPath $Path) {
            $existing.Add($line)
        }

        if ([System.Linq.Enumerable]::SequenceEqual($existing, $lines, $comparer)) {
            return
        }
    }

    [System.IO.File]::WriteAllLines(
        $Path,
        $lines,
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
    if (-not $entry.StartsWith($removedPrefix, [System.StringComparison]::Ordinal)) {
        $null = $currentApis.Add($entry)
    }
}

$shippedApis = [System.Collections.Generic.HashSet[string]]::new($comparer)
foreach ($entry in Get-ApiEntries $originalShipped) {
    if ($currentApis.Contains($entry) -or -not $confirmedRemovedApis.Contains($entry)) {
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
    if (-not $currentApis.Contains($entry) -and $confirmedRemovedApis.Contains($entry)) {
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
