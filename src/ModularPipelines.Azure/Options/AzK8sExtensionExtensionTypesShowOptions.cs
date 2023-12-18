using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("k8s-extension", "extension-types", "show")]
public record AzK8sExtensionExtensionTypesShowOptions(
[property: CommandSwitch("--extension-type")] string ExtensionType,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

