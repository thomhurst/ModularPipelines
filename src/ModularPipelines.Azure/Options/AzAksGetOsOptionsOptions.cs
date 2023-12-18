using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "get-os-options")]
public record AzAksGetOsOptionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;