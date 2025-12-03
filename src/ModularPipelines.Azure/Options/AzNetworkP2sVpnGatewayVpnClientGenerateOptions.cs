using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "p2s-vpn-gateway", "vpn-client", "generate")]
public record AzNetworkP2sVpnGatewayVpnClientGenerateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authentication-method")]
    public string? AuthenticationMethod { get; set; }
}