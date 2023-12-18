using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastic-san", "volume", "delete")]
public record AzElasticSanVolumeDeleteOptions : AzOptions
{
    [CommandSwitch("--elastic-san")]
    public string? ElasticSan { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--volume-group")]
    public string? VolumeGroup { get; set; }

    [BooleanCommandSwitch("--x-ms-delete-snapshots")]
    public bool? XMsDeleteSnapshots { get; set; }

    [BooleanCommandSwitch("--x-ms-force-delete")]
    public bool? XMsForceDelete { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}