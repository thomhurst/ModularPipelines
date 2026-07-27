<#
.SYNOPSIS
Runs a local dotnet command with workstation-safe limits for autonomous agents.

.DESCRIPTION
Disables reusable MSBuild/Roslyn servers, optionally limits MSBuild to one node,
uses below-normal priority, and kills the full process tree when time or memory
limits are exceeded. Exit 124 means timeout; exit 137 means memory limit.

.EXAMPLE
pwsh scripts/Invoke-AgentDotNet.ps1 -SingleNode `
    -DotNetArguments @('build', 'ModularPipelines.sln', '-c', 'Release')
#>

[CmdletBinding(PositionalBinding = $false)]
param(
    [ValidateRange(1, 86400)]
    [int]$TimeoutSeconds = 600,

    [ValidateRange(64, 131072)]
    [int]$MemoryLimitMb = 2048,

    [ValidateRange(50, 10000)]
    [int]$PollIntervalMilliseconds = 500,

    [switch]$SingleNode,

    [ValidateNotNullOrEmpty()]
    [string]$DotNetPath = 'dotnet',

    [Parameter(Mandatory, ValueFromRemainingArguments)]
    [string[]]$DotNetArguments
)

$ErrorActionPreference = 'Stop'

function Get-ProcessSnapshot {
    if ($IsWindows) {
        return Get-CimInstance Win32_Process -Property ProcessId, ParentProcessId, WorkingSetSize |
            ForEach-Object {
                [pscustomobject]@{
                    ProcessId = [int]$_.ProcessId
                    ParentProcessId = [int]$_.ParentProcessId
                    WorkingSetBytes = [long]$_.WorkingSetSize
                }
            }
    }

    return & ps -eo pid=,ppid=,rss= |
        ForEach-Object {
            $columns = $_.Trim() -split '\s+'
            if ($columns.Count -eq 3) {
                [pscustomobject]@{
                    ProcessId = [int]$columns[0]
                    ParentProcessId = [int]$columns[1]
                    WorkingSetBytes = [long]$columns[2] * 1KB
                }
            }
        }
}

function Get-ProcessTreeState([int]$RootProcessId) {
    $snapshot = @(Get-ProcessSnapshot)
    $processIds = [System.Collections.Generic.HashSet[int]]::new()
    $null = $processIds.Add($RootProcessId)

    do {
        $added = $false
        foreach ($item in $snapshot) {
            if ($processIds.Contains($item.ParentProcessId) -and $processIds.Add($item.ProcessId)) {
                $added = $true
            }
        }
    }
    while ($added)

    $workingSetBytes = ($snapshot |
        Where-Object { $processIds.Contains($_.ProcessId) } |
        Measure-Object -Property WorkingSetBytes -Sum).Sum

    return [pscustomobject]@{
        ProcessIds = $processIds
        WorkingSetBytes = [long]($workingSetBytes ?? 0)
    }
}

function Get-ProcessStartTimeUtcTicks([int]$ProcessId) {
    try {
        $candidate = [System.Diagnostics.Process]::GetProcessById($ProcessId)
        try {
            return $candidate.StartTime.ToUniversalTime().Ticks
        }
        finally {
            $candidate.Dispose()
        }
    }
    catch [System.ArgumentException] {
        Write-Verbose "Process $ProcessId exited before its identity could be read."
        return $null
    }
    catch [System.InvalidOperationException] {
        Write-Verbose "Process $ProcessId became unavailable before its identity could be read."
        return $null
    }
    catch [System.ComponentModel.Win32Exception] {
        Write-Verbose "Process $ProcessId identity could not be read: $_"
        return $null
    }
}

function Sync-TrackedProcesses(
    [System.Collections.Generic.Dictionary[int, long]]$TrackedProcesses,
    [System.Collections.Generic.HashSet[int]]$CurrentProcessIds) {
    foreach ($processId in @($TrackedProcesses.Keys)) {
        $currentStartTimeUtcTicks = Get-ProcessStartTimeUtcTicks $processId
        if (($null -eq $currentStartTimeUtcTicks) -or
            ($currentStartTimeUtcTicks -ne $TrackedProcesses[$processId])) {
            $null = $TrackedProcesses.Remove($processId)
        }
    }

    foreach ($processId in $CurrentProcessIds) {
        if ($TrackedProcesses.ContainsKey($processId)) {
            continue
        }

        $startTimeUtcTicks = Get-ProcessStartTimeUtcTicks $processId
        if ($null -ne $startTimeUtcTicks) {
            $TrackedProcesses[$processId] = $startTimeUtcTicks
        }
    }
}

function Stop-ProcessTree(
    [System.Diagnostics.Process]$RootProcess,
    [System.Collections.Generic.Dictionary[int, long]]$TrackedProcesses) {
    try {
        if (-not $RootProcess.HasExited) {
            $RootProcess.Kill($true)
        }
    }
    catch [System.InvalidOperationException] {
        Write-Verbose 'Root process exited before process-tree cleanup completed.'
    }
    catch [System.ComponentModel.Win32Exception] {
        Write-Verbose "Root process-tree cleanup was unavailable: $_"
    }

    foreach ($trackedProcess in $TrackedProcesses.GetEnumerator()) {
        $processId = $trackedProcess.Key
        if ($processId -eq $PID) {
            continue
        }

        try {
            $candidate = [System.Diagnostics.Process]::GetProcessById($processId)
            try {
                if ($candidate.StartTime.ToUniversalTime().Ticks -eq $trackedProcess.Value) {
                    $candidate.Kill($true)
                }
            }
            finally {
                $candidate.Dispose()
            }
        }
        catch [System.ArgumentException] {
            Write-Verbose "Tracked process $processId already exited."
        }
        catch [System.InvalidOperationException] {
            Write-Verbose "Tracked process $processId became unavailable during cleanup."
        }
        catch [System.ComponentModel.Win32Exception] {
            Write-Verbose "Tracked process $processId could not be stopped: $_"
        }
    }
}

function Add-SingleNodeArgument([string[]]$Arguments) {
    if (-not $SingleNode -or $Arguments.Count -eq 0) {
        return $Arguments
    }

    $verb = $Arguments[0]
    $supportsMaxCpuCount = $verb -in @('build', 'test', 'pack', 'publish', 'msbuild')
    $alreadyConfigured = $Arguments |
        Where-Object { $_ -match '^(?:-m|--maxcpucount)(?::|$)' } |
        Select-Object -First 1
    if (-not $supportsMaxCpuCount -or $alreadyConfigured) {
        return $Arguments
    }

    $separatorIndex = [Array]::IndexOf($Arguments, '--')
    if ($separatorIndex -lt 0) {
        return @($Arguments) + '-m:1'
    }

    return @($Arguments[0..($separatorIndex - 1)]) +
        '-m:1' +
        @($Arguments[$separatorIndex..($Arguments.Count - 1)])
}

$effectiveArguments = Add-SingleNodeArgument $DotNetArguments
$startInfo = [System.Diagnostics.ProcessStartInfo]::new()
$startInfo.FileName = $DotNetPath
$startInfo.UseShellExecute = $false
$startInfo.Environment['BuildInParallel'] = 'false'
$startInfo.Environment['DOTNET_CLI_TELEMETRY_OPTOUT'] = '1'
$startInfo.Environment['DOTNET_CLI_USE_MSBUILD_SERVER'] = '0'
$startInfo.Environment['MSBUILDDISABLENODEREUSE'] = '1'
$startInfo.Environment['UseSharedCompilation'] = 'false'

foreach ($argument in $effectiveArguments) {
    $startInfo.ArgumentList.Add($argument)
}

$process = [System.Diagnostics.Process]::new()
$process.StartInfo = $startInfo
$processStarted = $false
$trackedProcesses = [System.Collections.Generic.Dictionary[int, long]]::new()
$deadline = [DateTimeOffset]::UtcNow.AddSeconds($TimeoutSeconds)
$memoryLimitBytes = [long]$MemoryLimitMb * 1MB
$guardExitCode = $null
$finalExitCode = 1

try {
    if (-not $process.Start()) {
        throw "Failed to start '$DotNetPath'."
    }

    $processStarted = $true
    $trackedProcesses[$process.Id] = $process.StartTime.ToUniversalTime().Ticks

    try {
        $process.PriorityClass = [System.Diagnostics.ProcessPriorityClass]::BelowNormal
    }
    catch {
        Write-Verbose "Could not lower process priority: $_"
    }

    while ($true) {
        $treeState = Get-ProcessTreeState $process.Id
        Sync-TrackedProcesses $trackedProcesses $treeState.ProcessIds

        if ($treeState.WorkingSetBytes -gt $memoryLimitBytes) {
            $workingSetMb = [math]::Round($treeState.WorkingSetBytes / 1MB)
            [Console]::Error.WriteLine(
                "Agent dotnet command exceeded ${MemoryLimitMb} MB process-tree limit (${workingSetMb} MB).")
            $guardExitCode = 137
            break
        }

        $remaining = $deadline - [DateTimeOffset]::UtcNow
        if ($remaining -le [TimeSpan]::Zero) {
            [Console]::Error.WriteLine(
                "Agent dotnet command exceeded ${TimeoutSeconds}s timeout.")
            $guardExitCode = 124
            break
        }

        $waitMilliseconds = [math]::Min(
            $PollIntervalMilliseconds,
            [math]::Max(1, [math]::Ceiling($remaining.TotalMilliseconds)))
        if ($process.WaitForExit([int]$waitMilliseconds)) {
            $treeState = Get-ProcessTreeState $process.Id
            Sync-TrackedProcesses $trackedProcesses $treeState.ProcessIds

            if ([DateTimeOffset]::UtcNow -gt $deadline) {
                [Console]::Error.WriteLine(
                    "Agent dotnet command exceeded ${TimeoutSeconds}s timeout.")
                $guardExitCode = 124
            }

            break
        }
    }

    if ($null -ne $guardExitCode) {
        $finalExitCode = $guardExitCode
    }
    else {
        $process.WaitForExit()
        $finalExitCode = $process.ExitCode
    }
}
finally {
    if ($processStarted) {
        Stop-ProcessTree $process $trackedProcesses
    }

    $process.Dispose()
}

exit $finalExitCode
