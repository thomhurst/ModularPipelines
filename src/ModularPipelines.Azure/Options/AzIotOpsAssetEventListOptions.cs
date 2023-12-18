using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "event", "list")]
public record AzIotOpsAssetEventListOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--en")]
    public string? En { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}