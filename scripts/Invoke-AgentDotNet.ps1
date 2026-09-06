<#
.SYNOPSIS
Runs a local dotnet command with workstation-safe limits for autonomous agents.

.DESCRIPTION
Disables reusable MSBuild/Roslyn servers, optionally limits MSBuild to one node,
uses below-normal priority, and kills the full process tree when time or memory
limits are exceeded. Exit 124 means timeout; exit 137 means memory limit.

.EXAMPLE
& scripts/Invoke-AgentDotNet.ps1 -SingleNode `
    -DotNetArguments @('build', 'ModularPipelines.slnx', '-c', 'Release')

Invoke in-process so -DotNetArguments receives the array intact; see CLAUDE.md.
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

public static class AgentDotNetUnixNative
{
    [DllImport("libc", SetLastError = true)]
    public static extern int kill(int processId, int signal);
}

public sealed class AgentDotNetUnixProcessInfo
{
    public int ProcessId { get; set; }
    public int ParentProcessId { get; set; }
    public long WorkingSetBytes { get; set; }
    public string StartIdentity { get; set; }
}

public static class AgentDotNetLinuxProcessSnapshot
{
    public static AgentDotNetUnixProcessInfo[] Capture()
    {
        var processes = new List<AgentDotNetUnixProcessInfo>();
        foreach (string directory in Directory.EnumerateDirectories("/proc"))
        {
            string name = Path.GetFileName(directory);
            if (!int.TryParse(name, NumberStyles.None, CultureInfo.InvariantCulture, out int processId))
            {
                continue;
            }

            AgentDotNetUnixProcessInfo process = CaptureProcess(processId);
            if (process != null)
            {
                processes.Add(process);
            }
        }

        return processes.ToArray();
    }

    public static AgentDotNetUnixProcessInfo CaptureProcess(int processId)
    {
        try
        {
            string stat = File.ReadAllText(
                Path.Combine("/proc", processId.ToString(CultureInfo.InvariantCulture), "stat"));
            int commandEnd = stat.LastIndexOf(") ", StringComparison.Ordinal);
            if (commandEnd < 0)
            {
                return null;
            }

            string[] fields = stat.Substring(commandEnd + 2)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length < 22)
            {
                return null;
            }

            return new AgentDotNetUnixProcessInfo
            {
                ProcessId = processId,
                ParentProcessId = int.Parse(fields[1], CultureInfo.InvariantCulture),
                StartIdentity = fields[19],
                WorkingSetBytes =
                    long.Parse(fields[21], CultureInfo.InvariantCulture) * Environment.SystemPageSize,
            };
        }
        catch (IOException)
        {
            // The process exited while its snapshot was being read.
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            // The process cannot be inspected by this user.
            return null;
        }
    }
}

public static class AgentDotNetMacProcessSnapshot
{
    private const int ProcPidTBsdInfo = 3;
    private const int ProcPidTaskInfo = 4;

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcBsdInfo
    {
        public uint Flags;
        public uint Status;
        public uint ExitStatus;
        public uint ProcessId;
        public uint ParentProcessId;
        public uint UserId;
        public uint GroupId;
        public uint RealUserId;
        public uint RealGroupId;
        public uint SavedUserId;
        public uint SavedGroupId;
        public uint Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Command;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] Name;
        public uint OpenFileCount;
        public uint ProcessGroupId;
        public uint JobControlCount;
        public uint ControllingTerminalDevice;
        public uint TerminalProcessGroupId;
        public int Nice;
        public ulong StartTimeSeconds;
        public ulong StartTimeMicroseconds;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcTaskInfo
    {
        public ulong VirtualSize;
        public ulong ResidentSize;
        public ulong TotalUserTime;
        public ulong TotalSystemTime;
        public ulong ThreadsUserTime;
        public ulong ThreadsSystemTime;
        public int Policy;
        public int Faults;
        public int PageIns;
        public int CopyOnWriteFaults;
        public int MessagesSent;
        public int MessagesReceived;
        public int MachSystemCalls;
        public int UnixSystemCalls;
        public int ContextSwitches;
        public int ThreadCount;
        public int RunningThreadCount;
        public int Priority;
    }

    [DllImport("/usr/lib/libproc.dylib")]
    private static extern int proc_listallpids([Out] int[] buffer, int bufferSize);

    [DllImport("/usr/lib/libproc.dylib")]
    private static extern int proc_pidinfo(
        int processId,
        int flavor,
        ulong argument,
        IntPtr buffer,
        int bufferSize);

    public static AgentDotNetUnixProcessInfo[] Capture()
    {
        var processIds = new int[131072];
        int processCount = proc_listallpids(processIds, processIds.Length * sizeof(int));
        var processes = new List<AgentDotNetUnixProcessInfo>(Math.Max(processCount, 0));

        for (int index = 0; index < processCount && index < processIds.Length; index++)
        {
            AgentDotNetUnixProcessInfo process = CaptureProcess(processIds[index]);
            if (process != null)
            {
                processes.Add(process);
            }
        }

        return processes.ToArray();
    }

    public static AgentDotNetUnixProcessInfo CaptureProcess(int processId)
    {
        if (processId <= 0 ||
            !TryRead(processId, ProcPidTBsdInfo, out ProcBsdInfo before))
        {
            return null;
        }

        TryRead(processId, ProcPidTaskInfo, out ProcTaskInfo task);
        if (!TryRead(processId, ProcPidTBsdInfo, out ProcBsdInfo after) ||
            before.ProcessId != after.ProcessId ||
            before.ParentProcessId != after.ParentProcessId ||
            before.StartTimeSeconds != after.StartTimeSeconds ||
            before.StartTimeMicroseconds != after.StartTimeMicroseconds)
        {
            return null;
        }

        return new AgentDotNetUnixProcessInfo
        {
            ProcessId = processId,
            ParentProcessId = (int)after.ParentProcessId,
            WorkingSetBytes = checked((long)task.ResidentSize),
            StartIdentity = string.Concat(
                after.StartTimeSeconds.ToString(CultureInfo.InvariantCulture),
                ":",
                after.StartTimeMicroseconds.ToString(CultureInfo.InvariantCulture)),
        };
    }

    private static bool TryRead<T>(int processId, int flavor, out T value)
        where T : struct
    {
        int size = Marshal.SizeOf<T>();
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            if (proc_pidinfo(processId, flavor, 0, buffer, size) != size)
            {
                value = default(T);
                return false;
            }

            value = Marshal.PtrToStructure<T>(buffer);
            return true;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }
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

    if ($IsMacOS) {
        return [AgentDotNetMacProcessSnapshot]::Capture()
    }

    return [AgentDotNetLinuxProcessSnapshot]::Capture()
}

function Get-LiveProcessIdentity([int]$ProcessId) {
    try {
        if ($IsWindows) {
            $process = Get-CimInstance Win32_Process `
                -Filter "ProcessId = $ProcessId" `
                -Property CreationDate
            if ($null -eq $process) {
                return $null
            }

            return $process.CreationDate.ToUniversalTime().Ticks.ToString(
                [Globalization.CultureInfo]::InvariantCulture)
        }

        $process = if ($IsMacOS) {
            [AgentDotNetMacProcessSnapshot]::CaptureProcess($ProcessId)
        }
        else {
            [AgentDotNetLinuxProcessSnapshot]::CaptureProcess($ProcessId)
        }

        return $process?.StartIdentity
    }
    catch {
        Write-Verbose "Live identity for process $ProcessId was unavailable: $_"
        return $null
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

    # The Job Object/process group is the containment boundary. Identity-checked
    # tracked cleanup is defense in depth for processes seen before reparenting.
    foreach ($trackedProcess in $TrackedProcesses.GetEnumerator()) {
        $processId = $trackedProcess.Key
        if ($processId -eq $PID) {
            continue
        }

        try {
            $candidate = [System.Diagnostics.Process]::GetProcessById($processId)
            try {
                # Pin the native process handle before checking identity so Windows
                # cleanup cannot reopen a recycled PID between verification and kill.
                $null = $candidate.SafeHandle

                # Re-read this PID immediately before acting. A bulk snapshot taken
                # before the loop can become stale and target a recycled PID.
                $liveIdentity = Get-LiveProcessIdentity -ProcessId $processId
                if (($null -eq $liveIdentity) -or
                    ($liveIdentity -ne $trackedProcess.Value)) {
                    continue
                }

                # Primary containment already handled descendants. Kill only this
                # identity-verified fallback process through the pinned handle.
                $candidate.Kill()
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
        Where-Object { $_ -match '^[-/]{1,2}(?:m|maxcpucount)(?::|$)' } |
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

# Set-Location only moves the PowerShell provider location; a child process inherits
# the process working directory instead. Pin the wrapper to the caller's location so a
# guard run inside an isolated worktree builds that worktree; dotnet inherits it.
$workingDirectory = (Get-Location).ProviderPath

function ConvertTo-PowerShellLiteral([string]$Value) {
    return "'" + $Value.Replace("'", "''") + "'"
}

# The wrapper receives dotnet's arguments as embedded literals rather than through
# `$args`: PowerShell's parameter binder would otherwise split MSBuild-style
# '-name:value' tokens such as '-m:1' or '-p:Configuration=Release' in two.
$argumentLiterals = ($effectiveArguments | ForEach-Object { ConvertTo-PowerShellLiteral $_ }) -join ', '
$wrapperPrologue = "`$dotnetPath = $(ConvertTo-PowerShellLiteral $DotNetPath)`n" +
    "`$dotnetArguments = @($argumentLiterals)`n"

$startInfo = [System.Diagnostics.ProcessStartInfo]::new()
$startInfo.UseShellExecute = $false
$startInfo.WorkingDirectory = $workingDirectory
$startInfo.Environment['BuildInParallel'] = 'false'
$startInfo.Environment['DOTNET_CLI_TELEMETRY_OPTOUT'] = '1'
$startInfo.Environment['DOTNET_CLI_USE_MSBUILD_SERVER'] = '0'
$startInfo.Environment['MSBUILDDISABLENODEREUSE'] = '1'
$startInfo.Environment['UseSharedCompilation'] = 'false'
$pwshPath = (Get-Process -Id $PID).Path
$wrapperPath = [System.IO.Path]::Combine(
    [System.IO.Path]::GetTempPath(),
    "agent-dotnet-wrapper-$([guid]::NewGuid()).ps1"
)

$childLaunchScript = @'
$startInfo = [Diagnostics.ProcessStartInfo]::new()
$startInfo.UseShellExecute = $false
$startInfo.FileName = $dotnetPath
foreach ($argument in $dotnetArguments) {
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

if ($IsWindows) {
    # The wrapper waits on a gate, allowing the guard to assign it to the Job Object
    # and lower its priority before it can launch dotnet or any descendants.
    $windowsStartGateName = "agent-dotnet-guard-$([guid]::NewGuid())"
    $wrapperScript = $wrapperPrologue +
        "`$startGateName = $(ConvertTo-PowerShellLiteral $windowsStartGateName)`n" + @'
$startGate = [Threading.EventWaitHandle]::OpenExisting($startGateName)
try {
    if (-not $startGate.WaitOne([TimeSpan]::FromSeconds(30))) {
        [Console]::Error.WriteLine('Agent dotnet guard launch gate timed out.')
        exit 126
    }
}
finally {
    $startGate.Dispose()
}

'@ + $childLaunchScript
}
else {
    # PowerShell is already a guard prerequisite, so libc calls provide portable
    # process-group containment without requiring setsid(1) or Python on macOS.
    $wrapperScript = $wrapperPrologue + @'
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

'@ + $childLaunchScript
}

$startInfo.FileName = $pwshPath
$startInfo.ArgumentList.Add('-NoProfile')
$startInfo.ArgumentList.Add('-NonInteractive')
$startInfo.ArgumentList.Add('-File')
$startInfo.ArgumentList.Add($wrapperPath)

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
    # -File works on supported PowerShell versions; -CommandWithArgs was
    # experimental before PowerShell 7.5.
    [System.IO.File]::WriteAllText(
        $wrapperPath,
        $wrapperScript,
        [System.Text.UTF8Encoding]::new($false))

    if ($IsWindows) {
        $windowsJobHandle = [AgentDotNetWindowsJob]::CreateKillOnClose()
        $windowsStartGate = [Threading.EventWaitHandle]::new(
            $false,
            [Threading.EventResetMode]::ManualReset,
            $windowsStartGateName)
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

    if (Test-Path -LiteralPath $wrapperPath) {
        Remove-Item -LiteralPath $wrapperPath -Force -ErrorAction SilentlyContinue
    }
}

exit $finalExitCode
