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
        # A changed solution cannot silently fall back to the remaining project after
        # deletion or rename. Whole-package removal is handled by full validation above.
        foreach ($path in $ChangedPath) {
            $solutionPath = $path.Replace('\', '/')
            if ($solutionPath -notlike "src/$package/*.slnx") { continue }
            if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $solutionPath) -PathType Leaf)) {
                throw "Changed integration solution is missing: $solutionPath."
            }
            if ($solutionPath -cne "src/$package/$package.slnx") {
                throw "Unsupported integration solution: $solutionPath. Expected src/$package/$package.slnx."
            }
        }
        if (-not $hasTestProject) {
            $testProject = ''
        }
        [pscustomobject]@{ package = $package; project = $project; test_project = $testProject }
    })

    if ($packages.Count -gt 0) {
        # Follow reverse references transitively, including consumers in examples and tools.
        # Inspect references instead of evaluating MSBuild during route classification.
        $affectedNames = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($package in $packages) { [void]$affectedNames.Add("$package.csproj") }
        $remaining = @(Get-ChildItem -LiteralPath $RepositoryRoot -Recurse -File -Filter '*.csproj' |
            Where-Object { $_.FullName -notmatch '[\\/](?:bin|obj|node_modules|\.git)[\\/]' } |
            ForEach-Object {
                $document = [xml](Get-Content -LiteralPath $_.FullName -Raw)
                [pscustomobject]@{
                    File = $_
                    References = @($document.SelectNodes('//*[local-name()="ProjectReference"]/@Include') |
                        ForEach-Object { $_.Value -split ';' } |
                        ForEach-Object { ($_.Replace('\', '/') -split '/')[-1] })
                }
            })
        do {
            $discovered = @($remaining | Where-Object {
                @($_.References | Where-Object { $affectedNames.Contains($_) }).Count -gt 0
            })
            foreach ($consumer in $discovered) {
                [void]$affectedNames.Add($consumer.File.Name)
                $relativeProject = [IO.Path]::GetRelativePath($RepositoryRoot, $consumer.File.FullName).Replace('\', '/')
                if ($relativeProject -notin $projects.project -and $relativeProject -notin $projects.test_project) {
                    $testProject = "test/$($consumer.File.BaseName).UnitTests/$($consumer.File.BaseName).UnitTests.csproj"
                    if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $testProject) -PathType Leaf)) { $testProject = '' }
                    $projects += [pscustomobject]@{
                        package = $consumer.File.BaseName
                        project = $relativeProject
                        test_project = $testProject
                    }
                }
            }
            $remaining = @($remaining | Where-Object { $_ -notin $discovered })
        } while ($discovered.Count -gt 0)
        $projects = @($projects | Sort-Object -Property project -Unique)
    }

    foreach ($entry in $projects) {
        $solution = "src/$($entry.package)/$($entry.package).slnx"
        # Build tools and arbitrary consumers keep their project target; integration
        # solutions validate their own project membership and XML as well as compilation.
        $buildTarget = if ($entry.package -notin $corePackages -and
            $entry.project -eq "src/$($entry.package)/$($entry.package).csproj" -and
            (Test-Path -LiteralPath (Join-Path $RepositoryRoot $solution) -PathType Leaf)) { $solution } else { $entry.project }
        $entry | Add-Member -NotePropertyName build_target -NotePropertyValue $buildTarget
    }

}

[pscustomobject]@{
    Required = $projects.Count -gt 0
    # Solution files and MSBuild imports may reference deleted packages without a ProjectReference.
    RequiresFullPipeline = $removedPackages.Count -gt 0
    Matrix = @{ include = $projects }
}
