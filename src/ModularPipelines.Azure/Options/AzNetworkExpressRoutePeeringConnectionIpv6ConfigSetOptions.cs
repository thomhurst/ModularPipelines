using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "connection", "ipv6-config", "set")]
public record AzNetworkExpressRoutePeeringConnectionIpv6ConfigSetOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}