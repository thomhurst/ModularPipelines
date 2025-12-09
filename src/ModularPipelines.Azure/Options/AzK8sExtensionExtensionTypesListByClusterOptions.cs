using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("k8s-extension", "extension-types", "list-by-cluster")]
public record AzK8sExtensionExtensionTypesListByClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--cluster-type")] string ClusterType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--release-train")]
    public string? ReleaseTrain { get; set; }
}