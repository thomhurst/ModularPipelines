$ErrorActionPreference = 'Stop'
$reportPath = [IO.Path]::GetTempFileName()
$summaryPath = [IO.Path]::GetTempFileName()
try {
    $report = @{
        totalDuration = '00:02:00'
        distributed = @{
            workers = @(
                @{ workerIndex = 0; moduleCount = 2; busyDuration = '00:01:00'; idleDuration = '00:01:00'; utilizationPercentage = 50 },
                @{ workerIndex = 1; moduleCount = 0; busyDuration = '00:00:00'; idleDuration = '00:02:00'; utilizationPercentage = 0 }
            )
            modules = @(
                @{ workerIndex = 0; queueWaitDuration = '00:00:01' },
                @{ workerIndex = 0; queueWaitDuration = '00:00:02' }
            )
        }
    }
    $report | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $reportPath
    & "$PSScriptRoot/Write-DistributedBuildSummary.ps1" -ReportPath $reportPath -SummaryPath $summaryPath
    $summary = Get-Content -LiteralPath $summaryPath -Raw
    foreach ($expected in @('2 minutes', '| 0 | 2 | 1 | 1 | 50% | 3 | 2 |', '| 1 | 0 | 0 | 2 | 0% | 0 | 0 |')) {
        if (-not $summary.Contains($expected)) { throw "Summary omitted '$expected'." }
    }

    $report.Remove('distributed')
    $report | ConvertTo-Json | Set-Content -LiteralPath $reportPath
    Clear-Content -LiteralPath $summaryPath
    & "$PSScriptRoot/Write-DistributedBuildSummary.ps1" -ReportPath $reportPath -SummaryPath $summaryPath
    if ((Get-Content -LiteralPath $summaryPath -Raw) -notmatch 'Standalone execution') {
        throw 'Standalone report was not identified.'
    }
} finally {
    foreach ($temporaryFile in @($reportPath, $summaryPath)) {
        $resolvedFile = [IO.Path]::GetFullPath($temporaryFile)
        if ([IO.Path]::GetDirectoryName($resolvedFile) -ne [IO.Path]::GetTempPath().TrimEnd([IO.Path]::DirectorySeparatorChar)) {
            throw 'Refusing to remove a file outside the temporary directory.'
        }
        Remove-Item -LiteralPath $resolvedFile -Force
    }
}
Write-Output 'Distributed summary: timing, idle worker, and standalone cases passed.'
