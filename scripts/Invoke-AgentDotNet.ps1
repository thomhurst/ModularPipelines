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

function Stop-ProcessTree(
    [System.Diagnostics.Process]$RootProcess,
    [System.Collections.Generic.HashSet[int]]$TrackedProcessIds) {
    try {
        if (-not $RootProcess.HasExited) {
            $RootProcess.Kill($true)
        }
    }
    catch [System.InvalidOperationException] {
        # Root exited between HasExited and Kill.
    }

    foreach ($processId in $TrackedProcessIds) {
        if ($processId -eq $PID) {
            continue
        }

        Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
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
$trackedProcessIds = [System.Collections.Generic.HashSet[int]]::new()
$deadline = [DateTimeOffset]::UtcNow.AddSeconds($TimeoutSeconds)
$memoryLimitBytes = [long]$MemoryLimitMb * 1MB
$guardExitCode = $null
$finalExitCode = 1

try {
    if (-not $process.Start()) {
        throw "Failed to start '$DotNetPath'."
    }

    $processStarted = $true

    try {
        $process.PriorityClass = [System.Diagnostics.ProcessPriorityClass]::BelowNormal
    }
    catch {
        # Priority adjustment is best-effort on platforms that do not permit it.
    }

    while (-not $process.WaitForExit($PollIntervalMilliseconds)) {
        $treeState = Get-ProcessTreeState $process.Id
        foreach ($processId in $treeState.ProcessIds) {
            $null = $trackedProcessIds.Add($processId)
        }

        if ($treeState.WorkingSetBytes -gt $memoryLimitBytes) {
            $workingSetMb = [math]::Round($treeState.WorkingSetBytes / 1MB)
            [Console]::Error.WriteLine(
                "Agent dotnet command exceeded ${MemoryLimitMb} MB process-tree limit (${workingSetMb} MB).")
            $guardExitCode = 137
            break
        }

        if ([DateTimeOffset]::UtcNow -ge $deadline) {
            [Console]::Error.WriteLine(
                "Agent dotnet command exceeded ${TimeoutSeconds}s timeout.")
            $guardExitCode = 124
            break
        }
    }

    if ($null -ne $guardExitCode) {
        Stop-ProcessTree $process $trackedProcessIds
        $finalExitCode = $guardExitCode
    }
    else {
        $process.WaitForExit()
        $finalExitCode = $process.ExitCode
    }
}
finally {
    if ($processStarted -and -not $process.HasExited) {
        Stop-ProcessTree $process $trackedProcessIds
    }

    $process.Dispose()
}

exit $finalExitCode
