[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string]$RunFullPipeline,

    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string]$IsGeneratedIntegration,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$FastFailResult,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$FullPipelineResult,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$GeneratedIntegrationResult,

    [AllowEmptyString()]
    [string]$Distributed = 'false',

    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$WorkerPipelineResult = 'skipped',

    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$CrossPlatformBuildResult = 'skipped'
)

$ErrorActionPreference = 'Stop'

if ($FastFailResult -ne 'success') {
    throw "Fast-fail result was '$FastFailResult'; required context cannot pass."
}

if ($IsGeneratedIntegration -notin @('true', 'false')) {
    throw "Generated integration routing value was '$IsGeneratedIntegration'; required context cannot pass."
}

if ($RunFullPipeline -notin @('true', 'false')) {
    throw "Full pipeline routing value was '$RunFullPipeline'; required context cannot pass."
}

if ($RunFullPipeline -eq 'true' -and $Distributed -notin @('true', 'false')) {
    throw "Distributed routing value was '$Distributed'; required context cannot pass."
}

$expectedWorkerResult = if ($RunFullPipeline -eq 'true' -and $Distributed -eq 'true') { 'success' } else { 'skipped' }
if ($WorkerPipelineResult -ne $expectedWorkerResult) {
    throw "Worker pipeline result was '$WorkerPipelineResult'; expected '$expectedWorkerResult'."
}

$expectedCrossPlatformResult = if ($RunFullPipeline -eq 'true' -and $Distributed -eq 'false') { 'success' } else { 'skipped' }
if ($CrossPlatformBuildResult -ne $expectedCrossPlatformResult) {
    throw "Cross-platform build result was '$CrossPlatformBuildResult'; expected '$expectedCrossPlatformResult'."
}

if ($IsGeneratedIntegration -eq 'true') {
    if ($RunFullPipeline -ne 'false') {
        throw 'Generated integration validation cannot also select the full pipeline.'
    }

    if ($FullPipelineResult -ne 'skipped') {
        throw "Generated validation expected the full pipeline to be skipped, received '$FullPipelineResult'."
    }

    if ($GeneratedIntegrationResult -ne 'success') {
        throw "Generated integration result was '$GeneratedIntegrationResult'; required context cannot pass."
    }

    Write-Host 'Required pipeline context passed through generated integration validation.'
    return
}

$expectedFullPipelineResult = if ($RunFullPipeline -eq 'true') { 'success' } else { 'skipped' }
if ($FullPipelineResult -ne $expectedFullPipelineResult) {
    throw "Full pipeline result was '$FullPipelineResult'; required context cannot pass."
}

if ($GeneratedIntegrationResult -ne 'skipped') {
    throw "Non-generated validation expected generated integration to be skipped, received '$GeneratedIntegrationResult'."
}

Write-Host "Required pipeline context passed (full pipeline required: $RunFullPipeline)."
