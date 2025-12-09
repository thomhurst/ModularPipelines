using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "cloudservicesnetwork", "create")]
public record AzNetworkcloudCloudservicesnetworkCreateOptions(
[property: CliOption("--cloud-services-network-name")] string CloudServicesNetworkName,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--additional-egress-endpoints")]
    public string? AdditionalEgressEndpoints { get; set; }

    [CliFlag("--enable-default-egress-endpoints")]
    public bool? EnableDefaultEgressEndpoints { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}