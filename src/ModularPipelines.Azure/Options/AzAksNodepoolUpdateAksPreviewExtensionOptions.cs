using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "nodepool", "update", "(aks-preview", "extension)")]
public record AzAksNodepoolUpdateAksPreviewExtensionOptions(
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

    [CliFlag("--dcat")]
    public bool? Dcat { get; set; }

    [CliFlag("--disable-cluster-autoscaler")]
    public bool? DisableClusterAutoscaler { get; set; }

    [CliOption("--drain-timeout")]
    public string? DrainTimeout { get; set; }

    [CliFlag("--enable-artifact-streaming")]
    public bool? EnableArtifactStreaming { get; set; }

    [CliFlag("--enable-cluster-autoscaler")]
    public bool? EnableClusterAutoscaler { get; set; }

    [CliFlag("--enable-custom-ca-trust")]
    public bool? EnableCustomCaTrust { get; set; }

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

    [CliOption("--node-soak-duration")]
    public string? NodeSoakDuration { get; set; }

    [CliOption("--node-taints")]
    public string? NodeTaints { get; set; }

    [CliOption("--os-sku")]
    public string? OsSku { get; set; }

    [CliOption("--scale-down-mode")]
    public string? ScaleDownMode { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--update-cluster-autoscaler")]
    public bool? UpdateClusterAutoscaler { get; set; }
}