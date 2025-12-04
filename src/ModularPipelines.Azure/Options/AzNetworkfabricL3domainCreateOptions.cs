using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "l3domain", "create")]
public record AzNetworkfabricL3domainCreateOptions(
[property: CliOption("--nf-id")] string NfId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--aggregate-route-configuration")]
    public string? AggregateRouteConfiguration { get; set; }

    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--connected-subnet-route-policy")]
    public string? ConnectedSubnetRoutePolicy { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--redistribute-connected-subnets")]
    public bool? RedistributeConnectedSubnets { get; set; }

    [CliFlag("--redistribute-static-routes")]
    public bool? RedistributeStaticRoutes { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}