namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing command execution logging.
/// </summary>
public record CommandLoggingOptions
{
    /// <summary>
    /// Gets or sets the verbosity level. Default is Normal.
    /// </summary>
    public CommandLogVerbosity Verbosity { get; init; } = CommandLogVerbosity.Normal;

    /// <summary>
    /// Gets or sets whether to include timestamps in output. Default is false.
    /// </summary>
    public bool IncludeTimestamps { get; init; }

    /// <summary>
    /// Gets or sets whether to show command arguments. Default is true.
    /// </summary>
    public bool ShowCommandArguments { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show standard output. Default is true.
    /// </summary>
    public bool ShowStandardOutput { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show standard error. Default is true.
    /// </summary>
    public bool ShowStandardError { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show the exit code. Default is false.
    /// </summary>
    public bool ShowExitCode { get; init; }

    /// <summary>
    /// Gets or sets whether to show the working directory. Default is false.
    /// </summary>
    public bool ShowWorkingDirectory { get; init; }

    /// <summary>
    /// Gets or sets whether to show execution time. Default is false.
    /// </summary>
    public bool ShowExecutionTime { get; init; }

    /// <summary>
    /// Default logging options (Normal verbosity, all standard options enabled).
    /// </summary>
    public static CommandLoggingOptions Default { get; } = new();

    /// <summary>
    /// Silent logging options (no output).
    /// </summary>
    public static CommandLoggingOptions Silent { get; } = new() { Verbosity = CommandLogVerbosity.Silent };

    /// <summary>
    /// Diagnostic logging options (maximum verbosity, all options enabled).
    /// </summary>
    public static CommandLoggingOptions Diagnostic { get; } = new()
    {
        Verbosity = CommandLogVerbosity.Diagnostic,
        IncludeTimestamps = true,
        ShowCommandArguments = true,
        ShowStandardOutput = true,
        ShowStandardError = true,
        ShowExitCode = true,
        ShowWorkingDirectory = true,
        ShowExecutionTime = true
    };
}
