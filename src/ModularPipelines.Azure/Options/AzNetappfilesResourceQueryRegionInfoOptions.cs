using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "resource", "query-region-info")]
public record AzNetappfilesResourceQueryRegionInfoOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;