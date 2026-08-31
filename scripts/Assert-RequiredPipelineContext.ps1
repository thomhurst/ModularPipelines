[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateSet('true', 'false')]
    [string]$IsGeneratedIntegration,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$FastFailResult,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$FullPipelineResult,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$GeneratedIntegrationResult
)

$ErrorActionPreference = 'Stop'

if ($FastFailResult -ne 'success') {
    throw "Fast-fail result was '$FastFailResult'; required context cannot pass."
}

if ($IsGeneratedIntegration -eq 'true') {
    if ($FullPipelineResult -ne 'skipped') {
        throw "Generated validation expected the full pipeline to be skipped, received '$FullPipelineResult'."
    }

    if ($GeneratedIntegrationResult -ne 'success') {
        throw "Generated integration result was '$GeneratedIntegrationResult'; required context cannot pass."
    }

    Write-Host 'Required pipeline context passed through generated integration validation.'
    return
}

if ($FullPipelineResult -ne 'success') {
    throw "Full pipeline result was '$FullPipelineResult'; required context cannot pass."
}

if ($GeneratedIntegrationResult -ne 'skipped') {
    throw "Full validation expected generated integration to be skipped, received '$GeneratedIntegrationResult'."
}

Write-Host 'Required pipeline context passed through full validation.'
