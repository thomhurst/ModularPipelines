using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "list-versions")]
public record AzK8sExtensionExtensionTypesListVersionsOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--cluster-type")] string ClusterType,
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--major-version")]
    public string? MajorVersion { get; set; }

    [CommandSwitch("--release-train")]
    public string? ReleaseTrain { get; set; }

    [BooleanCommandSwitch("--show-latest")]
    public bool? ShowLatest { get; set; }
}