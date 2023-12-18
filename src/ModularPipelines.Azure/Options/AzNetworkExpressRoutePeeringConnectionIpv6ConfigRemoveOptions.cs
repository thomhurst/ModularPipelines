using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "connection", "ipv6-config", "remove")]
public record AzNetworkExpressRoutePeeringConnectionIpv6ConfigRemoveOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

