using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "volume", "delete")]
public record AzElasticSanVolumeDeleteOptions(
[property: CommandSwitch("--elastic-san")] string ElasticSan,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--x-ms-delete-snapshots")]
    public bool? XMsDeleteSnapshots { get; set; }

    [BooleanCommandSwitch("--x-ms-force-delete")]
    public bool? XMsForceDelete { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}