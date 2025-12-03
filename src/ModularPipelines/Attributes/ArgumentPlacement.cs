namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies where a positional argument should be placed relative to options/flags.
/// </summary>
public enum ArgumentPlacement
{
    /// <summary>
    /// Argument is placed after all options and flags (default).
    /// Example: helm install [OPTIONS] RELEASE_NAME CHART
    /// </summary>
    AfterOptions,

    /// <summary>
    /// Argument is placed before any options and flags.
    /// Example: git -C PATH [OPTIONS] COMMAND
    /// </summary>
    BeforeOptions,

    /// <summary>
    /// Argument is placed immediately after the command, before options.
    /// Example: docker run IMAGE [OPTIONS]
    /// </summary>
    ImmediatelyAfterCommand,
}
