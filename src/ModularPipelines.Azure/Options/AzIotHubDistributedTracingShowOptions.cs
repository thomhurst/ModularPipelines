using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "hub", "distributed-tracing", "show")]
public record AzIotHubDistributedTracingShowOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--hub-name")] string HubName
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}