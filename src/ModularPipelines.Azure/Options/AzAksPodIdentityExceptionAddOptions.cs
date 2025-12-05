using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "pod-identity", "exception", "add")]
public record AzAksPodIdentityExceptionAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--pod-labels")] string PodLabels,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aks-custom-headers")]
    public string? AksCustomHeaders { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}