using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "routeserver", "create")]
public record AzNetworkRouteserverCreateOptions(
[property: CommandSwitch("--hosted-subnet")] string HostedSubnet,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--public-ip-address")] string PublicIpAddress,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--hub-routing-preference")]
    public string? HubRoutingPreference { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}