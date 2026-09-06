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
    # Whitespace-only lines are blank, matching Read-Baseline in the merge script.
    return -not [string]::IsNullOrWhiteSpace($Line) -and -not $Line.StartsWith('#', [System.StringComparison]::Ordinal)
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

    # Markers belong in PublicAPI.Unshipped.txt only; one in PublicAPI.Shipped.txt is a stray
    # edit that the analyzer would read as a shipped symbol.
    $shippedRelativePath = [System.IO.Path]::GetRelativePath($repositoryRootPath, $shippedPath)
    foreach ($line in $shippedLines) {
        if ((Test-ApiEntry $line) -and (Test-RemovedMarker $line)) {
            $orphanedMarkers.Add("${shippedRelativePath}: $line (marker in PublicAPI.Shipped.txt)")
        }
    }

    # A *REMOVED* marker retires a shipped entry until a release ships the unshipped
    # baseline, so every marker must still have its entry in PublicAPI.Shipped.txt. A
    # plain entry that is also still shipped is the RS0025 duplicate-symbol case.
    $shipped = [System.Collections.Generic.HashSet[string]]::new($shippedLines, [System.StringComparer]::Ordinal)
    $relativePath = [System.IO.Path]::GetRelativePath($repositoryRootPath, $unshippedPath)
    $inspected = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    foreach ($line in $unshippedLines) {
        # Repeats within the file are already reported above; inspect each entry once so a
        # duplicated entry that is also shipped does not surface as several findings.
        if (-not (Test-ApiEntry $line) -or -not $inspected.Add($line)) {
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

# Report every populated category at once so one CI run shows every problem to fix.
$newLine = [Environment]::NewLine
$problems = [System.Collections.Generic.List[string]]::new()
if ($missingBaselines.Count -gt 0) {
    $problems.Add("Public API baseline coverage is incomplete:$newLine$($missingBaselines -join $newLine)")
}

if ($orphanedMarkers.Count -gt 0) {
    $problems.Add("Public API baselines contain *REMOVED* markers without a shipped entry:$newLine$($orphanedMarkers -join $newLine)")
}

if ($duplicateEntries.Count -gt 0) {
    $problems.Add("Public API baselines list the same entry more than once:$newLine$($duplicateEntries -join $newLine)")
}

if ($problems.Count -gt 0) {
    throw ($problems -join ($newLine + $newLine))
}

Write-Output "Verified public API baselines for $($packageProjects.Count) package projects."
