using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "upgrade", "(aks-preview", "extension)")]
public record AzAksUpgradeAksPreviewExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliOption("--cluster-snapshot-id")]
    public string? ClusterSnapshotId { get; set; }

    [CliFlag("--control-plane-only")]
    public bool? ControlPlaneOnly { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--node-image-only")]
    public bool? NodeImageOnly { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}