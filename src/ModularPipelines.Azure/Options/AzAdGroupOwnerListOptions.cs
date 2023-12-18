using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "owner", "list")]
public record AzAdGroupOwnerListOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions
{
}

