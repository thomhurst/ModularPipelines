using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastic-san", "volume", "snapshot", "show")]
public record AzElasticSanVolumeSnapshotShowOptions : AzOptions
{
    [CliOption("--elastic-san")]
    public string? ElasticSan { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--volume-group")]
    public string? VolumeGroup { get; set; }
}