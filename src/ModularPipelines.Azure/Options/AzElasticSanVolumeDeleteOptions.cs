using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("elastic-san", "volume", "delete")]
public record AzElasticSanVolumeDeleteOptions : AzOptions
{
    [CliOption("--elastic-san")]
    public string? ElasticSan { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--volume-group")]
    public string? VolumeGroup { get; set; }

    [CliFlag("--x-ms-delete-snapshots")]
    public bool? XMsDeleteSnapshots { get; set; }

    [CliFlag("--x-ms-force-delete")]
    public bool? XMsForceDelete { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}