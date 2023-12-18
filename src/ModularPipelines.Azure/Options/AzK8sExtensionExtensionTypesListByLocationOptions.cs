using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "list-by-location")]
public record AzK8sExtensionExtensionTypesListByLocationOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--cluster-type")]
    public string? ClusterType { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--release-train")]
    public string? ReleaseTrain { get; set; }
}

