using System.Diagnostics.CodeAnalysis;
using CliWrap;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.Models;

public record CommandResult
{
    /// <summary>
    /// Gets the command that was executed.
    /// </summary>
    public string CommandInput { get; }

    public IReadOnlyDictionary<string, string?> EnvironmentVariables { get; }

    public string WorkingDirectory { get; }

    public CommandResult(
        string commandInput,
        string workingDirectory,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> environmentVariables,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        TimeSpan duration,
        int exitCode
        )
    {
        CommandInput = commandInput;
        WorkingDirectory = workingDirectory;
        StandardOutput = standardOutput;
        StandardError = standardError;
        EnvironmentVariables = environmentVariables;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
        ExitCode = exitCode;
    }

#if NET7_0_OR_GREATER
    [SetsRequiredMembersAttribute]
#endif
#pragma warning disable CS8618
    internal CommandResult(Command command)
#pragma warning restore CS8618
    {
        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = GetPublicEnvironmentVariables(command);
    }

    internal CommandResult(Command command, CliWrap.CommandResult commandResult, string standardOutput, string standardError)
    {
        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = GetPublicEnvironmentVariables(command);

        StandardOutput = standardOutput;
        StandardError = standardError;
        StartTime = commandResult.StartTime;
        EndTime = commandResult.ExitTime;
        Duration = commandResult.RunTime;

        ExitCode = commandResult.ExitCode;
    }

    private static IReadOnlyDictionary<string, string?> GetPublicEnvironmentVariables(Command command)
    {
        var environmentVariables = command.EnvironmentVariables;
        return environmentVariables.Keys.Any(CliCommandFactory.IsInternalEnvironmentVariable)
            ? environmentVariables
                .Where(pair => !CliCommandFactory.IsInternalEnvironmentVariable(pair.Key))
                .ToDictionary(StringComparer.OrdinalIgnoreCase)
            : environmentVariables;
    }

    /// <summary>
    /// Gets standard output data produced by the underlying process.
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Gets standard error data produced by the underlying process.
    /// </summary>
    public string StandardError { get; }

    /// <summary>
    /// Gets exit code set by the underlying process.
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Gets time at which the command started executing.
    /// </summary>
    public DateTimeOffset StartTime { get; }

    /// <summary>
    /// Gets time at which the command finished executing.
    /// </summary>
    public DateTimeOffset EndTime { get; }

    /// <summary>
    /// Gets total duration of the command execution.
    /// </summary>
    public TimeSpan Duration { get; }
}
