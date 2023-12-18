using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "dapr", "enable")]
public record AzContainerappDaprEnableOptions : AzOptions
{
    [BooleanCommandSwitch("--dal")]
    public bool? Dal { get; set; }

    [CommandSwitch("--dapr-app-id")]
    public string? DaprAppId { get; set; }

    [CommandSwitch("--dapr-app-port")]
    public string? DaprAppPort { get; set; }

    [CommandSwitch("--dapr-app-protocol")]
    public string? DaprAppProtocol { get; set; }

    [CommandSwitch("--dapr-http-max-request-size")]
    public string? DaprHttpMaxRequestSize { get; set; }

    [CommandSwitch("--dapr-http-read-buffer-size")]
    public string? DaprHttpReadBufferSize { get; set; }

    [CommandSwitch("--dapr-log-level")]
    public string? DaprLogLevel { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}