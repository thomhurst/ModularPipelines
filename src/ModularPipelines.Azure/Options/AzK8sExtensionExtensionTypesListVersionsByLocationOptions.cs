using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "extension-types", "list-versions-by-location")]
public record AzK8sExtensionExtensionTypesListVersionsByLocationOptions(
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--cluster-type")]
    public string? ClusterType { get; set; }

    [CliOption("--major-version")]
    public string? MajorVersion { get; set; }

    [CliOption("--release-train")]
    public string? ReleaseTrain { get; set; }

    [CliFlag("--show-latest")]
    public bool? ShowLatest { get; set; }
}