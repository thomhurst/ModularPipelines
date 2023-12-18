using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "private-link", "list")]
public record AzNetworkApplicationGatewayPrivateLinkListOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;