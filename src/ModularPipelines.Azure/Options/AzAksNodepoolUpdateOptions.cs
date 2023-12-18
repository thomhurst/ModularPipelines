using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "update")]
public record AzAksNodepoolUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [BooleanCommandSwitch("--allowed-host-ports")]
    public bool? AllowedHostPorts { get; set; }

    [CommandSwitch("--asg-ids")]
    public string? AsgIds { get; set; }

    [BooleanCommandSwitch("--disable-cluster-autoscaler")]
    public bool? DisableClusterAutoscaler { get; set; }

    [CommandSwitch("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [BooleanCommandSwitch("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--max-surge")]
    public string? MaxSurge { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-taints")]
    public string? NodeTaints { get; set; }

    [CommandSwitch("--scale-down-mode")]
    public string? ScaleDownMode { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--update-cluster-autoscaler")]
    public bool? UpdateClusterAutoscaler { get; set; }
}