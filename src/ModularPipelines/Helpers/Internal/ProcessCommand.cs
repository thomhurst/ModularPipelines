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
/// The full path to start, when it was resolved. CreateProcess searches the system directories before
/// <c>PATH</c>, so <c>bash</c> would otherwise start WSL's <c>System32\bash.exe</c> on Windows.
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
        var startInfo = new ProcessStartInfo
        {
            FileName = ResolvedFilePath ?? TargetFilePath,
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

        // KillOnParentExit is deliberately not set: on .NET 11 RC1 every Windows start fails with
        // "Access is denied" when the pipeline itself runs inside a job object, as CI runners do.
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
