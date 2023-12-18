using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "connection", "create")]
public record AzNetworkExpressRoutePeeringConnectionCreateOptions(
[property: CommandSwitch("--circuit-name")] string CircuitName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CommandSwitch("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peer-circuit")]
    public string? PeerCircuit { get; set; }

    [CommandSwitch("--source-circuit")]
    public string? SourceCircuit { get; set; }
}