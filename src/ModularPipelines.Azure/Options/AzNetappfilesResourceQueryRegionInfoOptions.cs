using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "resource", "query-region-info")]
public record AzNetappfilesResourceQueryRegionInfoOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

