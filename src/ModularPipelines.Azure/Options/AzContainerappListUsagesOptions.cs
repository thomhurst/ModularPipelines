using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "list-usages")]
public record AzContainerappListUsagesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;