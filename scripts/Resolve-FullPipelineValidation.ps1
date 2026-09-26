[CmdletBinding(DefaultParameterSetName = 'Git')]
param(
    [Parameter(Mandatory)]
    [ValidateSet('pull_request', 'push', 'workflow_dispatch')]
    [string]$EventName,
    [Parameter(ParameterSetName = 'Git')][string]$BaseSha,
    [Parameter(ParameterSetName = 'Git')][string]$HeadSha = 'HEAD',
    [Parameter(ParameterSetName = 'Git')][string]$RepositoryRoot = (Split-Path $PSScriptRoot -Parent),
    [Parameter(Mandatory, ParameterSetName = 'Paths')][AllowEmptyCollection()][string[]]$ChangedPath,
    [string]$GitHubOutput
)

$ErrorActionPreference = 'Stop'

# Include the core's dependencies, tests, and shared build inputs. CLI integrations
# and the options generator have their own validation and do not require this suite.
$corePaths = @(
    'src/ModularPipelines/*',
    'src/ModularPipelines.Cmd/*',
    'src/ModularPipelines.SourceGenerator/*',
    'src/ModularPipelines.Analyzers/*',
    'src/ModularPipelines.Development.Analyzers/*',
    'test/ModularPipelines.UnitTests/*',
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
    'src/ModularPipelines.Build/*',
    '.github/workflows/dotnet.yml',
    'scripts/Resolve-FullPipelineValidation.ps1',
    'scripts/Test-FullPipelineValidation.ps1',
    'scripts/Assert-RequiredPipelineContext.ps1',
    'scripts/Test-RequiredPipelineContext.ps1',
    'scripts/Resolve-DistributedBuildMatrix.ps1',
    'scripts/Test-DistributedBuildMatrix.ps1',
    'scripts/Test-DistributedBuildArtifacts.ps1',
    'scripts/Write-DistributedBuildSummary.ps1',
    'scripts/Test-DistributedBuildSummary.ps1',
    'src/ModularPipelines.Distributed.Redis/*',
    'test/ModularPipelines.Distributed.Redis.UnitTests/*'
)

$runFullPipeline = $EventName -eq 'workflow_dispatch'
if (-not $runFullPipeline -and $PSCmdlet.ParameterSetName -eq 'Git') {
    if ($EventName -eq 'push' -and ([string]::IsNullOrWhiteSpace($BaseSha) -or $BaseSha -match '^0+$')) {
        # Initial pushes have no comparison commit; retain full validation.
        $runFullPipeline = $true
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

if ($GitHubOutput) {
    "run_full_pipeline=$($runFullPipeline.ToString().ToLowerInvariant())" |
        Add-Content -LiteralPath $GitHubOutput -Encoding utf8
}

Write-Host "Run full pipeline: $runFullPipeline"
return $runFullPipeline
