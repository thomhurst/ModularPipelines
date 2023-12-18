using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "show-by-location")]
public record AzK8sExtensionExtensionTypesShowByLocationOptions(
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--location")] string Location
) : AzOptions;