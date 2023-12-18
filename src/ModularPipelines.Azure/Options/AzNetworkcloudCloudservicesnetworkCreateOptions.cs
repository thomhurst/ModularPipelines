using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cloudservicesnetwork", "create")]
public record AzNetworkcloudCloudservicesnetworkCreateOptions(
[property: CommandSwitch("--cloud-services-network-name")] string CloudServicesNetworkName,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--additional-egress-endpoints")]
    public string? AdditionalEgressEndpoints { get; set; }

    [BooleanCommandSwitch("--enable-default-egress-endpoints")]
    public bool? EnableDefaultEgressEndpoints { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}