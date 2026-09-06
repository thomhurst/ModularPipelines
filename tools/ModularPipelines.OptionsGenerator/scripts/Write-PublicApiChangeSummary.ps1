[CmdletBinding()]
param(
    [Parameter(Mandatory)][string] $OriginalShippedPath,
    [Parameter(Mandatory)][string] $OriginalUnshippedPath,
    [Parameter(Mandatory)][string] $CurrentShippedPath,
    [Parameter(Mandatory)][string] $CurrentUnshippedPath,
    [Parameter(Mandatory)][string] $PackageDirectory,
    [Parameter(Mandatory)][string] $OutputPath,
    [ValidateRange(1, 20)][int] $MaximumExamples = 5
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

foreach ($path in @(
    $OriginalShippedPath,
    $OriginalUnshippedPath,
    $CurrentShippedPath,
    $CurrentUnshippedPath)) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Public API baseline does not exist: $path"
    }
}

if (-not (Test-Path -LiteralPath $PackageDirectory -PathType Container)) {
    throw "Package directory does not exist: $PackageDirectory"
}

. (Join-Path $PSScriptRoot '../../../scripts/PublicApiRemovedMarker.ps1')

function Read-ActiveApi([string[]] $Paths) {
    # A shipped entry stays in PublicAPI.Shipped.txt after removal; the matching
    # *REMOVED* marker in PublicAPI.Unshipped.txt is what takes it out of the active surface.
    $entries = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $retired = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($path in $Paths) {
        foreach ($line in Get-Content -LiteralPath $path) {
            $entry = $line.Trim()
            if (($entry.Length -eq 0) -or $entry.StartsWith('#', [StringComparison]::Ordinal)) {
                continue
            }

            if (Test-RemovedMarker $entry) {
                [void] $retired.Add((Get-RemovedMarkerEntry $entry))
            } else {
                [void] $entries.Add($entry)
            }
        }
    }

    $entries.ExceptWith($retired)
    return ,$entries
}

function Get-ApiFamily([string] $Api, [string[]] $Prefixes) {
    $match = [regex]::Match(
        $Api,
        '^ModularPipelines\.[^.]+\.(?:Enums|Extensions|Models|Options|Services)\.(?<type>[A-Za-z_][A-Za-z0-9_]*)')
    if (-not $match.Success) {
        return 'Assembly/common'
    }

    $typeName = $match.Groups['type'].Value
    foreach ($prefix in $Prefixes) {
        if ($typeName.StartsWith($prefix, [StringComparison]::OrdinalIgnoreCase) -or
            ($typeName.StartsWith('I', [StringComparison]::Ordinal) -and
                $typeName.Substring(1).StartsWith($prefix, [StringComparison]::OrdinalIgnoreCase))) {
            return $prefix
        }
    }

    return 'Assembly/common'
}

function Get-MemberKey([string] $Api) {
    $arrowIndex = $Api.IndexOf(' -> ', [StringComparison]::Ordinal)
    $declaration = if ($arrowIndex -ge 0) { $Api.Substring(0, $arrowIndex) } else { $Api }
    $parameterIndex = $declaration.IndexOf('(', [StringComparison]::Ordinal)
    if ($parameterIndex -ge 0) {
        return $declaration.Substring(0, $parameterIndex)
    }

    return $declaration
}

$oldApi = Read-ActiveApi @($OriginalShippedPath, $OriginalUnshippedPath)
$currentApi = Read-ActiveApi @($CurrentShippedPath, $CurrentUnshippedPath)
$added = @($currentApi.Where({ -not $oldApi.Contains($_) }) | Sort-Object)
$removed = @($oldApi.Where({ -not $currentApi.Contains($_) }) | Sort-Object)

$coverageDirectory = Join-Path $PackageDirectory 'Generated'
$prefixLabels = @{}
if (Test-Path -LiteralPath $coverageDirectory -PathType Container) {
    foreach ($coverageFile in Get-ChildItem -LiteralPath $coverageDirectory -Filter '*.CommandCoverage.json' -File) {
        $prefix = $coverageFile.BaseName -replace '\.CommandCoverage$', ''
        $metadata = Get-Content -LiteralPath $coverageFile.FullName -Raw | ConvertFrom-Json
        $toolName = [string] $metadata.toolName
        $prefixLabels[$prefix] = if ($toolName -and -not $prefix.Equals($toolName, [StringComparison]::OrdinalIgnoreCase)) {
            "$prefix ($toolName)"
        } else {
            $prefix
        }
    }
}
$prefixes = @($prefixLabels.Keys |
    Sort-Object -Property @{ Expression = { $_.Length }; Descending = $true }, @{ Expression = { $_ } })

$families = @($added + $removed |
    ForEach-Object { Get-ApiFamily $_ $prefixes } |
    Sort-Object -Unique)
$removedKeys = [Collections.Generic.HashSet[string]]::new(
    [string[]] @($removed | ForEach-Object { Get-MemberKey $_ }),
    [StringComparer]::Ordinal)
$changedSignatureCount = @($added | Where-Object { $removedKeys.Contains((Get-MemberKey $_)) }).Count

$lines = [Collections.Generic.List[string]]::new()
$lines.Add('## Assembly-wide public API impact')
$lines.Add('')
if ($added.Count -eq 0 -and $removed.Count -eq 0) {
    $lines.Add('No active public API changes were detected in this assembly.')
} else {
    $familyList = ($families | ForEach-Object {
        $label = if ($prefixLabels.ContainsKey($_)) { $prefixLabels[$_] } else { $_ }
        "``$label``"
    }) -join ', '
    $lines.Add("Affected API families: $familyList.")
    $lines.Add('')
    $lines.Add("- Added APIs: $($added.Count)")
    $lines.Add("- Removed or changed APIs: $($removed.Count)")
    $lines.Add("- Members with matching names but changed signatures: $changedSignatureCount")

    if ($removed.Count -gt 0) {
        $lines.Add('')
        $lines.Add('Breaking changes are present. Consumers may need to update method arguments, option property types or nullability, enum members, and references to removed APIs.')
        $lines.Add('')
        $lines.Add('Representative removed or changed members:')
        foreach ($api in $removed | Select-Object -First $MaximumExamples) {
            $lines.Add("- ``$api``")
        }
    }

    if ($added.Count -gt 0) {
        $lines.Add('')
        $lines.Add('Representative added members:')
        foreach ($api in $added | Select-Object -First $MaximumExamples) {
            $lines.Add("- ``$api``")
        }
    }
}

$outputDirectory = Split-Path -Parent $OutputPath
if ($outputDirectory) {
    [IO.Directory]::CreateDirectory($outputDirectory) | Out-Null
}
[IO.File]::WriteAllLines($OutputPath, $lines, [Text.UTF8Encoding]::new($false))
Write-Output "Wrote public API change summary to $OutputPath"
