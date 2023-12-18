using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "data-point", "add")]
public record AzIotOpsAssetDataPointAddOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--data-source")] string DataSource,
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