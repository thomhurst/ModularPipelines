using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "services", "update")]
public record GcloudEdgeCacheServicesUpdateOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--edge-security-policy")]
    public string? EdgeSecurityPolicy { get; set; }

    [CliOption("--edge-ssl-certificate")]
    public string[]? EdgeSslCertificate { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--logging-sample-rate")]
    public string? LoggingSampleRate { get; set; }

    [CliFlag("--require-tls")]
    public bool? RequireTls { get; set; }
}