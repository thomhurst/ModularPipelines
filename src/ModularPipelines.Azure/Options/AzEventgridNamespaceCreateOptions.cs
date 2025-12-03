using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "create")]
public record AzEventgridNamespaceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--inbound-ip-rules")]
    public string? InboundIpRules { get; set; }

    [CliFlag("--is-zone-redundant")]
    public bool? IsZoneRedundant { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--minimum-tls-version-allowed")]
    public string? MinimumTlsVersionAllowed { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-endpoint-connections")]
    public string? PrivateEndpointConnections { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--topic-spaces-configuration")]
    public string? TopicSpacesConfiguration { get; set; }
}