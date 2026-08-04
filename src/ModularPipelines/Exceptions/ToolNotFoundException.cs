using ModularPipelines.Models;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when a command cannot start because its executable was not found.
/// </summary>
public sealed class ToolNotFoundException : CommandException
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ToolNotFoundException"/> class.
    /// </summary>
    /// <param name="executable">The executable that could not be found.</param>
    /// <param name="result">The result captured for the failed command start.</param>
    /// <param name="innerException">The native process-start failure.</param>
    public ToolNotFoundException(
        string executable,
        CommandResult result,
        Exception? innerException = null)
        : base(
            $"Executable '{executable}' was not found on PATH. "
            + "Install it or add it to PATH; see context.Installers for scripted installation.",
            result,
            innerException)
    {
        Executable = executable;
    }

    /// <summary>
    /// Gets the executable that could not be found.
    /// </summary>
    public string Executable { get; }
}
