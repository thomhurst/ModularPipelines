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
    if (-not $name.Contains('${{ github.run_attempt }}', [StringComparison]::Ordinal)) {
        throw "Artifact '$name' must be attempt-scoped so full retries cannot collide with earlier uploads."
    }
    if ($step.Value -match 'uses: actions/upload-artifact@') { $uploads += $name }
    else { $downloads += $name }
}
if ($uploads.Count -ne 5 -or $downloads.Count -ne 2) {
    throw 'Expected the host, prerequisite results, and three pipeline output artifacts with two matching downloads.'
}
foreach ($download in $downloads) {
    if ($download -cnotin $uploads) { throw "Download '$download' has no matching upload in the same attempt." }
}
Write-Output 'Distributed artifacts: five retry-safe uploads and two matching downloads passed.'
