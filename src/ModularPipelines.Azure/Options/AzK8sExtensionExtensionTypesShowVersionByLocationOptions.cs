using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("k8s-extension", "extension-types", "show-version-by-location")]
public record AzK8sExtensionExtensionTypesShowVersionByLocationOptions(
[property: CliOption("--extension-type")] string ExtensionType,
[property: CliOption("--location")] string Location,
[property: CliOption("--version")] string Version
) : AzOptions;