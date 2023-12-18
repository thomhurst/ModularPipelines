using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "event", "add")]
public record AzIotOpsAssetEventAddOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--en")] string En,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--capability-id")]
    public string? CapabilityId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--observability-mode")]
    public string? ObservabilityMode { get; set; }

    [CommandSwitch("--qs")]
    public string? Qs { get; set; }

    [CommandSwitch("--sampling-interval")]
    public string? SamplingInterval { get; set; }
}