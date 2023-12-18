using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "list")]
public record AzK8sExtensionExtensionTypesListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--release-train")]
    public string? ReleaseTrain { get; set; }
}