[CmdletBinding()]
param(
    [Parameter(Mandatory)] [string]$ReportPath,
    [Parameter(Mandatory)] [string]$SummaryPath
)

$ErrorActionPreference = 'Stop'
if (-not (Test-Path -LiteralPath $ReportPath)) {
    Write-Warning 'No pipeline report was written; inspect the pipeline logs for startup failures.'
    return
}

$report = Get-Content -LiteralPath $ReportPath -Raw | ConvertFrom-Json
$minutes = [Math]::Round([TimeSpan]::Parse($report.totalDuration).TotalMinutes, 2)
$lines = [System.Collections.Generic.List[string]]::new()
$lines.Add('## Pipeline efficiency')
$lines.Add('')
$lines.Add("Pipeline wall-clock duration: $minutes minutes. Restore, host preparation, and standalone prebuild time are excluded.")
$lines.Add('')
if ($report.distributed) {
    $lines.Add('| Instance | Modules | Busy (min) | Idle (min) | Utilization | Queue wait total (s) | Queue wait max (s) |')
    $lines.Add('| --- | --- | --- | --- | --- | --- | --- |')
    foreach ($worker in $report.distributed.workers | Sort-Object workerIndex) {
        $waits = @($report.distributed.modules | Where-Object workerIndex -eq $worker.workerIndex |
            ForEach-Object { [TimeSpan]::Parse($_.queueWaitDuration).TotalSeconds })
        $wait = $waits | Measure-Object -Sum -Maximum
        $busy = [Math]::Round([TimeSpan]::Parse($worker.busyDuration).TotalMinutes, 2)
        $idle = [Math]::Round([TimeSpan]::Parse($worker.idleDuration).TotalMinutes, 2)
        $utilization = [Math]::Round($worker.utilizationPercentage, 2)
        $totalWait = [Math]::Round([double]$wait.Sum, 2)
        $maxWait = [Math]::Round([double]$wait.Maximum, 2)
        $lines.Add("| $($worker.workerIndex) | $($worker.moduleCount) | $busy | $idle | $utilization% | $totalWait | $maxWait |")
    }
} else {
    $lines.Add('Standalone execution: no distributed queue or worker utilization measurements.')
}
$lines.Add('')
$lines.Add('Compare complete GitHub job durations as well as this report when evaluating distributed versus standalone runs. The uploaded JSON contains per-module queue, execution, transfer, and artifact timings.')
$lines | Add-Content -LiteralPath $SummaryPath -Encoding utf8
