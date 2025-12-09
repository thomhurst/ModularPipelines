using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san", "volume", "list")]
public record AzElasticSanVolumeListOptions(
[property: CliOption("--elastic-san")] string ElasticSan,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--volume-group")] string VolumeGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}