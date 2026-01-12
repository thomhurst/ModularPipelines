namespace ModularPipelines.Exceptions;

/// <summary>
/// An exception that occurs when a command execution fails.
/// </summary>
public class CommandException : PipelineException
{
    private readonly Lazy<string> _formattedMessage;

    /// <summary>
    /// Initialises a new instance of the <see cref="CommandException"/> class.
    /// Initializes a new instance of the <see cref="CommandException"/> class.
    /// </summary>
    /// <param name="input">The command input that was executed.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="executionTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="innerException">The inner exception that caused this command failure.</param>
    public CommandException(string input, int exitCode, TimeSpan executionTime, string standardOutput, string standardError, Exception? innerException = null)
        : base($"Command failed with exit code {exitCode}", innerException)
    {
        Input = input;
        ExitCode = exitCode;
        ExecutionTime = executionTime;
        StandardOutput = standardOutput;
        StandardError = standardError;

        _formattedMessage = new Lazy<string>(() => $"""

               Input: {Input}

               Error: {GetOutput(StandardOutput, StandardError)}

               Exit Code: {ExitCode}
               """);
    }

    /// <inheritdoc />
    public override string Message => _formattedMessage.Value;

    /// <summary>
    /// Gets the command input that was executed.
    /// </summary>
    public string Input { get; }

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

    private static string GetOutput(string standardOutput, string standardError)
    {
        return !string.IsNullOrEmpty(standardError) ? standardError : standardOutput;
    }
}