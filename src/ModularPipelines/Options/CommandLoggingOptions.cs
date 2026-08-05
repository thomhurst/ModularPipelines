namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing command execution logging.
/// </summary>
/// <remarks>
/// <para>Set via <see cref="CommandExecutionOptions.LogSettings"/> or <see cref="PipelineCommandOptions.Logging"/>.</para>
/// <para>Verbosity levels control what is logged automatically:</para>
/// <list type="bullet">
/// <item><description><see cref="CommandLogVerbosity.Silent"/> - No logging</description></item>
/// <item><description><see cref="CommandLogVerbosity.Minimal"/> - Command input only</description></item>
/// <item><description><see cref="CommandLogVerbosity.Normal"/> - Input, output, and errors on failure</description></item>
/// <item><description><see cref="CommandLogVerbosity.Detailed"/> - Above plus exit code and duration</description></item>
/// <item><description><see cref="CommandLogVerbosity.Diagnostic"/> - Everything including working directory and timestamps</description></item>
/// </list>
/// <para>Individual Show* properties can disable features provided by the verbosity level.</para>
/// </remarks>
public record CommandLoggingOptions
{
    /// <summary>
    /// Gets the verbosity level. Default is Normal.
    /// </summary>
    public CommandLogVerbosity Verbosity { get; init; } = CommandLogVerbosity.Normal;

    /// <summary>
    /// Gets a value indicating whether timestamps are included in output. Default is false.
    /// </summary>
    public bool IncludeTimestamps { get; init; }

    /// <summary>
    /// Gets a value indicating whether command arguments are shown. Default is true.
    /// </summary>
    public bool ShowCommandArguments { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether standard output is shown. Default is true.
    /// </summary>
    public bool ShowStandardOutput { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether standard error is shown. Default is true.
    /// </summary>
    public bool ShowStandardError { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the exit code is shown. Default is false.
    /// </summary>
    public bool ShowExitCode { get; init; }

    /// <summary>
    /// Gets a value indicating whether the working directory is shown. Default is false.
    /// </summary>
    public bool ShowWorkingDirectory { get; init; }

    /// <summary>
    /// Gets a value indicating whether execution time is shown. Default is false.
    /// </summary>
    public bool ShowExecutionTime { get; init; }

    /// <summary>
    /// Gets the default logging options (Normal verbosity, all standard options enabled).
    /// </summary>
    public static CommandLoggingOptions Default { get; } = new();

    /// <summary>
    /// Gets silent logging options that produce no output.
    /// </summary>
    public static CommandLoggingOptions Silent { get; } = new() { Verbosity = CommandLogVerbosity.Silent };

    /// <summary>
    /// Gets diagnostic logging options with maximum verbosity and all options enabled.
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
        ShowExecutionTime = true,
    };
}
