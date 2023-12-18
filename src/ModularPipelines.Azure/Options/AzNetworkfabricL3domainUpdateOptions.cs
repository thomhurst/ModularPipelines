using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "l3domain", "update")]
public record AzNetworkfabricL3domainUpdateOptions : AzOptions
{
    [CommandSwitch("--aggregate-route-configuration")]
    public string? AggregateRouteConfiguration { get; set; }

    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--connected-subnet-route-policy")]
    public string? ConnectedSubnetRoutePolicy { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--redistribute-connected-subnets")]
    public bool? RedistributeConnectedSubnets { get; set; }

    [BooleanCommandSwitch("--redistribute-static-routes")]
    public bool? RedistributeStaticRoutes { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-name")]
    public string? ResourceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}