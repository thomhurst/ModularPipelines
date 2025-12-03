using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "dapr", "enable")]
public record AzContainerappDaprEnableOptions : AzOptions
{
    [CliFlag("--dal")]
    public bool? Dal { get; set; }

    [CliOption("--dapr-app-id")]
    public string? DaprAppId { get; set; }

    [CliOption("--dapr-app-port")]
    public string? DaprAppPort { get; set; }

    [CliOption("--dapr-app-protocol")]
    public string? DaprAppProtocol { get; set; }

    [CliOption("--dapr-http-max-request-size")]
    public string? DaprHttpMaxRequestSize { get; set; }

    [CliOption("--dapr-http-read-buffer-size")]
    public string? DaprHttpReadBufferSize { get; set; }

    [CliOption("--dapr-log-level")]
    public string? DaprLogLevel { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}