using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vnet-gateway", "nat-rule", "add")]
public record AzNetworkVnetGatewayNatRuleAddOptions(
[property: CommandSwitch("--external-mappings")] string ExternalMappings,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--internal-mappings")] string InternalMappings,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ip-config-id")]
    public string? IpConfigId { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

