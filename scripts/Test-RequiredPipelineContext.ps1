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
             '-RunFullPipeline ''${{ needs.fast-fail.outputs.run_full_pipeline }}''',
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
