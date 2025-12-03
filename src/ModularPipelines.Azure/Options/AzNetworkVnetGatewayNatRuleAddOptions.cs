using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet-gateway", "nat-rule", "add")]
public record AzNetworkVnetGatewayNatRuleAddOptions(
[property: CliOption("--external-mappings")] string ExternalMappings,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--internal-mappings")] string InternalMappings,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ip-config-id")]
    public string? IpConfigId { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}