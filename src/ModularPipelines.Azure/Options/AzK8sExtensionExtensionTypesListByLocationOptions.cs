using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "extension-types", "list-by-location")]
public record AzK8sExtensionExtensionTypesListByLocationOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--cluster-type")]
    public string? ClusterType { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--release-train")]
    public string? ReleaseTrain { get; set; }
}