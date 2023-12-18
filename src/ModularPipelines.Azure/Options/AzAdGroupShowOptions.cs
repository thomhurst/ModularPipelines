using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "show")]
public record AzAdGroupShowOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions
{
}