using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "set")]
public record AzAccountSetOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}