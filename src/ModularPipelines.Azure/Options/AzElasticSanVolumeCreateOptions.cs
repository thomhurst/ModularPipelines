using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san", "volume", "create")]
public record AzElasticSanVolumeCreateOptions(
[property: CliOption("--elastic-san")] string ElasticSan,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--size-gib")] string SizeGib,
[property: CliOption("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CliOption("--creation-data")]
    public string? CreationData { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}