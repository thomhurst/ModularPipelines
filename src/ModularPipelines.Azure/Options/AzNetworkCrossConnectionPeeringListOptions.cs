using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-connection", "peering", "list")]
public record AzNetworkCrossConnectionPeeringListOptions(
[property: CommandSwitch("--cross-connection-name")] string CrossConnectionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;