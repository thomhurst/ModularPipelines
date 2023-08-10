using CliWrap;

namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the context of a command
/// </summary>
public record CommandLineOptions
{
    /// <summary>
    /// Any EnvironmentVariables to pass to the command
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// The working directory to run the command from
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <inheritdoc cref="Credentials"/>>
    public Credentials? Credentials { get; init; }

    /// <summary>
    /// Whether to Log the input command in the pipeline output
    /// </summary>
    public bool LogInput { get; init; } = GlobalConfig.LogCommandInput;

    /// <summary>
    /// Whether to Log the command output in the pipeline output
    /// </summary>
    public bool LogOutput { get; init; } = GlobalConfig.LogCommandOutput;

    /// <summary>
    /// If logging input, you can use this to edit how the input is logged
    /// E.g. if you want to replace a secret value with stars
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// If logging output, you can use this to edit how the output is logged
    /// E.g. if you want to replace a secret value with stars
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    internal bool InternalDryRun { get; set; }
    public bool DoNotErrorOnNonZeroExitCode { get; set; }
}
