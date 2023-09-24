using CliWrap;
using ModularPipelines.Enums;

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

    /// <inheritdoc cref="CommandLineCredentials"/>>
    public Credentials? CommandLineCredentials { get; init; }
    
    /// <summary>
    /// Controls command logging
    /// e.g. Logging = CommandLogging.Input | CommandLogging.Output | CommandLogging.Error;
    /// </summary>
    public CommandLogging? CommandLogging { get; init; }

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

    /// <summary>
    /// Prefix commands with Sudo to run with elevated priveliges for Unix systems
    /// </summary>
    public bool Sudo { get; set; }
    
    internal bool InternalDryRun { get; set; }
    public bool ThrowOnNonZeroExitCode { get; set; } = true;
}
