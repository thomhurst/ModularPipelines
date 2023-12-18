using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "data-point", "list")]
public record AzIotOpsAssetDataPointListOptions(
[property: CommandSwitch("--asset")] string Asset,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--data-source")]
    public string? DataSource { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }
}