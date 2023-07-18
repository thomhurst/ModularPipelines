using CliWrap;
using CliWrap.Buffered;

namespace ModularPipelines.Models;

public class CommandResult
{
    /// <summary>
    /// The command that was executed.
    /// </summary>
    public string CommandInput { get; }
    public IReadOnlyDictionary<string, string?> EnvironmentVariables { get; }
    public string WorkingDirectory { get; }


    // For Dry-Run Unit Tests
    //[SetsRequiredMembersAttribute] TODO Once we upgrade past .NET 7
#pragma warning disable CS8618
    internal CommandResult(Command command)
#pragma warning restore CS8618
    {
        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = command.EnvironmentVariables;
    }

    internal CommandResult(Command command, BufferedCommandResult commandResult)
    {
        CommandInput = command.ToString();
        WorkingDirectory = command.WorkingDirPath;
        EnvironmentVariables = command.EnvironmentVariables;

        StandardOutput = commandResult.StandardOutput;
        StandardError = commandResult.StandardError;
        StartTime = commandResult.StartTime;
        EndTime = commandResult.ExitTime;
        Duration = commandResult.RunTime;
    }

    /// <summary>
    /// Standard output data produced by the underlying process.
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Standard error data produced by the underlying process.
    /// </summary>
    public string StandardError { get; }

    /// <summary>
    /// Exit code set by the underlying process.
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Time at which the command started executing.
    /// </summary>
    public DateTimeOffset StartTime { get; }

    /// <summary>
    /// Time at which the command finished executing.
    /// </summary>
    public DateTimeOffset EndTime { get; }

    /// <summary>
    /// Total duration of the command execution.
    /// </summary>
    public TimeSpan Duration { get; }
}
