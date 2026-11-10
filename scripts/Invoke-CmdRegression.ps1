#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory)][string]$LogDirectory,
    [string]$GuardScript = (Join-Path $PSScriptRoot 'Invoke-AgentDotNet.ps1'),
    [string]$ProbePath = (Get-Process -Id $PID).Path,
    [string[]]$ProbeArguments = @('-NoProfile', '-NonInteractive', '-Command', 'exit 0'),
    [ValidateRange(1, 10)][int]$ProbeTimeoutSeconds = 10
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'WindowsStartupDiagnostics.ps1')
$null = New-Item -ItemType Directory -Force $LogDirectory
$project = 'test/ModularPipelines.Cmd.UnitTests/ModularPipelines.Cmd.UnitTests.csproj'
$results = Join-Path $LogDirectory 'results'
$phases = @(
    @{ Name = 'restore'; Timeout = 180; SingleNode = $false; Arguments = @('restore', $project, '--disable-parallel') },
    @{ Name = 'build'; Timeout = 300; SingleNode = $true; Arguments = @('build', $project, '-c', 'Release', '--framework', 'net11.0', '--no-restore') },
    @{ Name = 'test'; Timeout = 120; SingleNode = $false; Arguments = @('run', '--project', $project, '-c', 'Release', '--framework', 'net11.0', '--no-build', '--no-restore', '--', '--results-directory', $results, '--report-trx', '--report-trx-filename', 'test-results.trx') }
)

# Keep this shell alive across phases: a fresh shell can fail before collecting evidence.
foreach ($phase in $phases) {
    $log = Join-Path $LogDirectory "$($phase.Name).log"
    & $GuardScript -SingleNode:$phase.SingleNode -TimeoutSeconds $phase.Timeout -LogPath $log -DotNetArguments $phase.Arguments
    $phaseExitCode = $LASTEXITCODE
    Add-Content -LiteralPath $log -Value "Cmd $($phase.Name) exited with code $phaseExitCode at $([DateTimeOffset]::UtcNow.ToString('O'))"
    Get-Content -LiteralPath $log -Tail 100

    $probe = Invoke-WindowsStartupProbe -FilePath $ProbePath -Arguments $ProbeArguments -TimeoutSeconds $ProbeTimeoutSeconds
    try {
        & {
            $probe | ConvertTo-Json -Compress | Write-Output
            Write-WindowsStartupSnapshot -Phase $phase.Name -IncludeEvents:($phaseExitCode -ne 0 -or $probe.ExitCode -ne 0)
        } |
            Tee-Object -FilePath (Join-Path $LogDirectory "$($phase.Name)-startup.log")
    }
    catch {
        Write-Output "Windows startup snapshot unavailable: $($_.Exception.GetType().FullName)"
    }

    # Diagnostics must never turn a phase failure (including guard limits) green.
    if ($phaseExitCode -ne 0) { exit $phaseExitCode }
    if ($probe.ExitCode -ne 0) { exit $probe.ExitCode }
}
