using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal static class UnixProcessGroupLauncher
{
    private const string InvocationArgument = "--internal-process-group-launcher";

    public static bool IsInvocation(string[] arguments) =>
        arguments.Length > 0
        && arguments[0].Equals(InvocationArgument, StringComparison.Ordinal);

    public static ProcessLaunch Wrap(ProcessStartInfo targetStartInfo)
    {
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
            UsesUnixProcessGroup: true,
            UsesWindowsJobLauncher: false);
    }

    public static async Task<int> RunAsync(string[] arguments)
    {
        if (arguments.Length != 4 || OperatingSystem.IsWindows())
        {
            return 1;
        }

        if (SetSessionId() < 0)
        {
            Console.Error.WriteLine(
                $"Unable to create process group: native error {Marshal.GetLastPInvokeError()}.");
            return 1;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = arguments[1],
            Arguments = arguments[2],
            WorkingDirectory = arguments[3],
            UseShellExecute = false,
        };

        using var process = Process.Start(startInfo);
        if (process is null)
        {
            return 1;
        }

        await process.WaitForExitAsync();
        return process.ExitCode;
    }

    private static ProcessStartInfo CreateLauncherStartInfo()
    {
        var assembly = typeof(UnixProcessGroupLauncher).Assembly;
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
    [DllImport("libc", EntryPoint = "setsid", SetLastError = true)]
    private static extern int SetSessionId();
#pragma warning restore SYSLIB1054
}

internal sealed record ProcessLaunch(
    ProcessStartInfo StartInfo,
    bool UsesUnixProcessGroup,
    bool UsesWindowsJobLauncher);
