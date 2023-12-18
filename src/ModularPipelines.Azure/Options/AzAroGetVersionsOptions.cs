using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aro", "get-versions")]
public record AzAroGetVersionsOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;