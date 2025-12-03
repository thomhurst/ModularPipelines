using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "cross-connection", "peering", "list")]
public record AzNetworkCrossConnectionPeeringListOptions(
[property: CliOption("--cross-connection-name")] string CrossConnectionName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;