using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "interface", "list")]
public record AzNetworkfabricInterfaceListOptions(
[property: CliOption("--device")] string Device,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;