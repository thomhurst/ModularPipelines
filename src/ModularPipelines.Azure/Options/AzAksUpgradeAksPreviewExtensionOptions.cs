using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "upgrade", "(aks-preview", "extension)")]
public record AzAksUpgradeAksPreviewExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CommandSwitch("--cluster-snapshot-id")]
    public string? ClusterSnapshotId { get; set; }

    [BooleanCommandSwitch("--control-plane-only")]
    public bool? ControlPlaneOnly { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--node-image-only")]
    public bool? NodeImageOnly { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}