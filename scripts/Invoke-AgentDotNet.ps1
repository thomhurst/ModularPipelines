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

if ($IsWindows) {
    Add-Type -TypeDefinition @'
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

public static class AgentDotNetWindowsJob
{
    private const int JobObjectExtendedLimitInformation = 9;
    private const uint JobObjectLimitKillOnJobClose = 0x00002000;

    [StructLayout(LayoutKind.Sequential)]
    private struct BasicLimitInformation
    {
        public long PerProcessUserTimeLimit;
        public long PerJobUserTimeLimit;
        public uint LimitFlags;
        public UIntPtr MinimumWorkingSetSize;
        public UIntPtr MaximumWorkingSetSize;
        public uint ActiveProcessLimit;
        public UIntPtr Affinity;
        public uint PriorityClass;
        public uint SchedulingClass;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IoCounters
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ExtendedLimitInformation
    {
        public BasicLimitInformation BasicLimitInformation;
        public IoCounters IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr CreateJobObject(IntPtr securityAttributes, string name);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetInformationJobObject(
        IntPtr job,
        int informationClass,
        IntPtr information,
        uint informationLength);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr handle);

    public static IntPtr CreateKillOnClose()
    {
        IntPtr job = CreateJobObject(IntPtr.Zero, null);
        if (job == IntPtr.Zero)
        {
            throw new Win32Exception();
        }

        int size = Marshal.SizeOf<ExtendedLimitInformation>();
        IntPtr information = Marshal.AllocHGlobal(size);
        try
        {
            var limits = new ExtendedLimitInformation();
            limits.BasicLimitInformation.LimitFlags = JobObjectLimitKillOnJobClose;
            Marshal.StructureToPtr(limits, information, false);
            if (!SetInformationJobObject(
                    job,
                    JobObjectExtendedLimitInformation,
                    information,
                    (uint)size))
            {
                throw new Win32Exception();
            }

            return job;
        }
        catch
        {
            CloseHandle(job);
            throw;
        }
        finally
        {
            Marshal.FreeHGlobal(information);
        }
    }

    public static void Assign(IntPtr job, IntPtr process)
    {
        if (!AssignProcessToJobObject(job, process))
        {
            throw new Win32Exception();
        }
    }

    public static void Close(IntPtr job)
    {
        if (job != IntPtr.Zero)
        {
            CloseHandle(job);
        }
    }
}
'@
}
else {
    Add-Type -TypeDefinition @'
using System.Runtime.InteropServices;

public static class AgentDotNetUnixNative
{
    [DllImport("libc", SetLastError = true)]
    public static extern int kill(int processId, int signal);
}
'@
}

function Get-ProcessSnapshot {
    if ($IsWindows) {
        return Get-CimInstance Win32_Process `
            -Property ProcessId, ParentProcessId, WorkingSetSize, CreationDate |
            ForEach-Object {
                [pscustomobject]@{
                    ProcessId = [int]$_.ProcessId
                    ParentProcessId = [int]$_.ParentProcessId
                    WorkingSetBytes = [long]$_.WorkingSetSize
                    StartIdentity = $_.CreationDate.ToUniversalTime().Ticks.ToString(
                        [Globalization.CultureInfo]::InvariantCulture)
                }
            }
    }

    return & ps -eo pid=,ppid=,rss=,lstart= |
        ForEach-Object {
            if ($_ -match '^\s*(\d+)\s+(\d+)\s+(\d+)\s+(.+?)\s*$') {
                [pscustomobject]@{
                    ProcessId = [int]$Matches[1]
                    ParentProcessId = [int]$Matches[2]
                    WorkingSetBytes = [long]$Matches[3] * 1KB
                    StartIdentity = $Matches[4]
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

    $treeProcesses = @($snapshot | Where-Object { $processIds.Contains($_.ProcessId) })
    return [pscustomobject]@{
        Snapshot = $snapshot
        Processes = $treeProcesses
    }
}

function Sync-TrackedProcesses(
    [System.Collections.Generic.Dictionary[int, string]]$TrackedProcesses,
    [object[]]$Snapshot,
    [object[]]$TreeProcesses) {
    $currentProcesses = @{}
    foreach ($process in $Snapshot) {
        $currentProcesses[$process.ProcessId] = $process
    }

    foreach ($processId in @($TrackedProcesses.Keys)) {
        $currentProcess = $currentProcesses[$processId]
        if (($null -eq $currentProcess) -or
            ($currentProcess.StartIdentity -ne $TrackedProcesses[$processId])) {
            $null = $TrackedProcesses.Remove($processId)
        }
    }

    foreach ($process in $TreeProcesses) {
        $TrackedProcesses[$process.ProcessId] = $process.StartIdentity
    }
}

function Get-TrackedWorkingSetBytes(
    [System.Collections.Generic.Dictionary[int, string]]$TrackedProcesses,
    [object[]]$Snapshot) {
    $workingSetBytes = ($Snapshot |
        Where-Object {
            $TrackedProcesses.ContainsKey($_.ProcessId) -and
            $TrackedProcesses[$_.ProcessId] -eq $_.StartIdentity
        } |
        Measure-Object -Property WorkingSetBytes -Sum).Sum

    return [long]($workingSetBytes ?? 0)
}

function Stop-ProcessTree(
    [System.Diagnostics.Process]$RootProcess,
    [System.Collections.Generic.Dictionary[int, string]]$TrackedProcesses,
    [IntPtr]$WindowsJobHandle,
    [int]$UnixProcessGroupId) {
    if ($IsWindows -and $WindowsJobHandle -ne [IntPtr]::Zero) {
        # Closing a kill-on-close Job Object catches descendants that exited the
        # parent-PID tree before the final snapshot.
        [AgentDotNetWindowsJob]::Close($WindowsJobHandle)
    }
    elseif ((-not $IsWindows) -and $UnixProcessGroupId -gt 0) {
        # Signal the process group from the guard so timeout and memory-limit paths
        # have the same containment as normal exit.
        $killResult = [AgentDotNetUnixNative]::kill(-$UnixProcessGroupId, 9)
        if ($killResult -ne 0) {
            $errorCode = [Runtime.InteropServices.Marshal]::GetLastWin32Error()
            if ($errorCode -ne 3) {
                Write-Verbose "Unix process group $UnixProcessGroupId could not be stopped (errno $errorCode)."
            }
        }
    }

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

    try {
        $currentSnapshot = @(Get-ProcessSnapshot)
    }
    catch {
        Write-Verbose "Final process snapshot was unavailable: $_"
        return
    }

    $currentProcesses = @{}
    foreach ($process in $currentSnapshot) {
        $currentProcesses[$process.ProcessId] = $process
    }

    # The Job Object/process group is the containment boundary. Identity-checked
    # tracked cleanup is defense in depth for processes seen before reparenting.
    foreach ($trackedProcess in $TrackedProcesses.GetEnumerator()) {
        $processId = $trackedProcess.Key
        if ($processId -eq $PID) {
            continue
        }

        $currentProcess = $currentProcesses[$processId]
        if (($null -eq $currentProcess) -or
            ($currentProcess.StartIdentity -ne $trackedProcess.Value)) {
            continue
        }

        try {
            $candidate = [System.Diagnostics.Process]::GetProcessById($processId)
            try {
                $candidate.Kill($true)
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
$startInfo.UseShellExecute = $false
$startInfo.Environment['DOTNET_CLI_TELEMETRY_OPTOUT'] = '1'
$startInfo.Environment['DOTNET_CLI_USE_MSBUILD_SERVER'] = '0'
$startInfo.Environment['MSBUILDDISABLENODEREUSE'] = '1'
$startInfo.Environment['UseSharedCompilation'] = 'false'
$pwshPath = (Get-Process -Id $PID).Path

if ($IsWindows) {
    # The wrapper waits on a gate, allowing the guard to assign it to the Job Object
    # and lower its priority before it can launch dotnet or any descendants.
    $windowsWrapper = @'
$startGate = [Threading.EventWaitHandle]::OpenExisting($args[0])
try {
    if (-not $startGate.WaitOne([TimeSpan]::FromSeconds(30))) {
        [Console]::Error.WriteLine('Agent dotnet guard launch gate timed out.')
        exit 126
    }
}
finally {
    $startGate.Dispose()
}

$startInfo = [Diagnostics.ProcessStartInfo]::new()
$startInfo.UseShellExecute = $false
$startInfo.FileName = $args[1]
foreach ($argument in $args[2..($args.Count - 1)]) {
    $startInfo.ArgumentList.Add($argument)
}

$child = [Diagnostics.Process]::Start($startInfo)
try {
    $child.WaitForExit()
    $exitCode = $child.ExitCode
}
finally {
    $child.Dispose()
}

exit $exitCode
'@
    $startInfo.FileName = $pwshPath
    $startInfo.ArgumentList.Add('-NoProfile')
    $startInfo.ArgumentList.Add('-NonInteractive')
    $startInfo.ArgumentList.Add('-CommandWithArgs')
    $startInfo.ArgumentList.Add($windowsWrapper)
    $startInfo.ArgumentList.Add('{WINDOWS_START_GATE}')
    $startInfo.ArgumentList.Add($DotNetPath)
    foreach ($argument in $effectiveArguments) {
        $startInfo.ArgumentList.Add($argument)
    }
}
else {
    # PowerShell is already a guard prerequisite, so libc calls provide portable
    # process-group containment without requiring setsid(1) or Python on macOS.
    $unixWrapper = @'
Add-Type -TypeDefinition @"
using System.Runtime.InteropServices;

public static class AgentDotNetUnixChildNative
{
    [DllImport("libc", SetLastError = true)]
    public static extern int setsid();

    [DllImport("libc", SetLastError = true)]
    public static extern int setpriority(int which, int who, int priority);
}
"@

if ([AgentDotNetUnixChildNative]::setsid() -lt 0) {
    $errorCode = [Runtime.InteropServices.Marshal]::GetLastWin32Error()
    [Console]::Error.WriteLine("Agent dotnet guard could not create a Unix session (errno $errorCode).")
    exit 126
}

if ([AgentDotNetUnixChildNative]::setpriority(0, 0, 10) -ne 0) {
    $errorCode = [Runtime.InteropServices.Marshal]::GetLastWin32Error()
    [Console]::Error.WriteLine("Agent dotnet guard could not lower Unix priority (errno $errorCode).")
}

$startInfo = [Diagnostics.ProcessStartInfo]::new()
$startInfo.UseShellExecute = $false
$startInfo.FileName = $args[0]
foreach ($argument in $args[1..($args.Count - 1)]) {
    $startInfo.ArgumentList.Add($argument)
}

$child = [Diagnostics.Process]::Start($startInfo)
try {
    $child.WaitForExit()
    $exitCode = $child.ExitCode
}
finally {
    $child.Dispose()
}

exit $exitCode
'@
    $startInfo.FileName = $pwshPath
    $startInfo.ArgumentList.Add('-NoProfile')
    $startInfo.ArgumentList.Add('-NonInteractive')
    $startInfo.ArgumentList.Add('-CommandWithArgs')
    $startInfo.ArgumentList.Add($unixWrapper)
    $startInfo.ArgumentList.Add($DotNetPath)
    foreach ($argument in $effectiveArguments) {
        $startInfo.ArgumentList.Add($argument)
    }
}

$process = [System.Diagnostics.Process]::new()
$process.StartInfo = $startInfo
$processStarted = $false
$trackedProcesses = [System.Collections.Generic.Dictionary[int, string]]::new()
$deadline = [DateTimeOffset]::UtcNow.AddSeconds($TimeoutSeconds)
$memoryLimitBytes = [long]$MemoryLimitMb * 1MB
$guardExitCode = $null
$finalExitCode = 1
$windowsJobHandle = [IntPtr]::Zero
$windowsStartGate = $null
$unixProcessGroupId = 0

try {
    if ($IsWindows) {
        $windowsJobHandle = [AgentDotNetWindowsJob]::CreateKillOnClose()
        $windowsStartGateName = "agent-dotnet-guard-$([guid]::NewGuid())"
        $windowsStartGate = [Threading.EventWaitHandle]::new(
            $false,
            [Threading.EventResetMode]::ManualReset,
            $windowsStartGateName)
        $gateArgumentIndex = $startInfo.ArgumentList.IndexOf('{WINDOWS_START_GATE}')
        $startInfo.ArgumentList[$gateArgumentIndex] = $windowsStartGateName
    }

    if (-not $process.Start()) {
        throw "Failed to start '$DotNetPath'."
    }

    $processStarted = $true
    if ($IsWindows) {
        [AgentDotNetWindowsJob]::Assign($windowsJobHandle, $process.Handle)
    }
    else {
        $unixProcessGroupId = $process.Id
    }

    try {
        $process.PriorityClass = [System.Diagnostics.ProcessPriorityClass]::BelowNormal
    }
    catch {
        Write-Verbose "Could not lower process priority: $_"
    }

    if ($IsWindows) {
        $null = $windowsStartGate.Set()
    }

    while ($true) {
        $treeState = Get-ProcessTreeState $process.Id
        Sync-TrackedProcesses $trackedProcesses @($treeState.Snapshot) @($treeState.Processes)
        $workingSetBytes = Get-TrackedWorkingSetBytes $trackedProcesses @($treeState.Snapshot)

        if ($workingSetBytes -gt $memoryLimitBytes) {
            $workingSetMb = [math]::Round($workingSetBytes / 1MB)
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
            Sync-TrackedProcesses $trackedProcesses @($treeState.Snapshot) @($treeState.Processes)

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
        Stop-ProcessTree `
            -RootProcess $process `
            -TrackedProcesses $trackedProcesses `
            -WindowsJobHandle $windowsJobHandle `
            -UnixProcessGroupId $unixProcessGroupId
        $windowsJobHandle = [IntPtr]::Zero
    }
    elseif ($windowsJobHandle -ne [IntPtr]::Zero) {
        [AgentDotNetWindowsJob]::Close($windowsJobHandle)
    }

    if ($null -ne $windowsStartGate) {
        $windowsStartGate.Dispose()
    }

    $process.Dispose()
}

exit $finalExitCode
