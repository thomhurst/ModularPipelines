using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "custom-ip", "prefix", "create")]
public record AzNetworkCustomIpPrefixCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--authorization-message")]
    public string? AuthorizationMessage { get; set; }

    [CommandSwitch("--cidr")]
    public string? Cidr { get; set; }

    [CommandSwitch("--cip-prefix-parent")]
    public string? CipPrefixParent { get; set; }

    [BooleanCommandSwitch("--express-route-advertise")]
    public bool? ExpressRouteAdvertise { get; set; }

    [CommandSwitch("--geo")]
    public string? Geo { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--signed-message")]
    public string? SignedMessage { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}