using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Google.Options;

/// <summary>
/// Base options class for gcloud CLI commands.
/// </summary>
[ExcludeFromCodeCoverage]
public record GcloudOptions() : CommandLineToolOptions("gcloud")
{
    /// <summary>
    /// Disable all interactive prompts when running gcloud commands.
    /// </summary>
    [CliOption("--quiet")]
    public virtual bool? Quiet { get; set; }

    /// <summary>
    /// Override the default verbosity for this command.
    /// </summary>
    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    /// <summary>
    /// Format the output.
    /// </summary>
    [CliOption("--format")]
    public virtual string? Format { get; set; }

    /// <summary>
    /// The Google Cloud project ID to use for this invocation.
    /// </summary>
    [CliOption("--project")]
    public virtual string? Project { get; set; }

    /// <summary>
    /// Google Cloud user account to use for invocation.
    /// </summary>
    [CliOption("--account")]
    public virtual string? Account { get; set; }

    /// <summary>
    /// Named configuration to use for this command invocation.
    /// </summary>
    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    /// <summary>
    /// Print user intended output to the console.
    /// </summary>
    [CliOption("--user-output-enabled")]
    public virtual bool? UserOutputEnabled { get; set; }

    /// <summary>
    /// Token used to route traces of service requests for investigation of issues.
    /// </summary>
    [CliOption("--trace-token")]
    public virtual string? TraceToken { get; set; }

    /// <summary>
    /// Log all HTTP server requests and responses to stderr.
    /// </summary>
    [CliOption("--log-http")]
    public virtual bool? LogHttp { get; set; }
}
