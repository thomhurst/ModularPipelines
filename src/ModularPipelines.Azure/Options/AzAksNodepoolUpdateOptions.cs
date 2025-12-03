using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool", "update")]
public record AzAksNodepoolUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliFlag("--allowed-host-ports")]
    public bool? AllowedHostPorts { get; set; }

    [CliOption("--asg-ids")]
    public string? AsgIds { get; set; }

    [CliFlag("--disable-cluster-autoscaler")]
    public bool? DisableClusterAutoscaler { get; set; }

    [CliOption("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [CliFlag("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--max-surge")]
    public string? MaxSurge { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-taints")]
    public string? NodeTaints { get; set; }

    [CliOption("--scale-down-mode")]
    public string? ScaleDownMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--update-cluster-autoscaler")]
    public bool? UpdateClusterAutoscaler { get; set; }
}