using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "resource", "query-region-info")]
public record AzNetappfilesResourceQueryRegionInfoOptions(
[property: CliOption("--location")] string Location
) : AzOptions;