using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "distributed-tracing", "update")]
public record AzIotHubDistributedTracingUpdateOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--sampling-mode")] string SamplingMode,
[property: CliOption("--sampling-rate")] string SamplingRate
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}