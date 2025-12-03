using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-san", "volume", "snapshot", "list")]
public record AzElasticSanVolumeSnapshotListOptions(
[property: CliOption("--elastic-san")] string ElasticSan,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}