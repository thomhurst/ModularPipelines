using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "distributed-tracing", "update")]
public record AzIotHubDistributedTracingUpdateOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--sampling-mode")] string SamplingMode,
[property: CommandSwitch("--sampling-rate")] string SamplingRate
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}