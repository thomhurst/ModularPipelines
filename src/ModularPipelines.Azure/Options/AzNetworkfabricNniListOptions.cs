using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "nni", "list")]
public record AzNetworkfabricNniListOptions(
[property: CliOption("--fabric")] string Fabric,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;