#!/usr/bin/env pwsh
[CmdletBinding(DefaultParameterSetName = 'Git')]
param(
    [Parameter(Mandatory)]
    [ValidateSet('pull_request', 'push', 'workflow_dispatch')]
    [string]$EventName,
    [Parameter(ParameterSetName = 'Git')][string]$BaseSha,
    [Parameter(ParameterSetName = 'Git')][string]$HeadSha = 'HEAD',
    [Parameter(ParameterSetName = 'Git')][string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent),
    [Parameter(Mandatory, ParameterSetName = 'Paths')][AllowEmptyCollection()][string[]]$ChangedPath,
    [ValidateSet('true', 'false')][string]$IsGeneratedIntegration = 'false',
    [string]$GitHubOutput
)

$ErrorActionPreference = 'Stop'

# Include the core's dependencies, tests, and shared build inputs. CLI integrations
# have a changed-package matrix; the options generator has its own validation.
$corePaths = @(
    'src/ModularPipelines/*',
    'src/ModularPipelines.Cmd/*',
    'src/ModularPipelines.SourceGenerator/*',
    'src/ModularPipelines.Analyzers/*',
    'src/ModularPipelines.Development.Analyzers/*',
    'test/ModularPipelines.UnitTests/*',
    'test/ModularPipelines.Consumer.UnitTests/*',
    'test/ModularPipelines.Cmd.UnitTests/*',
    'test/ModularPipelines.Distributed.UnitTests/*',
    'test/ModularPipelines.SourceGenerator.UnitTests/*',
    'test/ModularPipelines.Development.Analyzers.UnitTests/*',
    'test/ModularPipelines.TrimAotSmoke/*',
    'test/ModularPipelines.TestHelpers/*',
    'test/ModularPipelines.TestsForTests/*',
    'test/ModularPipelines.*.TestFixtures/*',
    'test/ModularPipelines.ProcessTestHost/*',
    'test/Shared/*',
    'test/GeneratedOptionsSmokeTests/*',
    'Directory.*.props',
    'Directory.*.targets',
    'src/Directory.*.props',
    'src/Directory.*.targets',
    'test/Directory.*.props',
    'test/Directory.*.targets',
    'global.json',
    'NuGet.Config',
    '.editorconfig',
    '.ruleset',
    'stylecop.json',
    'ModularPipelines.slnx',
    'ModularPipelines.Tests.slnf',
    # Changes to the suite itself must exercise the suite.
    'BuildSolutions.txt',
    'PackProjects.txt',
    'src/ModularPipelines.Build/*',
    '.github/workflows/dotnet.yml',
    '.github/actions/run-pipeline/*',
    'scripts/Resolve-FullPipelineValidation.ps1',
    'scripts/Test-FullPipelineValidation.ps1',
    'scripts/Resolve-ChangedIntegrationValidation.ps1',
    'scripts/Test-ChangedIntegrationValidation.ps1',
    'scripts/Assert-RequiredPipelineContext.ps1',
    'scripts/Test-RequiredPipelineContext.ps1',
    'scripts/Publish-Release.ps1',
    'scripts/Test-PublishRelease.ps1',
    '.github/scripts/release-gate.test.mjs',
    '.github/scripts/release-inputs.test.mjs',
    'scripts/Resolve-DistributedBuildMatrix.ps1',
    'scripts/Test-DistributedBuildMatrix.ps1',
    'scripts/Test-DistributedBuildArtifacts.ps1',
    'scripts/Write-DistributedBuildSummary.ps1',
    'scripts/Test-DistributedBuildSummary.ps1',
    'src/ModularPipelines.Distributed.Redis/*',
    'test/ModularPipelines.Distributed.Redis.UnitTests/*'
)

