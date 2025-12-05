using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "health-checks", "update", "tcp")]
public record GcloudComputeHealthChecksUpdateTcpOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--check-interval")]
    public string? CheckInterval { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CliOption("--healthy-threshold")]
    public string? HealthyThreshold { get; set; }

    [CliOption("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CliOption("--request")]
    public string? Request { get; set; }

    [CliOption("--response")]
    public string? Response { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--unhealthy-threshold")]
    public string? UnhealthyThreshold { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--port-name")]
    public string? PortName { get; set; }

    [CliFlag("--use-serving-port")]
    public bool? UseServingPort { get; set; }
}