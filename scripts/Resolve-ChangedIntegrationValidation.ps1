[CmdletBinding()]
param(
    [Parameter(Mandatory)][AllowEmptyCollection()][string[]]$ChangedPath,
    [string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent),
    [bool]$IsGeneratedIntegration = $false
)

$ErrorActionPreference = 'Stop'
$projects = @()
if (-not $IsGeneratedIntegration) {
    # These projects are already covered by core/analyzer validation and use different layouts.
    $corePackages = @('ModularPipelines.Build', 'ModularPipelines.Cmd',
        'ModularPipelines.SourceGenerator', 'ModularPipelines.Analyzers',
        'ModularPipelines.Development.Analyzers')
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
            # A completely deleted package has no remaining project to build.
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
}

[pscustomobject]@{
    Required = $projects.Count -gt 0
    Matrix = @{ include = $projects }
}
