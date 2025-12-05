using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "cloudservicesnetwork", "update")]
public record AzNetworkcloudCloudservicesnetworkUpdateOptions : AzOptions
{
    [CliOption("--additional-egress-endpoints")]
    public string? AdditionalEgressEndpoints { get; set; }

    [CliOption("--cloud-services-network-name")]
    public string? CloudServicesNetworkName { get; set; }

    [CliFlag("--enable-default-egress-endpoints")]
    public bool? EnableDefaultEgressEndpoints { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}