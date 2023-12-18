using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "show-version-by-location")]
public record AzK8sExtensionExtensionTypesShowVersionByLocationOptions(
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--version")] string Version
) : AzOptions;