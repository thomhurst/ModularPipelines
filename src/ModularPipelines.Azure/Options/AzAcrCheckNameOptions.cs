using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "check-name")]
public record AzAcrCheckNameOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;