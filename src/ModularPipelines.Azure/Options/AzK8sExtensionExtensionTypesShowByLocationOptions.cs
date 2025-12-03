using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "extension-types", "show-by-location")]
public record AzK8sExtensionExtensionTypesShowByLocationOptions(
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--location")] string Location
) : AzOptions;