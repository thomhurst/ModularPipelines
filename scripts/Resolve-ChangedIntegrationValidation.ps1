[CmdletBinding()]
param(
    [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$ChangedPath,
    [string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent),
    [bool]$IsGeneratedIntegration = $false
)

$ErrorActionPreference = 'Stop'
$projects = @()
$removedPackages = [Collections.Generic.List[string]]::new()
if (-not $IsGeneratedIntegration) {
    # These projects are already covered by core/analyzer validation and use different layouts.
    $corePackages = @('ModularPipelines.Build', 'ModularPipelines.Cmd',
        'ModularPipelines.SourceGenerator', 'ModularPipelines.Analyzers',
        'ModularPipelines.Development.Analyzers', 'ModularPipelines.Distributed')
    $packages = @($ChangedPath | ForEach-Object {
        $path = $_.Replace('\', '/')
        if ($path -match '^src/(?<package>ModularPipelines\.[^/]+)/' -or
            $path -match '^test/(?<package>ModularPipelines\.[^/]+)\.UnitTests/') {
            if ($Matches.package -notin $corePackages) { $Matches.package }
        }
    } | Sort-Object -Unique)

    $projects = @(foreach ($package in $packages) {
        $directory = Join-Path $RepositoryRoot "src/$package"
        $testProject = "test/$package.UnitTests/$package.UnitTests.csproj"
        $hasTestProject = Test-Path -LiteralPath (Join-Path $RepositoryRoot $testProject) -PathType Leaf
        if (-not (Test-Path -LiteralPath $directory -PathType Container)) {
            if ($hasTestProject) {
                throw "Changed integration tests have no matching library directory: $testProject."
            }
            # The library is gone, but unchanged consumers may still reference it.
            $removedPackages.Add($package)
            continue
        }
        $project = "src/$package/$package.csproj"
        if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $project) -PathType Leaf)) {
            throw "Changed integration has no expected project: $project."
        }
        if (-not $hasTestProject) {
            $testProject = ''
        }
        [pscustomobject]@{ package = $package; project = $project; test_project = $testProject }
    })

    if ($removedPackages.Count -gt 0) {
        # Include consumers anywhere in the repository, including examples and build tools.
        # Inspect references instead of evaluating MSBuild during route classification.
        $consumers = @(Get-ChildItem -LiteralPath $RepositoryRoot -Recurse -File -Filter '*.csproj' |
            Where-Object { $_.FullName -notmatch '[\\/](?:bin|obj|node_modules|\.git)[\\/]' } |
            ForEach-Object {
                $projectFile = $_
                $document = [xml](Get-Content -LiteralPath $projectFile.FullName -Raw)
                $references = @($document.SelectNodes('//*[local-name()="ProjectReference"]/@Include') |
                    ForEach-Object { $_.Value -split ';' } |
                    ForEach-Object { ($_.Replace('\', '/') -split '/')[-1] })
                if (@($removedPackages | Where-Object { "$_.csproj" -in $references }).Count -gt 0) {
                    $relativeProject = [IO.Path]::GetRelativePath($RepositoryRoot, $projectFile.FullName).Replace('\', '/')
                    if ($relativeProject -notin $projects.project) {
                        [pscustomobject]@{
                            package = $projectFile.BaseName
                            project = $relativeProject
                            test_project = ''
                        }
                    }
                }
            })
        $projects = @(@($projects) + $consumers | Sort-Object -Property project -Unique)
    }
}

[pscustomobject]@{
    Required = $projects.Count -gt 0
    Matrix = @{ include = $projects }
}
