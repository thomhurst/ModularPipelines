using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "nodepool", "delete", "(aks-preview", "extension)")]
public record AzAksNodepoolDeleteAksPreviewExtensionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--ignore-pod-disruption-budget")]
    public bool? IgnorePodDisruptionBudget { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}