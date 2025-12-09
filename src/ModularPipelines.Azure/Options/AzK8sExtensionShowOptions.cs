using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("k8s-extension", "show")]
public record AzK8sExtensionShowOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cluster-resource-provider")]
    public string? ClusterResourceProvider { get; set; }
}