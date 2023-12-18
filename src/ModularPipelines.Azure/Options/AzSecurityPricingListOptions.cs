using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "pricing", "list")]
public record AzSecurityPricingListOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}

