using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "l3domain", "create")]
public record AzNetworkfabricL3domainCreateOptions(
[property: CommandSwitch("--nf-id")] string NfId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--aggregate-route-configuration")]
    public string? AggregateRouteConfiguration { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--connected-subnet-route-policy")]
    public string? ConnectedSubnetRoutePolicy { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--redistribute-connected-subnets")]
    public bool? RedistributeConnectedSubnets { get; set; }

    [BooleanCommandSwitch("--redistribute-static-routes")]
    public bool? RedistributeStaticRoutes { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

