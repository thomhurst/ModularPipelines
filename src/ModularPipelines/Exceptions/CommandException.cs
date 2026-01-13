namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a CLI command executed by the framework fails with a non-zero exit code.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown by command execution methods when the underlying process
/// returns a non-zero exit code. The exception contains the full command output
/// for debugging purposes.
/// </para>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="ExitCode"/> - The non-zero exit code returned by the command</item>
/// <item><see cref="ExecutionTime"/> - How long the command ran before failing</item>
/// <item><see cref="StandardOutput"/> - The standard output stream content</item>
/// <item><see cref="StandardError"/> - The standard error stream content</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("mycommand"));
/// }
/// catch (CommandException ex)
/// {
///     Console.WriteLine($"Command failed with exit code: {ex.ExitCode}");
///     Console.WriteLine($"Error output: {ex.StandardError}");
///     Console.WriteLine($"Execution time: {ex.ExecutionTime}");
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
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