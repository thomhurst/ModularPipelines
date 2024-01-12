using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "services", "update")]
public record GcloudEdgeCacheServicesUpdateOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--edge-security-policy")]
    public string? EdgeSecurityPolicy { get; set; }

    [CommandSwitch("--edge-ssl-certificate")]
    public string[]? EdgeSslCertificate { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--logging-sample-rate")]
    public string? LoggingSampleRate { get; set; }

    [BooleanCommandSwitch("--require-tls")]
    public bool? RequireTls { get; set; }
}