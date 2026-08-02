using System.Diagnostics.CodeAnalysis;
using CliWrap;

namespace ModularPipelines.Models;

public record CommandResult
{
    /// <summary>
    /// Gets the command that was executed.
    /// </summary>
    public required string CommandInput { get; init; }

    public required IReadOnlyDictionary<string, string?> EnvironmentVariables { get; init; }

    public required string WorkingDirectory { get; init; }

    [SetsRequiredMembers]
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

    [SetsRequiredMembers]
    internal CommandResult(
        Command command,
        IReadOnlyDictionary<string, string?> environmentVariables)
    {
        var completedAt = DateTimeOffset.UtcNow;

        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = environmentVariables;
        StandardOutput = string.Empty;
        StandardError = string.Empty;
        ExitCode = 0;
        StartTime = completedAt;
        EndTime = completedAt;
        Duration = TimeSpan.Zero;
    }

    [SetsRequiredMembers]
    internal CommandResult(
        Command command,
        CliWrap.CommandResult commandResult,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> environmentVariables)
    {
        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = environmentVariables;

        StandardOutput = standardOutput;
        StandardError = standardError;
        StartTime = commandResult.StartTime;
        EndTime = commandResult.ExitTime;
        Duration = commandResult.RunTime;

        ExitCode = commandResult.ExitCode;
    }

    /// <summary>
    /// Gets standard output data produced by the underlying process.
    /// </summary>
    public required string StandardOutput { get; init; }

    /// <summary>
    /// Gets standard error data produced by the underlying process.
    /// </summary>
    public required string StandardError { get; init; }

    /// <summary>
    /// Gets exit code set by the underlying process.
    /// </summary>
    public required int ExitCode { get; init; }

    /// <summary>
    /// Gets time at which the command started executing.
    /// </summary>
    public required DateTimeOffset StartTime { get; init; }

    /// <summary>
    /// Gets time at which the command finished executing.
    /// </summary>
    public required DateTimeOffset EndTime { get; init; }

    /// <summary>
    /// Gets total duration of the command execution.
    /// </summary>
    public required TimeSpan Duration { get; init; }
}
