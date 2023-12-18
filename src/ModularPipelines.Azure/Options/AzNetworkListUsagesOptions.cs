using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "list-usages")]
public record AzNetworkListUsagesOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

