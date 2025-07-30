namespace ModularPipelines.Exceptions;

/// <summary>
/// An exception that occurs when a command execution fails.
/// </summary>
public class CommandException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandException"/> class.
    /// </summary>
    /// <param name="input">The command input that was executed.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="executionTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="innerException">The inner exception that caused this command failure.</param>
    public CommandException(string input, int exitCode, TimeSpan executionTime, string standardOutput, string standardError, Exception? innerException = null) : base(GenerateMessage(input, exitCode, executionTime, standardOutput, standardError), innerException)
    {
        ExitCode = exitCode;
        ExecutionTime = executionTime;
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    /// <summary>
    /// Gets the exit code returned by the failed command.
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Gets the time taken to execute the failed command.
    /// </summary>
    public TimeSpan ExecutionTime { get; }

    /// <summary>
    /// Gets the standard output from the failed command.
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Gets the standard error from the failed command.
    /// </summary>
    public string StandardError { get; }

    private static string GenerateMessage(string input, int exitCode, TimeSpan executionTime,
        string standardOutput, string standardError)
    {
        return $"""
               
               Input: {input}
               
               Error: {GetOutput(standardOutput, standardError)}
               
               Exit Code: {exitCode}
               """;
    }

    private static string GetOutput(string standardOutput, string standardError)
    {
        return !string.IsNullOrEmpty(standardError) ? standardError : standardOutput;
    }
}