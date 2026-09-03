$ErrorActionPreference = 'Stop'

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$assertScript = Join-Path $PSScriptRoot 'Assert-RequiredPipelineContext.ps1'

function Assert-RoutePasses {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][AllowEmptyString()][string]$IsGeneratedIntegration,
        [Parameter(Mandatory)][string]$FastFailResult,
        [Parameter(Mandatory)][string]$FullPipelineResult,
        [Parameter(Mandatory)][string]$GeneratedIntegrationResult
    )

    & $assertScript `
        -IsGeneratedIntegration $IsGeneratedIntegration `
        -FastFailResult $FastFailResult `
        -FullPipelineResult $FullPipelineResult `
        -GeneratedIntegrationResult $GeneratedIntegrationResult
}

function Assert-RouteFails {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][AllowEmptyString()][string]$IsGeneratedIntegration,
        [Parameter(Mandatory)][string]$FastFailResult,
        [Parameter(Mandatory)][string]$FullPipelineResult,
        [Parameter(Mandatory)][string]$GeneratedIntegrationResult
    )

    try {
        Assert-RoutePasses @PSBoundParameters
    }
    catch {
        return
    }

    throw "Route '$Name' unexpectedly passed."
}

Assert-RoutePasses `
    -Name 'normal pull request' `
    -IsGeneratedIntegration false `
    -FastFailResult success `
    -FullPipelineResult success `
    -GeneratedIntegrationResult skipped
Assert-RoutePasses `
    -Name 'generated integration pull request' `
    -IsGeneratedIntegration true `
    -FastFailResult success `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult success
Assert-RouteFails `
    -Name 'generated validation skipped' `
    -IsGeneratedIntegration true `
    -FastFailResult success `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult skipped
Assert-RouteFails `
    -Name 'generated validation failed' `
    -IsGeneratedIntegration true `
    -FastFailResult success `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult failure
Assert-RouteFails `
    -Name 'generated validation canceled' `
    -IsGeneratedIntegration true `
    -FastFailResult success `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult cancelled
Assert-RouteFails `
    -Name 'fast-fail failed' `
    -IsGeneratedIntegration false `
    -FastFailResult failure `
    -FullPipelineResult failure `
    -GeneratedIntegrationResult skipped
Assert-RouteFails `
    -Name 'full pipeline failed' `
    -IsGeneratedIntegration false `
    -FastFailResult success `
    -FullPipelineResult failure `
    -GeneratedIntegrationResult skipped
Assert-RouteFails `
    -Name 'full pipeline canceled' `
    -IsGeneratedIntegration false `
    -FastFailResult success `
    -FullPipelineResult cancelled `
    -GeneratedIntegrationResult skipped
Assert-RouteFails `
    -Name 'routing output missing after fast-fail success' `
    -IsGeneratedIntegration '' `
    -FastFailResult success `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult skipped
Assert-RouteFails `
    -Name 'routing output missing after fast-fail failure' `
    -IsGeneratedIntegration '' `
    -FastFailResult failure `
    -FullPipelineResult skipped `
    -GeneratedIntegrationResult skipped

$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$requiredJob = [regex]::Match(
    $workflow,
    '(?ms)^  required-pipeline:.*?(?=^  [a-z0-9-]+:|\z)').Value
if ([string]::IsNullOrWhiteSpace($requiredJob)) {
    throw 'Required pipeline aggregate job was not found.'
}

foreach ($requiredText in @(
             'name: pipeline (ubuntu-latest)',
             'needs: [fast-fail, pipeline, generated-integration]',
             'if: always()',
             './scripts/Assert-RequiredPipelineContext.ps1'
         )) {
    if (-not $requiredJob.Contains($requiredText, [StringComparison]::Ordinal)) {
        throw "Required pipeline aggregate omitted '$requiredText'."
    }
}

$fullPipelineJob = [regex]::Match(
    $workflow,
    '(?ms)^  pipeline:.*?(?=^  [a-z0-9-]+:)').Value
if (-not $fullPipelineJob.Contains(
        'name: full pipeline (${{ matrix.os }})',
        [StringComparison]::Ordinal)) {
    throw 'The full pipeline job must not emit the required aggregate context directly.'
}

Write-Host 'Required pipeline context tests passed.'
