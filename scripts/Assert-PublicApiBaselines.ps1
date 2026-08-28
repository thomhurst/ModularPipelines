[CmdletBinding()]
param(
    [string] $RepositoryRoot = (Split-Path $PSScriptRoot -Parent)
)

$ErrorActionPreference = 'Stop'
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

$missingBaselines = foreach ($project in $packageProjects) {
    $projectDirectory = Split-Path $project -Parent

    foreach ($fileName in 'PublicAPI.Shipped.txt', 'PublicAPI.Unshipped.txt') {
        $baselinePath = Join-Path $projectDirectory $fileName

        if (-not (Test-Path -LiteralPath $baselinePath -PathType Leaf)) {
            [System.IO.Path]::GetRelativePath($repositoryRootPath, $baselinePath)
        }
    }
}

if ($missingBaselines) {
    $missingList = $missingBaselines -join [Environment]::NewLine
    throw "Public API baseline coverage is incomplete:$([Environment]::NewLine)$missingList"
}

Write-Output "Verified public API baselines for $($packageProjects.Count) package projects."
