using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-san", "volume", "snapshot", "create")]
public record AzElasticSanVolumeSnapshotCreateOptions(
[property: CliOption("--creation-data")] string CreationData,
[property: CliOption("--elastic-san")] string ElasticSan,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}