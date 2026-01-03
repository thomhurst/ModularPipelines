using CliWrap;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration for how a command is executed, independent of tool-specific arguments.
/// </summary>
public record CommandExecutionOptions
{
    /// <summary>
    /// Gets any EnvironmentVariables to pass to the command.
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// Gets the working directory to run the command from.
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <inheritdoc cref="CommandLineCredentials"/>
    public Credentials? CommandLineCredentials { get; init; }

    /// <summary>
    /// Gets or sets logging options for command execution.
    /// </summary>
    public CommandLoggingOptions? LogSettings { get; init; }

    /// <summary>
    /// Gets if logging input, you can use this to edit how the input is logged.
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets if logging output, you can use this to edit how the output is logged.
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to prefix commands with Sudo.
    /// </summary>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception on non-zero exit codes.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    /// <summary>
    /// Gets or sets the maximum time allowed for the command to complete.
    /// </summary>
    public TimeSpan? ExecutionTimeout { get; init; }

    /// <summary>
    /// Gets or sets the time to wait for graceful shutdown before forcefully terminating.
    /// </summary>
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);

    internal bool InternalDryRun { get; set; }
}
