using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "get-versions")]
public record AzAksGetVersionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;