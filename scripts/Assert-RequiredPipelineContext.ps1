#!/usr/bin/env pwsh
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

    [Parameter(Mandatory)]
    [AllowEmptyString()]
    [string]$RunIntegrationValidation,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$IntegrationResult,

    [AllowEmptyString()]
    [string]$Distributed = 'false',

    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$WorkerPipelineResult = 'skipped',

    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$CrossPlatformBuildResult = 'skipped',

    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$ArtifactStorageResult = 'skipped',

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$AnalyzersResult,

    [Parameter(Mandatory)]
    [ValidateSet('success', 'failure', 'cancelled', 'skipped')]
    [string]$TrimAotResult,

    [AllowEmptyString()]
    [string]$RunDistributedTemplate = 'false',
    [AllowEmptyString()]
    [string]$DistributedTemplateResult = 'skipped',
    [AllowEmptyString()]
    [string]$RunGoValidation = 'false',
    [AllowEmptyString()]
    [string]$GoValidationResult = 'skipped',
    [AllowEmptyString()]
    [string]$RunDocsDeploy = 'false',
    [AllowEmptyString()]
    [string]$DocsDeployResult = 'skipped'
)

$ErrorActionPreference = 'Stop'

if ($FastFailResult -ne 'success') {
    throw "Fast-fail result was '$FastFailResult'; required context cannot pass."
}

foreach ($validation in @(
        @{ Name = 'Distributed template smoke'; Required = $RunDistributedTemplate; Result = $DistributedTemplateResult },
        @{ Name = 'Go validation'; Required = $RunGoValidation; Result = $GoValidationResult },
        @{ Name = 'Docs deploy test'; Required = $RunDocsDeploy; Result = $DocsDeployResult }
    )) {
    if ($validation.Required -notin @('true', 'false')) {
        throw "$($validation.Name) routing value was '$($validation.Required)'; required context cannot pass."
    }
    $expected = if ($validation.Required -eq 'true') { 'success' } else { 'skipped' }
    if ($validation.Result -ne $expected) {
        throw "$($validation.Name) result was '$($validation.Result)'; expected '$expected'."
    }
}

if ($IsGeneratedIntegration -notin @('true', 'false')) {
    throw "Generated integration routing value was '$IsGeneratedIntegration'; required context cannot pass."
}

if ($RunFullPipeline -notin @('true', 'false')) {
    throw "Full pipeline routing value was '$RunFullPipeline'; required context cannot pass."
}

$expectedQualityResult = if ($RunFullPipeline -eq 'true') { 'success' } else { 'skipped' }
if ($AnalyzersResult -ne $expectedQualityResult) {
    throw "Analyzers result was '$AnalyzersResult'; expected '$expectedQualityResult'."
}
if ($TrimAotResult -ne $expectedQualityResult) {
    throw "Trim/AOT result was '$TrimAotResult'; expected '$expectedQualityResult'."
}

$expectedArtifactStorageResult = if ($RunFullPipeline -eq 'true') { 'success' } else { 'skipped' }
if ($ArtifactStorageResult -ne $expectedArtifactStorageResult) {
    throw "Artifact storage result was '$ArtifactStorageResult'; expected '$expectedArtifactStorageResult'."
}

if ($RunIntegrationValidation -notin @('true', 'false')) {
    throw "Changed integration routing value was '$RunIntegrationValidation'; required context cannot pass."
}
if ($IsGeneratedIntegration -eq 'true' -and $RunIntegrationValidation -eq 'true') {
    throw 'Generated integration validation cannot also select the changed-package matrix.'
}
$expectedIntegrationResult = if ($RunIntegrationValidation -eq 'true') { 'success' } else { 'skipped' }
if ($IntegrationResult -ne $expectedIntegrationResult) {
    throw "Changed integration result was '$IntegrationResult'; expected '$expectedIntegrationResult'."
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
