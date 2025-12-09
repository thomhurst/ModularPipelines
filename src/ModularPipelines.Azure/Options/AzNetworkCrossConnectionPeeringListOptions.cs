using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "cross-connection", "peering", "list")]
public record AzNetworkCrossConnectionPeeringListOptions(
[property: CliOption("--cross-connection-name")] string CrossConnectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;