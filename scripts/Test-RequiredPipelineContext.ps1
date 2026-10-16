$ErrorActionPreference = 'Stop'

$repositoryRoot = Split-Path $PSScriptRoot -Parent
$assertScript = Join-Path $PSScriptRoot 'Assert-RequiredPipelineContext.ps1'

# Only the selected validation route may pass. Missing routing outputs must fail closed.
$caseCount = 0
foreach ($runFull in @('true', 'false', '', 'invalid')) {
    foreach ($generated in @('true', 'false', '', 'invalid')) {
        foreach ($fastResult in @('success', 'failure', 'cancelled', 'skipped')) {
            foreach ($fullResult in @('success', 'failure', 'cancelled', 'skipped')) {
                foreach ($generatedResult in @('success', 'failure', 'cancelled', 'skipped')) {
                    $shouldPass = $fastResult -eq 'success' -and (
                        ($runFull -eq 'true' -and $generated -eq 'false' -and
                            $fullResult -eq 'success' -and $generatedResult -eq 'skipped') -or
                        ($runFull -eq 'false' -and $generated -eq 'true' -and
                            $fullResult -eq 'skipped' -and $generatedResult -eq 'success') -or
                        ($runFull -eq 'false' -and $generated -eq 'false' -and
                            $fullResult -eq 'skipped' -and $generatedResult -eq 'skipped'))
                    $crossPlatformResult = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
                    $passed = $true
                    try {
                        & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration $generated `
                            -FastFailResult $fastResult -FullPipelineResult $fullResult `
                            -GeneratedIntegrationResult $generatedResult -CrossPlatformBuildResult $crossPlatformResult `
                            -ArtifactStorageResult $crossPlatformResult `
                            -AnalyzersResult $crossPlatformResult -TrimAotResult $crossPlatformResult `
                            -RunIntegrationValidation 'false' -IntegrationResult 'skipped' *> $null
                    }
                    catch {
                        $passed = $false
                    }
                    if ($passed -ne $shouldPass) {
                        throw "Unexpected route result: full=$runFull generated=$generated fast=$fastResult fullResult=$fullResult generatedResult=$generatedResult"
                    }
                    $caseCount++
                }
            }
        }
    }
}
Write-Host "Required pipeline routing: $caseCount cases passed."

$runnerCaseCount = 0
foreach ($runFull in @('true', 'false')) {
    foreach ($distributed in @('true', 'false', '', 'invalid')) {
        foreach ($workerResult in @('success', 'failure', 'cancelled', 'skipped')) {
            foreach ($crossPlatformResult in @('success', 'failure', 'cancelled', 'skipped')) {
                $fullResult = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
                $shouldPass = ($runFull -eq 'false' -and $workerResult -eq 'skipped' -and $crossPlatformResult -eq 'skipped') -or
                    ($runFull -eq 'true' -and $distributed -eq 'true' -and $workerResult -eq 'success' -and $crossPlatformResult -eq 'skipped') -or
                    ($runFull -eq 'true' -and $distributed -eq 'false' -and $workerResult -eq 'skipped' -and $crossPlatformResult -eq 'success')
                $passed = $true
                try {
                    & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration 'false' `
                        -FastFailResult 'success' -FullPipelineResult $fullResult -GeneratedIntegrationResult 'skipped' `
                        -Distributed $distributed -WorkerPipelineResult $workerResult `
                        -CrossPlatformBuildResult $crossPlatformResult `
                        -ArtifactStorageResult $fullResult `
                        -AnalyzersResult $fullResult -TrimAotResult $fullResult `
                        -RunIntegrationValidation 'false' -IntegrationResult 'skipped' *> $null
                }
                catch {
                    $passed = $false
                }
                if ($passed -ne $shouldPass) {
                    throw "Unexpected runner route: full=$runFull distributed=$distributed workers=$workerResult crossPlatform=$crossPlatformResult"
                }
                $runnerCaseCount++
            }
        }
    }
}
Write-Host "Required runner routing: $runnerCaseCount cases passed."

$integrationCaseCount = 0
foreach ($runIntegration in @('true', 'false', '', 'invalid')) {
    foreach ($integrationResult in @('success', 'failure', 'cancelled', 'skipped')) {
        foreach ($route in @('full', 'generated', 'fast-only')) {
            $full = $route -eq 'full'
            $generated = $route -eq 'generated'
            $expectedResult = if ($runIntegration -eq 'true') { 'success' } else { 'skipped' }
            $shouldPass = $runIntegration -in @('true', 'false') -and
                -not ($generated -and $runIntegration -eq 'true') -and $integrationResult -eq $expectedResult
            $passed = $true
            try {
                & $assertScript -RunFullPipeline $full.ToString().ToLowerInvariant() `
                    -IsGeneratedIntegration $generated.ToString().ToLowerInvariant() `
                    -FastFailResult success -FullPipelineResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -CrossPlatformBuildResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -ArtifactStorageResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -AnalyzersResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -TrimAotResult $(if ($full) { 'success' } else { 'skipped' }) `
                    -GeneratedIntegrationResult $(if ($generated) { 'success' } else { 'skipped' }) `
                    -RunIntegrationValidation $runIntegration -IntegrationResult $integrationResult *> $null
            }
            catch { $passed = $false }
            if ($passed -ne $shouldPass) {
                throw "Unexpected integration gate: route=$route required=$runIntegration result=$integrationResult."
            }
            $integrationCaseCount++
        }
    }
}
Write-Host "Required integration routing: $integrationCaseCount cases passed."

foreach ($runFull in @('true', 'false')) {
    foreach ($storageResult in @('success', 'failure', 'cancelled', 'skipped')) {
        $expected = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
        $passed = $true
        try {
            & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration 'false' `
                -FastFailResult success -FullPipelineResult $expected -GeneratedIntegrationResult skipped `
                -CrossPlatformBuildResult $expected -ArtifactStorageResult $storageResult `
                -AnalyzersResult $expected -TrimAotResult $expected `
                -RunIntegrationValidation false -IntegrationResult skipped *> $null
        }
        catch { $passed = $false }
        if ($passed -ne ($storageResult -eq $expected)) {
            throw "Unexpected artifact storage gate: full=$runFull storage=$storageResult."
        }
    }
}

$qualityCaseCount = 0
foreach ($route in @('full', 'distributed', 'generated', 'integration', 'fast-only')) {
    foreach ($analyzersResult in @('success', 'failure', 'cancelled', 'skipped', '', 'invalid')) {
        foreach ($trimAotResult in @('success', 'failure', 'cancelled', 'skipped', '', 'invalid')) {
            $runFull = $route -in @('full', 'distributed')
            $expected = if ($runFull) { 'success' } else { 'skipped' }
            $passed = $true
            try {
                & $assertScript -RunFullPipeline $runFull.ToString().ToLowerInvariant() `
                    -IsGeneratedIntegration ($route -eq 'generated').ToString().ToLowerInvariant() `
                    -FastFailResult success -FullPipelineResult $expected `
                    -Distributed ($route -eq 'distributed').ToString().ToLowerInvariant() `
                    -WorkerPipelineResult $(if ($route -eq 'distributed') { 'success' } else { 'skipped' }) `
                    -CrossPlatformBuildResult $(if ($route -eq 'full') { 'success' } else { 'skipped' }) `
                    -ArtifactStorageResult $expected `
                    -GeneratedIntegrationResult $(if ($route -eq 'generated') { 'success' } else { 'skipped' }) `
                    -RunIntegrationValidation ($route -eq 'integration').ToString().ToLowerInvariant() `
                    -IntegrationResult $(if ($route -eq 'integration') { 'success' } else { 'skipped' }) `
                    -AnalyzersResult $analyzersResult -TrimAotResult $trimAotResult *> $null
            }
            catch { $passed = $false }
            if ($passed -ne ($analyzersResult -eq $expected -and $trimAotResult -eq $expected)) {
                throw "Unexpected quality gate: route=$route analyzers=$analyzersResult trimAot=$trimAotResult."
            }
            $qualityCaseCount++
        }
    }
}
Write-Host "Required analyzer and trim/AOT routing: $qualityCaseCount cases passed."

$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$requiredJob = [regex]::Match(
    $workflow,
    '(?ms)^  required-pipeline:.*?(?=^  [a-z0-9-]+:|\z)').Value
if ([string]::IsNullOrWhiteSpace($requiredJob)) {
    throw 'Required pipeline aggregate job was not found.'
}

foreach ($requiredText in @(
             'name: pipeline (ubuntu-latest)',
             'needs: [fast-fail, pipeline, pipeline-workers, cross-platform-build, generated-integration, changed-integration, artifact-storage, analyzers, trim-aot]',
             'if: always()',
             '-RunFullPipeline ''${{ needs.fast-fail.outputs.run_full_pipeline }}''',
             '-Distributed ''${{ needs.fast-fail.outputs.distributed }}''',
             '-WorkerPipelineResult ''${{ needs.pipeline-workers.result }}''',
             '-CrossPlatformBuildResult ''${{ needs.cross-platform-build.result }}''',
             '-ArtifactStorageResult ''${{ needs.artifact-storage.result }}''',
             '-AnalyzersResult ''${{ needs.analyzers.result }}''',
             '-TrimAotResult ''${{ needs.trim-aot.result }}''',
             '-RunIntegrationValidation ''${{ needs.fast-fail.outputs.run_integration_validation }}''',
             '-IntegrationResult ''${{ needs.changed-integration.result }}''',
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
        'name: full pipeline (${{ matrix.os }}, instance ${{ matrix.instance }})',
        [StringComparison]::Ordinal)) {
    throw 'The full pipeline job must not emit the required aggregate context directly.'
}

$workerJob = [regex]::Match(
    $workflow,
    '(?ms)^  pipeline-workers:.*?(?=^  [a-z0-9-]+:)').Value
if ($workerJob -notmatch '(?m)^    permissions:\s*\r?\n      contents: read\s*\r?\n    timeout-minutes:' -or
    -not $workerJob.Contains('steps: *pipeline-steps', [StringComparison]::Ordinal) -or
    -not $workerJob.Contains("needs.fast-fail.outputs.distributed == 'true'", [StringComparison]::Ordinal)) {
    throw 'Distributed workers must share pipeline steps with read-only repository permissions and no identity-token grant.'
}
if (-not $fullPipelineJob.Contains('steps: &pipeline-steps', [StringComparison]::Ordinal) -or
    -not $fullPipelineJob.Contains('id-token: write', [StringComparison]::Ordinal)) {
    throw 'The master must retain the shared steps and publishing identity permission.'
}

Write-Host 'Required pipeline context tests passed.'
