$ErrorActionPreference = 'Stop'
$workflowPath = Join-Path (Split-Path $PSScriptRoot -Parent) '.github/workflows/dotnet.yml'
$workflow = Get-Content -LiteralPath $workflowPath -Raw
$artifactSteps = [regex]::Matches($workflow,
    '(?m)^      - (?:uses: actions/(?:upload|download)-artifact@|name:)[\s\S]*?(?=^      - |^  [a-z0-9-]+:|\z)') |
    Where-Object { $_.Value -match 'uses: actions/(upload|download)-artifact@' }
$uploads = @()
$downloads = @()
foreach ($step in $artifactSteps) {
    $name = [regex]::Match($step.Value, '(?m)^          name: (.+)$').Groups[1].Value.Trim()
    if ($step.Value -match 'uses: actions/upload-artifact@') {
        if (-not $name.Contains('${{ github.run_attempt }}', [StringComparison]::Ordinal)) {
            throw "Artifact '$name' must be attempt-scoped so full retries cannot collide with earlier uploads."
        }
        $uploads += $name
    }
    else { $downloads += $name }
}
if ($uploads.Count -ne 5 -or $downloads.Count -ne 2) {
    throw 'Expected the host, prerequisite results, and three pipeline output artifacts with two matching downloads.'
}
$producers = @(
    @{ Job = 'fast-fail'; Output = 'test_results_artifact' },
    @{ Job = 'pipeline-host'; Output = 'artifact_name' }
)
foreach ($producer in $producers) {
    $reference = '${{ needs.' + $producer.Job + '.outputs.' + $producer.Output + ' }}'
    if ($reference -cnotin $downloads) {
        throw "Downloads must use '$reference' so standalone partial retries retain the successful producer's artifact."
    }
    $job = [regex]::Match($workflow, '(?ms)^  ' + $producer.Job + ':.*?(?=^  [a-z0-9-]+:|\z)').Value
    $name = [regex]::Match($job, '(?m)^      ' + $producer.Output + ': (.+)$').Groups[1].Value.Trim()
    if ($name -cnotin $uploads) { throw "Producer output '$name' does not match an attempt-scoped upload." }
}
Write-Output 'Distributed artifacts: five retry-safe uploads and two producer-bound downloads passed.'
