using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "contact", "list")]
public record AzSecurityContactListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