$supplementalPaths = [ordered]@{
    run_distributed_template = @(
        'src/ModularPipelines/*',
        'src/ModularPipelines.GitHub/*',
        'src/ModularPipelines.Distributed.Redis/*',
        'test/ModularPipelines.Distributed.SmokeTests/*',
        'test/ModularPipelines.GitHub.UnitTests/*',
        'Directory.*',
        'global.json',
        '.github/workflows/distributed-template-smoke.yml'
    )
    run_go_validation = @(
        'src/ModularPipelines.Go/*',
        'test/ModularPipelines.Go.UnitTests/*',
        '.github/workflows/go-validation.yml'
    )
    run_docs_deploy = @(
        'docs/*',
        '.github/workflows/deploy-pages.yml',
        '.github/workflows/deploy-pages-test.yml'
    )
}
# Changes to the orchestration itself must exercise every supplemental route.
$sharedSupplementalPaths = @(
    '.github/workflows/dotnet.yml',
    'scripts/Resolve-FullPipelineValidation.ps1',
    'scripts/Test-FullPipelineValidation.ps1',
    'scripts/Assert-RequiredPipelineContext.ps1',
    'scripts/Test-RequiredPipelineContext.ps1'
)

$runFullPipeline = $EventName -eq 'workflow_dispatch'
$runAllValidation = $runFullPipeline
if (-not $runFullPipeline -and $PSCmdlet.ParameterSetName -eq 'Git') {
    if ($EventName -eq 'push' -and ([string]::IsNullOrWhiteSpace($BaseSha) -or $BaseSha -match '^0+$')) {
        # Initial pushes have no comparison commit; retain full validation.
        $runFullPipeline = $true
        $runAllValidation = $true
    }
    else {
        if ([string]::IsNullOrWhiteSpace($BaseSha)) {
            throw 'A base commit is required to classify changes.'
        }

        if ($EventName -eq 'pull_request') {
            $BaseSha = git -C $RepositoryRoot merge-base $BaseSha $HeadSha
            if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($BaseSha)) {
                throw 'Unable to determine the pull request merge base.'
            }
        }

        # Include deletions and both sides of renames, even when a file leaves core.
        $ChangedPath = @(git -C $RepositoryRoot -c core.quotepath=false diff --name-only --no-renames $BaseSha $HeadSha)
        if ($LASTEXITCODE -ne 0) {
            throw 'Unable to determine changed paths.'
        }
    }
}

foreach ($path in $ChangedPath) {
    foreach ($pattern in $corePaths) {
        if ($path.Replace('\', '/') -like $pattern) {
            $runFullPipeline = $true
            break
        }
    }
}

if ($IsGeneratedIntegration -eq 'true' -and ($EventName -ne 'pull_request' -or $runFullPipeline)) {
    throw 'Focused generated integration validation requires an integration-only pull request.'
}
$integrations = & (Join-Path $PSScriptRoot 'Resolve-ChangedIntegrationValidation.ps1') `
    -ChangedPath @($ChangedPath | Where-Object { $_ }) `
    -RepositoryRoot $RepositoryRoot `
    -IsGeneratedIntegration ($IsGeneratedIntegration -eq 'true')
$runFullPipeline = $runFullPipeline -or $integrations.RequiresFullPipeline

$supplementalRoutes = [ordered]@{}
foreach ($route in $supplementalPaths.Keys) {
    $required = $runAllValidation
    foreach ($path in $ChangedPath) {
        foreach ($pattern in ($supplementalPaths[$route] + $sharedSupplementalPaths)) {
            if ($path.Replace('\', '/') -like $pattern) {
                $required = $true
                break
            }
        }
    }
    $supplementalRoutes[$route] = $required.ToString().ToLowerInvariant()
}

if ($GitHubOutput) {
    "run_full_pipeline=$($runFullPipeline.ToString().ToLowerInvariant())" |
        Add-Content -LiteralPath $GitHubOutput -Encoding utf8
    "run_integration_validation=$($integrations.Required.ToString().ToLowerInvariant())" |
        Add-Content -LiteralPath $GitHubOutput -Encoding utf8
    "integration_matrix=$($integrations.Matrix | ConvertTo-Json -Depth 4 -Compress)" |
        Add-Content -LiteralPath $GitHubOutput -Encoding utf8
    foreach ($route in $supplementalRoutes.Keys) {
        "$route=$($supplementalRoutes[$route])" | Add-Content -LiteralPath $GitHubOutput -Encoding utf8
    }
}

Write-Host "Run full pipeline: $runFullPipeline"
return $runFullPipeline
