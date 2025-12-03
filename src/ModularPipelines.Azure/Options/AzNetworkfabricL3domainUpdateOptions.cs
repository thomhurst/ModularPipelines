using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "l3domain", "update")]
public record AzNetworkfabricL3domainUpdateOptions : AzOptions
{
    [CliOption("--aggregate-route-configuration")]
    public string? AggregateRouteConfiguration { get; set; }

    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--connected-subnet-route-policy")]
    public string? ConnectedSubnetRoutePolicy { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--redistribute-connected-subnets")]
    public bool? RedistributeConnectedSubnets { get; set; }

    [CliFlag("--redistribute-static-routes")]
    public bool? RedistributeStaticRoutes { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}