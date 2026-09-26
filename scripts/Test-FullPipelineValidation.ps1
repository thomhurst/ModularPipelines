$ErrorActionPreference = 'Stop'
$originalExitCode = $global:LASTEXITCODE
$resolveScript = Join-Path $PSScriptRoot 'Resolve-FullPipelineValidation.ps1'

function Assert-Route {
    param([string]$Name, [bool]$Expected, [hashtable]$Arguments)

    $actual = & $resolveScript @Arguments 6>$null
    if ($actual -isnot [bool] -or $actual -ne $Expected) {
        throw "Route '$Name': expected $Expected, received '$actual'."
    }
}

$corePaths = @(
    'src/ModularPipelines/Engine.cs',
    'src/ModularPipelines/ModularPipelines.csproj',
    'src/ModularPipelines.SourceGenerator/Generator.cs',
    'src/ModularPipelines.Analyzers/Analyzer.cs',
    'src/ModularPipelines.Development.Analyzers/Analyzer.cs',
    'src/ModularPipelines.Cmd/Command.cs',
    'test/ModularPipelines.UnitTests/EngineTests.cs',
    'test/ModularPipelines.TrimAotSmoke/Program.cs',
    'test/ModularPipelines.FSharp.TestFixtures/Module.fs',
    'test/Shared/GlobalTestSetup.cs',
    'Directory.Build.props',
    'Directory.Packages.props',
    'src/Directory.Build.props',
    'test/Directory.Build.props',
    'global.json',
    'NuGet.config',
    '.github/workflows/dotnet.yml',
    'src/ModularPipelines.Build/Modules/BuildSolutionsModule.cs',
    'scripts/Resolve-FullPipelineValidation.ps1',
    'scripts/Resolve-DistributedBuildMatrix.ps1',
    'scripts/Test-DistributedBuildMatrix.ps1',
    'scripts/Write-DistributedBuildSummary.ps1',
    'scripts/Test-DistributedBuildSummary.ps1',
    'src/ModularPipelines.Distributed.Redis/RedisDistributedExtensions.cs',
    'test/ModularPipelines.Distributed.Redis.UnitTests/DistributedBuildConfigurationTests.cs'
)
$unrelatedPaths = @(
    'src/ModularPipelines.Docker/Options/DockerBuildOptions.Generated.cs',
    'src/ModularPipelines.Docker/Generated/Docker.Generation.json',
    'test/ModularPipelines.Docker.UnitTests/DockerTests.cs',
    'tools/ModularPipelines.OptionsGenerator/src/Scraper.cs',
    'tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator.Tests/ScraperTests.cs',
    'tools/Directory.Build.props',
    'docs/docs/mp-packages/cli/docker.md',
    'README.md',
    '.github/workflows/generate-cli-options.yml',
    'scripts/Write-GeneratedOptionsProvenance.ps1'
)
foreach ($eventName in @('pull_request', 'push')) {
    foreach ($path in $corePaths) {
        Assert-Route $path $true @{ EventName = $eventName; ChangedPath = @($path) }
    }
    foreach ($path in $unrelatedPaths) {
        Assert-Route $path $false @{ EventName = $eventName; ChangedPath = @($path) }
    }
    Assert-Route 'mixed changes' $true @{ EventName = $eventName; ChangedPath = $unrelatedPaths + $corePaths[0] }
    Assert-Route 'no changes' $false @{ EventName = $eventName; ChangedPath = @() }
}
Assert-Route 'manual dispatch' $true @{ EventName = 'workflow_dispatch'; ChangedPath = $unrelatedPaths }
Assert-Route 'initial push' $true @{ EventName = 'push'; BaseSha = ('0' * 40) }

# Use a real history to catch diff-range mistakes that path-only tests cannot detect.
$temporaryRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$repository = Join-Path $temporaryRoot "full-pipeline-routing-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $repository | Out-Null
function Invoke-TestGit {
    param([Parameter(ValueFromRemainingArguments)][string[]]$Arguments)
    $output = & git -C $repository @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Test git command failed: $Arguments"
    }
    return $output
}
try {
    Invoke-TestGit init --quiet
    Invoke-TestGit config user.name 'CI routing test'
    Invoke-TestGit config user.email 'ci-routing@example.invalid'
    Invoke-TestGit config commit.gpgsign false
    New-Item -ItemType Directory -Path "$repository/src/ModularPipelines" -Force | Out-Null
    Set-Content "$repository/src/ModularPipelines/Core.cs" 'core'
    Set-Content "$repository/README.md" 'readme'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m initial
    $initial = Invoke-TestGit rev-parse HEAD

    # A rename out of core must still count the deleted core path.
    Invoke-TestGit mv src/ModularPipelines/Core.cs Moved.cs
    Invoke-TestGit commit --quiet -m rename
    $renamed = Invoke-TestGit rev-parse HEAD
    Assert-Route 'rename out of core' $true @{
        EventName = 'pull_request'; BaseSha = $initial; HeadSha = $renamed; RepositoryRoot = $repository
    }

    Set-Content "$repository/README.md" 'updated'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m docs
    $docs = Invoke-TestGit rev-parse HEAD
    Assert-Route 'multi-commit push includes earlier core change' $true @{
        EventName = 'push'; BaseSha = $initial; HeadSha = $docs; RepositoryRoot = $repository
    }
    Assert-Route 'docs push after core merge' $false @{
        EventName = 'push'; BaseSha = $renamed; HeadSha = $docs; RepositoryRoot = $repository
    }

    Invoke-TestGit checkout --quiet --detach $initial
    Set-Content "$repository/README.md" 'branch docs'
    Invoke-TestGit add .
    Invoke-TestGit commit --quiet -m branch-docs
    $branch = Invoke-TestGit rev-parse HEAD
    Assert-Route 'PR excludes core changes made only on base branch' $false @{
        EventName = 'pull_request'; BaseSha = $renamed; HeadSha = $branch; RepositoryRoot = $repository
    }

    Invoke-TestGit rm --quiet src/ModularPipelines/Core.cs
    Invoke-TestGit commit --quiet -m delete
    Assert-Route 'core deletion' $true @{
        EventName = 'push'; BaseSha = $branch; RepositoryRoot = $repository
    }

    foreach ($eventName in @('pull_request', 'push')) {
        $failed = $false
        try {
            & $resolveScript -EventName $eventName -BaseSha 'missing-commit' -RepositoryRoot $repository 2>$null
        }
        catch {
            $failed = $true
        }
        if (-not $failed) {
            throw "Invalid $eventName comparison must fail instead of silently skipping validation."
        }
    }

    $outputPath = Join-Path $repository 'github-output'
    Assert-Route 'GitHub output' $false @{
        EventName = 'push'; ChangedPath = $unrelatedPaths; GitHubOutput = $outputPath
    }
    if ((Get-Content $outputPath -Raw).Trim() -ne 'run_full_pipeline=false') {
        throw 'Expected a lowercase boolean GitHub output.'
    }
}
finally {
    $resolvedRepository = [IO.Path]::GetFullPath($repository)
    if (-not $resolvedRepository.StartsWith($temporaryRoot, [StringComparison]::OrdinalIgnoreCase) -or
        (Split-Path $resolvedRepository -Leaf) -notlike 'full-pipeline-routing-*') {
        throw 'Refusing to remove a test repository outside the temporary directory.'
    }
    Remove-Item -LiteralPath $resolvedRepository -Recurse -Force
    # Expected invalid-revision tests must not leave Git's failure code in the CI shell.
    $global:LASTEXITCODE = $originalExitCode
}

Write-Host 'Full pipeline validation tests passed.'
