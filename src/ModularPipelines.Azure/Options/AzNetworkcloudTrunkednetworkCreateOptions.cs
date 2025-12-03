using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "trunkednetwork", "create")]
public record AzNetworkcloudTrunkednetworkCreateOptions(
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--isolation-domain-ids")] string IsolationDomainIds,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vlans")] string Vlans
) : AzOptions
{
    [CliOption("--interface-name")]
    public string? InterfaceName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}