using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "externalnetwork", "update")]
public record AzNetworkfabricExternalnetworkUpdateOptions : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CliOption("--export-route-policy-id")]
    public string? ExportRoutePolicyId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CliOption("--l3-isolation-domain-name")]
    public string? L3IsolationDomainName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--option-a-properties")]
    public string? OptionAProperties { get; set; }

    [CliOption("--option-b-properties")]
    public string? OptionBProperties { get; set; }

    [CliOption("--peering-option")]
    public string? PeeringOption { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}