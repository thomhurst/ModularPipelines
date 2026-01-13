using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Base options class for Chocolatey commands.
/// Contains common flags available across most choco commands.
/// </summary>
[ExcludeFromCodeCoverage]
[CliTool("choco")]
public record ChocolateyOptions : CommandLineToolOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to show debug messages.
    /// </summary>
    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show verbose messages.
    /// </summary>
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show trace messages.
    /// </summary>
    [CliFlag("--trace")]
    public virtual bool? Trace { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not show progress.
    /// </summary>
    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to confirm all prompts automatically.
    /// </summary>
    [CliFlag("--yes")]
    public virtual bool? Yes { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to force the operation.
    /// </summary>
    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to perform a dry run (noop).
    /// </summary>
    [CliFlag("--noop")]
    public virtual bool? Noop { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to limit output.
    /// </summary>
    [CliFlag("--limit-output")]
    public virtual bool? LimitOutput { get; set; }

    /// <summary>
    /// Gets or sets the execution timeout in seconds.
    /// </summary>
    [CliFlag("--execution-timeout")]
    public virtual int? ExecutionTimeout { get; set; }

    /// <summary>
    /// Gets or sets the cache location path.
    /// </summary>
    [CliFlag("--cache-location")]
    public virtual string? CacheLocation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept license automatically.
    /// </summary>
    [CliFlag("--accept-license")]
    public virtual bool? AcceptLicense { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to confirm all prompts for scripts.
    /// </summary>
    [CliFlag("--confirm")]
    public virtual bool? Confirm { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show help information.
    /// </summary>
    [CliFlag("--help")]
    public virtual bool? Help { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show version information.
    /// </summary>
    [CliFlag("--version")]
    public virtual bool? Version { get; set; }
}
