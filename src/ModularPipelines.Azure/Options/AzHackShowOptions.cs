using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hack", "show")]
public record AzHackShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;