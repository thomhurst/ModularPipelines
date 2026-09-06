[CmdletBinding()]
param(
    [string] $RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'PublicApiRemovedMarker.ps1')
$repositoryRootPath = (Resolve-Path -LiteralPath $RepositoryRoot).Path
$sourceRoot = Join-Path $repositoryRootPath 'src'

# Cover conventional standalone package solutions plus the release list's non-standard
# core and OptionsGenerator layouts. SourceGenerator and Analyzers are embedded Roslyn
# artifacts, not entries in FindProjectsModule's release project list; analyzer diagnostic
# compatibility is tracked separately by AnalyzerReleases.Shipped.md/Unshipped.md.
$packageProjects = @(
    Join-Path $sourceRoot 'ModularPipelines/ModularPipelines.csproj'
    Join-Path $sourceRoot 'ModularPipelines.Cmd/ModularPipelines.Cmd.csproj'

    Get-ChildItem -LiteralPath $sourceRoot -Directory -Filter 'ModularPipelines.*' |
        Where-Object {
            (Test-Path -LiteralPath (Join-Path $_.FullName "$($_.Name).csproj") -PathType Leaf) -and
            (Test-Path -LiteralPath (Join-Path $_.FullName "$($_.Name).slnx") -PathType Leaf)
        } |
        ForEach-Object { Join-Path $_.FullName "$($_.Name).csproj" }

    Join-Path $repositoryRootPath 'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj'
) | Sort-Object -Unique

$missingBaselines = [System.Collections.Generic.List[string]]::new()
$orphanedMarkers = [System.Collections.Generic.List[string]]::new()
$duplicateEntries = [System.Collections.Generic.List[string]]::new()

function Test-ApiEntry([string] $Line) {
    return $Line.Length -gt 0 -and -not $Line.StartsWith('#', [System.StringComparison]::Ordinal)
}

function Add-DuplicateEntries([string] $Path, [string[]] $Lines) {
    # PublicApiAnalyzers rejects a baseline that lists the same entry twice (RS0024).
    # Entries compare exactly, as the merge script and the analyzer do. Bulk-constructing
    # the set is cheap even for 100k-line files; only walk line by line when the counts
    # prove something repeats.
    $distinct = [System.Collections.Generic.HashSet[string]]::new($Lines, [System.StringComparer]::Ordinal)
    if ($distinct.Count -eq $Lines.Length) {
        return
    }

    $seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    foreach ($line in $Lines) {
        if ((Test-ApiEntry $line) -and -not $seen.Add($line)) {
            $duplicateEntries.Add("$([System.IO.Path]::GetRelativePath($repositoryRootPath, $Path)): $line")
        }
    }
}

foreach ($project in $packageProjects) {
    $projectDirectory = Split-Path $project -Parent
    $shippedPath = Join-Path $projectDirectory 'PublicAPI.Shipped.txt'
    $unshippedPath = Join-Path $projectDirectory 'PublicAPI.Unshipped.txt'
    $missing = @($shippedPath, $unshippedPath | Where-Object { -not (Test-Path -LiteralPath $_ -PathType Leaf) })
    if ($missing.Count -gt 0) {
        foreach ($path in $missing) {
            $missingBaselines.Add([System.IO.Path]::GetRelativePath($repositoryRootPath, $path))
        }

        continue
    }

    $shippedLines = [System.IO.File]::ReadAllLines($shippedPath)
    $unshippedLines = [System.IO.File]::ReadAllLines($unshippedPath)
    Add-DuplicateEntries $shippedPath $shippedLines
    Add-DuplicateEntries $unshippedPath $unshippedLines

    # A *REMOVED* marker retires a shipped entry until a release ships the unshipped
    # baseline, so every marker must still have its entry in PublicAPI.Shipped.txt. A
    # plain entry that is also still shipped is the RS0025 duplicate-symbol case.
    $shipped = [System.Collections.Generic.HashSet[string]]::new($shippedLines, [System.StringComparer]::Ordinal)
    $relativePath = [System.IO.Path]::GetRelativePath($repositoryRootPath, $unshippedPath)
    foreach ($line in $unshippedLines) {
        if (-not (Test-ApiEntry $line)) {
            continue
        }

        if (Test-RemovedMarker $line) {
            if (-not $shipped.Contains((Get-RemovedMarkerEntry $line))) {
                $orphanedMarkers.Add("${relativePath}: $line")
            }
        }
        elseif ($shipped.Contains($line)) {
            $duplicateEntries.Add("${relativePath}: $line (also in PublicAPI.Shipped.txt)")
        }
    }
}

if ($missingBaselines.Count -gt 0) {
    $missingList = $missingBaselines -join [Environment]::NewLine
    throw "Public API baseline coverage is incomplete:$([Environment]::NewLine)$missingList"
}

if ($orphanedMarkers.Count -gt 0) {
    $orphanList = $orphanedMarkers -join [Environment]::NewLine
    throw "Public API baselines contain *REMOVED* markers without a shipped entry:$([Environment]::NewLine)$orphanList"
}

if ($duplicateEntries.Count -gt 0) {
    $duplicateList = $duplicateEntries -join [Environment]::NewLine
    throw "Public API baselines list the same entry more than once:$([Environment]::NewLine)$duplicateList"
}

Write-Output "Verified public API baselines for $($packageProjects.Count) package projects."
