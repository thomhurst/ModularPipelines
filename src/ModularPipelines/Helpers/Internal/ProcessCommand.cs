using System.ComponentModel;
using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// A process to start: the executable, its arguments, working directory, environment and credentials.
/// </summary>
/// <param name="TargetFilePath">The executable as the caller named it, shown in the command input.</param>
/// <param name="ArgumentList">The individual arguments, or <see langword="null"/> when <paramref name="Arguments"/> is a raw command line.</param>
/// <param name="Arguments">The arguments as they appear on the command line.</param>
/// <param name="WorkingDirPath">The working directory of the process.</param>
/// <param name="EnvironmentVariables">Variables to set, or to remove when the value is <see langword="null"/>.</param>
/// <param name="Credentials">The credentials to start the process with.</param>
/// <param name="ResolvedFilePath">
/// The full path to start, when it was resolved during preparation. Windows requires a resolved path
/// to prevent CreateProcess from searching the current and system directories before <c>PATH</c>.
/// </param>
internal sealed record ProcessCommand(
    string TargetFilePath,
    IReadOnlyList<string>? ArgumentList,
    string Arguments,
    string WorkingDirPath,
    IReadOnlyDictionary<string, string?> EnvironmentVariables,
    CommandLineCredentials? Credentials,
    string? ResolvedFilePath = null)
{
    public ProcessStartInfo CreateStartInfo()
    {
        var filePath = ResolvedFilePath;
        if (OperatingSystem.IsWindows() && filePath is null)
        {
            // Resolve at startup so interceptors can still handle commands without executing them.
            // Never let an unresolved name fall back to CreateProcess's implicit directory search.
            filePath = CliCommandFactory.ResolveWindowsCommand(TargetFilePath, WorkingDirPath, EnvironmentVariables)
                ?? throw new Win32Exception(2, $"Command '{TargetFilePath}' was not found on PATH or at the specified path.");
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = filePath ?? TargetFilePath,
            WorkingDirectory = WorkingDirPath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        if (ArgumentList is null)
        {
            startInfo.Arguments = Arguments;
        }
        else
        {
            foreach (var argument in ArgumentList)
            {
                startInfo.ArgumentList.Add(argument);
            }
        }

        foreach (var (key, value) in EnvironmentVariables)
        {
            if (value is null)
            {
                startInfo.Environment.Remove(key);
            }
            else
            {
                startInfo.Environment[key] = value;
            }
        }

        // Linux can protect the direct child at process creation. macOS does not support this API.
        // Windows uses pipeline-owned jobs: .NET 11 RC1 KillOnParentExit fails with "Access is denied"
        // inside a job object. Re-test it when the pinned SDK moves to .NET 11 GA.
        if (OperatingSystem.IsLinux())
        {
            startInfo.KillOnParentExit = true;
        }

        if (Credentials is not null)
        {
            CommandLineCredentialsMapper.Apply(Credentials, startInfo);
        }
        else if (OperatingSystem.IsWindows())
        {
            // Passing only the standard handles speeds up parallel starts on Windows. Unix hosts without
            // close_range or posix_spawn support fall back to a slow descriptor walk, so they keep the default.
            // Credentials start through CreateProcessWithLogonW, which keeps the default inheritance.
            startInfo.InheritedHandles = [];
        }

        if (OperatingSystem.IsWindows())
        {
            // Keeps a console Ctrl+C aimed at the pipeline from reaching the command.
            startInfo.CreateNewProcessGroup = true;
        }

        return startInfo;
    }

    public override string ToString() => $"{TargetFilePath} {Arguments}";
}
