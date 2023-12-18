using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "location", "list")]
public record AzSecurityLocationListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}