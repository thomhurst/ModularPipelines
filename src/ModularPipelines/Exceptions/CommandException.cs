using ModularPipelines.Models;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a CLI command executed by the framework fails with a non-zero exit code.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown by command execution methods when the underlying process
/// returns a non-zero exit code. Framework-thrown exceptions expose an obfuscated
/// command result for debugging and include its command input, but not command output,
/// in the exception message.
/// </para>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="Result"/> - The command result, including output and timing</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await context.Shell.RunAsync("mycommand", []);
/// }
/// catch (CommandException ex)
/// {
///     Console.WriteLine($"Command failed with exit code: {ex.Result.ExitCode}");
///     Console.WriteLine($"Error output: {ex.Result.StandardError}");
///     Console.WriteLine($"Execution time: {ex.Result.Duration}");
/// }
/// </code>
/// </remarks>
/// <seealso cref="PipelineException"/>
public class CommandException : PipelineException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="CommandException"/> class.
    /// </summary>
    /// <param name="result">The result of the failed command.</param>
    /// <param name="innerException">The inner exception that caused this command failure.</param>
    public CommandException(CommandResult result, Exception? innerException = null)
        : this($"Command failed with exit code {result.ExitCode}.", result, innerException)
    {
    }

    // CommandInput is embedded in the message, so callers must pass an already-obfuscated result.
    internal static CommandException FromAlreadyObfuscatedResult(
        CommandResult result,
        Exception? innerException = null) =>
        new(
            $"Command '{result.CommandInput}' failed with exit code {result.ExitCode}.",
            result,
            innerException);

    /// <summary>
    /// Initialises a new instance of the <see cref="CommandException"/> class with a custom message.
    /// </summary>
    /// <param name="message">The message that describes the command failure.</param>
    /// <param name="result">The result of the failed command.</param>
    /// <param name="innerException">The inner exception that caused this command failure.</param>
    protected CommandException(string message, CommandResult result, Exception? innerException = null)
        : base(message, innerException)
    {
        Result = result;
    }

    /// <summary>
    /// Gets the result of the failed command.
    /// </summary>
    public CommandResult Result { get; }
}
