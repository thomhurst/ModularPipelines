using CliWrap.Buffered;

namespace ModularPipelines.Models;

public class CommandResult
{
    /// <summary>
    /// The command that was executed.
    /// </summary>
    public string CommandInput { get; }

    internal CommandResult(string commandInput, BufferedCommandResult commandResult)
    {
        CommandInput = commandInput;
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