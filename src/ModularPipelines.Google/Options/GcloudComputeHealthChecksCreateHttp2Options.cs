using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "health-checks", "create", "http2")]
public record GcloudComputeHealthChecksCreateHttp2Options(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--check-interval")]
    public string? CheckInterval { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-logging")]
    public bool? EnableLogging { get; set; }

    [CommandSwitch("--healthy-threshold")]
    public string? HealthyThreshold { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CommandSwitch("--request-path")]
    public string? RequestPath { get; set; }

    [CommandSwitch("--response")]
    public string? Response { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--unhealthy-threshold")]
    public string? UnhealthyThreshold { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--port-name")]
    public string? PortName { get; set; }

    [BooleanCommandSwitch("--use-serving-port")]
    public bool? UseServingPort { get; set; }
}