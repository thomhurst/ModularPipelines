using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "identity", "assign")]
public record AzNetworkApplicationGatewayIdentityAssignOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--identity")] string Identity,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}