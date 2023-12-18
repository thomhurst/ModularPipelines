using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "namespace", "create")]
public record AzEventgridNamespaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--inbound-ip-rules")]
    public string? InboundIpRules { get; set; }

    [BooleanCommandSwitch("--is-zone-redundant")]
    public bool? IsZoneRedundant { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--minimum-tls-version-allowed")]
    public string? MinimumTlsVersionAllowed { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-endpoint-connections")]
    public string? PrivateEndpointConnections { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--topic-spaces-configuration")]
    public string? TopicSpacesConfiguration { get; set; }
}