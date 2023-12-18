using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "location", "show")]
public record AzSecurityLocationShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}