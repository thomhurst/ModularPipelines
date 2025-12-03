using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "routeserver", "create")]
public record AzNetworkRouteserverCreateOptions(
[property: CliOption("--hosted-subnet")] string HostedSubnet,
[property: CliOption("--name")] string Name,
[property: CliOption("--public-ip-address")] string PublicIpAddress,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--hub-routing-preference")]
    public string? HubRoutingPreference { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}