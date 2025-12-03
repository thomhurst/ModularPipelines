using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "list-usages")]
public record AzNetworkListUsagesOptions(
[property: CliOption("--location")] string Location
) : AzOptions;