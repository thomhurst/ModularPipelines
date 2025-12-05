using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "l2network", "create")]
public record AzNetworkcloudL2networkCreateOptions(
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--l2-isolation-domain-id")] string L2IsolationDomainId,
[property: CliOption("--l2-network-name")] string L2NetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
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