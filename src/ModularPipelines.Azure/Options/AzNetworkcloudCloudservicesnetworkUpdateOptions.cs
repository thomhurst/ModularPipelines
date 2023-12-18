using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cloudservicesnetwork", "update")]
public record AzNetworkcloudCloudservicesnetworkUpdateOptions : AzOptions
{
    [CommandSwitch("--additional-egress-endpoints")]
    public string? AdditionalEgressEndpoints { get; set; }

    [CommandSwitch("--cloud-services-network-name")]
    public string? CloudServicesNetworkName { get; set; }

    [BooleanCommandSwitch("--enable-default-egress-endpoints")]
    public bool? EnableDefaultEgressEndpoints { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

