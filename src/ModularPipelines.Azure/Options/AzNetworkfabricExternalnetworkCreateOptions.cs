using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkfabric", "externalnetwork", "create")]
public record AzNetworkfabricExternalnetworkCreateOptions(
[property: CliOption("--l3-isolation-domain-name")] string L3IsolationDomainName,
[property: CliOption("--peering-option")] string PeeringOption,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--export-route-policy")]
    public string? ExportRoutePolicy { get; set; }

    [CliOption("--export-route-policy-id")]
    public string? ExportRoutePolicyId { get; set; }

    [CliOption("--import-route-policy")]
    public string? ImportRoutePolicy { get; set; }

    [CliOption("--import-route-policy-id")]
    public string? ImportRoutePolicyId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--option-a-properties")]
    public string? OptionAProperties { get; set; }

    [CliOption("--option-b-properties")]
    public string? OptionBProperties { get; set; }
}