using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "volume", "snapshot", "create")]
public record AzElasticSanVolumeSnapshotCreateOptions(
[property: CommandSwitch("--creation-data")] string CreationData,
[property: CommandSwitch("--elastic-san")] string ElasticSan,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--volume-group")] string VolumeGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}