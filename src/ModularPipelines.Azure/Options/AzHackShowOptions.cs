using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hack", "show")]
public record AzHackShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

