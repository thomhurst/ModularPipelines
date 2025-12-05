using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "private-link", "ip-config", "remove")]
public record AzNetworkApplicationGatewayPrivateLinkIpConfigRemoveOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--private-link")] string PrivateLink,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}