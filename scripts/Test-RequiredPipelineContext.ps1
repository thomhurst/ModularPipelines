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
                    $passed = $true
                    try {
                        & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration $generated `
                            -FastFailResult $fastResult -FullPipelineResult $fullResult `
                            -GeneratedIntegrationResult $generatedResult *> $null
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

$workerCaseCount = 0
foreach ($runFull in @('true', 'false')) {
    foreach ($distributed in @('true', 'false', '', 'invalid')) {
        foreach ($workerResult in @('success', 'failure', 'cancelled', 'skipped')) {
            $fullResult = if ($runFull -eq 'true') { 'success' } else { 'skipped' }
            $shouldPass = ($runFull -eq 'false' -and $workerResult -eq 'skipped') -or
                ($runFull -eq 'true' -and $distributed -eq 'true' -and $workerResult -eq 'success') -or
                ($runFull -eq 'true' -and $distributed -eq 'false' -and $workerResult -eq 'skipped')
            $passed = $true
            try {
                & $assertScript -RunFullPipeline $runFull -IsGeneratedIntegration 'false' `
                    -FastFailResult 'success' -FullPipelineResult $fullResult -GeneratedIntegrationResult 'skipped' `
                    -Distributed $distributed -WorkerPipelineResult $workerResult *> $null
            }
            catch {
                $passed = $false
            }
            if ($passed -ne $shouldPass) {
                throw "Unexpected worker route: full=$runFull distributed=$distributed workers=$workerResult"
            }
            $workerCaseCount++
        }
    }
}
Write-Host "Required worker routing: $workerCaseCount cases passed."
$workflow = Get-Content -LiteralPath (Join-Path $repositoryRoot '.github/workflows/dotnet.yml') -Raw
$requiredJob = [regex]::Match(
    $workflow,
    '(?ms)^  required-pipeline:.*?(?=^  [a-z0-9-]+:|\z)').Value
if ([string]::IsNullOrWhiteSpace($requiredJob)) {
    throw 'Required pipeline aggregate job was not found.'
}

foreach ($requiredText in @(
             'name: pipeline (ubuntu-latest)',
             'needs: [fast-fail, pipeline, pipeline-workers, generated-integration]',
             'if: always()',
             '-RunFullPipeline ''${{ needs.fast-fail.outputs.run_full_pipeline }}''',
             '-Distributed ''${{ needs.fast-fail.outputs.distributed }}''',
             '-WorkerPipelineResult ''${{ needs.pipeline-workers.result }}''',
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
