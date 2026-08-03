using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal static class WindowsJobLauncher
{
    private const string InvocationArgument = "--internal-windows-job-launcher";
    private const uint CreateNoWindow = 0x08000000;
    private const uint CreateSuspended = 0x00000004;
    private const uint Infinite = 0xFFFFFFFF;
    private const uint StartfUseStdHandles = 0x00000100;

    public static bool IsInvocation(string[] arguments) =>
        arguments.Length > 0
        && arguments[0].Equals(InvocationArgument, StringComparison.Ordinal);

    public static ProcessLaunch Wrap(ProcessStartInfo targetStartInfo)
    {
        if (!OperatingSystem.IsWindows())
        {
            return new ProcessLaunch(
                targetStartInfo,
                UsesUnixProcessGroup: false,
                UsesWindowsJobLauncher: false);
        }

        var launcherStartInfo = CreateLauncherStartInfo();
        launcherStartInfo.ArgumentList.Add(InvocationArgument);
        launcherStartInfo.ArgumentList.Add(targetStartInfo.FileName);
        launcherStartInfo.ArgumentList.Add(targetStartInfo.Arguments);
        launcherStartInfo.ArgumentList.Add(targetStartInfo.WorkingDirectory);
        launcherStartInfo.WorkingDirectory = targetStartInfo.WorkingDirectory;
        launcherStartInfo.RedirectStandardOutput = true;
        launcherStartInfo.RedirectStandardError = true;
        launcherStartInfo.RedirectStandardInput = true;
        launcherStartInfo.CreateNoWindow = true;

        launcherStartInfo.Environment.Clear();
        foreach (var (name, value) in targetStartInfo.Environment)
        {
            launcherStartInfo.Environment[name] = value;
        }

        return new ProcessLaunch(
            launcherStartInfo,
            UsesUnixProcessGroup: false,
            UsesWindowsJobLauncher: true);
    }

    public static Task<int> RunAsync(string[] arguments)
    {
        if (arguments.Length != 4 || !OperatingSystem.IsWindows())
        {
            return Task.FromResult(1);
        }

        return Task.FromResult(Run(arguments[1], arguments[2], arguments[3]));
    }

    private static int Run(string executablePath, string arguments, string workingDirectory)
    {
        using var job = WindowsJob.TryCreate(killOnClose: true);
        if (job is null)
        {
            return ReportNativeFailure("create the process job");
        }

        using var target = TryStartTarget(executablePath, arguments, workingDirectory);
        if (target is null || !TryContainAndResume(job, target))
        {
            return 1;
        }

        return WaitForCompletion(job, target.ProcessHandle);
    }

    private static SuspendedProcess? TryStartTarget(
        string executablePath,
        string arguments,
        string workingDirectory)
    {
        var startupInfo = new StartupInfo
        {
            Size = (uint) Marshal.SizeOf<StartupInfo>(),
            Flags = StartfUseStdHandles,
            StandardInput = GetStdHandle(-10),
            StandardOutput = GetStdHandle(-11),
            StandardError = GetStdHandle(-12),
        };
        var commandLine = new StringBuilder($"\"{executablePath}\"");
        if (!string.IsNullOrWhiteSpace(arguments))
        {
            commandLine.Append(' ').Append(arguments);
        }

        if (!CreateProcess(
                executablePath,
                commandLine,
                nint.Zero,
                nint.Zero,
                inheritHandles: true,
                CreateSuspended | CreateNoWindow,
                nint.Zero,
                string.IsNullOrWhiteSpace(workingDirectory) ? null : workingDirectory,
                ref startupInfo,
                out var processInformation))
        {
            ReportNativeFailure("start the target process");
            return null;
        }

        return new SuspendedProcess(processInformation);
    }

    private static bool TryContainAndResume(SafeFileHandle job, SuspendedProcess target)
    {
        if (!WindowsJob.TryAssign(job, target.ProcessHandle))
        {
            var nativeError = Marshal.GetLastPInvokeError();
            _ = TerminateProcess(target.ProcessHandle, 1);
            ReportNativeFailure("assign the target process to its job", nativeError);
            return false;
        }

        if (ResumeThread(target.ThreadHandle) == uint.MaxValue)
        {
            var nativeError = Marshal.GetLastPInvokeError();
            _ = TerminateProcess(target.ProcessHandle, 1);
            ReportNativeFailure("resume the target process", nativeError);
            return false;
        }

        return true;
    }

    private static int WaitForCompletion(SafeFileHandle job, SafeProcessHandle processHandle)
    {
        if (WaitForSingleObject(processHandle, Infinite) == uint.MaxValue)
        {
            return ReportNativeFailure("wait for the target process");
        }

        if (!GetExitCodeProcess(processHandle, out var exitCode))
        {
            return ReportNativeFailure("read the target process exit code");
        }

        while (true)
        {
            if (!WindowsJob.TryGetActiveProcessCount(job, out var activeProcessCount))
            {
                return ReportNativeFailure("inspect the target process job");
            }

            if (activeProcessCount == 0)
            {
                return unchecked((int) exitCode);
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(25));
        }
    }

    private sealed class SuspendedProcess : IDisposable
    {
        public SuspendedProcess(ProcessInformation processInformation)
        {
            ProcessHandle = new SafeProcessHandle(processInformation.Process, ownsHandle: true);
            ThreadHandle = new SafeWaitHandle(processInformation.Thread, ownsHandle: true);
        }

        public SafeProcessHandle ProcessHandle { get; }

        public SafeWaitHandle ThreadHandle { get; }

        public void Dispose()
        {
            ThreadHandle.Dispose();
            ProcessHandle.Dispose();
        }
    }

    private static int ReportNativeFailure(string operation, int? nativeError = null)
    {
        Console.Error.WriteLine(
            $"Unable to {operation}: {new Win32Exception(nativeError ?? Marshal.GetLastPInvokeError()).Message}.");
        return 1;
    }

    private static ProcessStartInfo CreateLauncherStartInfo()
    {
        var assembly = typeof(WindowsJobLauncher).Assembly;
        var processPath = Environment.ProcessPath;
        if (Assembly.GetEntryAssembly() == assembly
            && processPath is not null
            && !Path.GetFileNameWithoutExtension(processPath)
                .Equals("dotnet", StringComparison.OrdinalIgnoreCase))
        {
            return new ProcessStartInfo(processPath) { UseShellExecute = false };
        }

        var dotNetHostPath = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        var startInfo = new ProcessStartInfo(dotNetHostPath) { UseShellExecute = false };
        startInfo.ArgumentList.Add(assembly.Location);
        return startInfo;
    }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateProcess(
        string applicationName,
        StringBuilder commandLine,
        nint processAttributes,
        nint threadAttributes,
        [MarshalAs(UnmanagedType.Bool)] bool inheritHandles,
        uint creationFlags,
        nint environment,
        string? currentDirectory,
        ref StartupInfo startupInfo,
        out ProcessInformation processInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint ResumeThread(SafeWaitHandle thread);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint WaitForSingleObject(SafeProcessHandle handle, uint milliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetExitCodeProcess(SafeProcessHandle process, out uint exitCode);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool TerminateProcess(SafeProcessHandle process, uint exitCode);

    [DllImport("kernel32.dll")]
    private static extern nint GetStdHandle(int standardHandle);
#pragma warning restore SYSLIB1054

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct StartupInfo
    {
        public uint Size;
        public string? Reserved;
        public string? Desktop;
        public string? Title;
        public uint X;
        public uint Y;
        public uint XSize;
        public uint YSize;
        public uint XCountChars;
        public uint YCountChars;
        public uint FillAttribute;
        public uint Flags;
        public ushort ShowWindow;
        public ushort Reserved2Count;
        public nint Reserved2;
        public nint StandardInput;
        public nint StandardOutput;
        public nint StandardError;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessInformation
    {
        public nint Process;
        public nint Thread;
        public uint ProcessId;
        public uint ThreadId;
    }
}
