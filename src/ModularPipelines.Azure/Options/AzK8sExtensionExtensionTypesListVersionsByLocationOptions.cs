using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "list-versions-by-location")]
public record AzK8sExtensionExtensionTypesListVersionsByLocationOptions(
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--cluster-type")]
    public string? ClusterType { get; set; }

    [CommandSwitch("--major-version")]
    public string? MajorVersion { get; set; }

    [CommandSwitch("--release-train")]
    public string? ReleaseTrain { get; set; }

    [BooleanCommandSwitch("--show-latest")]
    public bool? ShowLatest { get; set; }
}