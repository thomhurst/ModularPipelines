using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "upgrade")]
public record AzAksNodepoolUpgradeOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--max-surge")]
    public string? MaxSurge { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--node-image-only")]
    public bool? NodeImageOnly { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}