using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adp", "check-name-availability")]
public record AzAdpCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AzOptions;