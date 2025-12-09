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
    /// Gets or sets disable all interactive prompts when running gcloud commands.
    /// </summary>
    [CliOption("--quiet")]
    public virtual bool? Quiet { get; set; }

    /// <summary>
    /// Gets or sets override the default verbosity for this command.
    /// </summary>
    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    /// <summary>
    /// Gets or sets format the output.
    /// </summary>
    [CliOption("--format")]
    public virtual string? Format { get; set; }

    /// <summary>
    /// Gets or sets the Google Cloud project ID to use for this invocation.
    /// </summary>
    [CliOption("--project")]
    public virtual string? Project { get; set; }

    /// <summary>
    /// Gets or sets google Cloud user account to use for invocation.
    /// </summary>
    [CliOption("--account")]
    public virtual string? Account { get; set; }

    /// <summary>
    /// Gets or sets named configuration to use for this command invocation.
    /// </summary>
    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    /// <summary>
    /// Gets or sets print user intended output to the console.
    /// </summary>
    [CliOption("--user-output-enabled")]
    public virtual bool? UserOutputEnabled { get; set; }

    /// <summary>
    /// Gets or sets token used to route traces of service requests for investigation of issues.
    /// </summary>
    [CliOption("--trace-token")]
    public virtual string? TraceToken { get; set; }

    /// <summary>
    /// Gets or sets log all HTTP server requests and responses to stderr.
    /// </summary>
    [CliOption("--log-http")]
    public virtual bool? LogHttp { get; set; }
}
