using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "bgpconnection", "create")]
public record AzNetworkVhubBgpconnectionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub-name")] string VhubName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peer-asn")]
    public string? PeerAsn { get; set; }

    [CliOption("--peer-ip")]
    public string? PeerIp { get; set; }

    [CliOption("--vhub-conn")]
    public string? VhubConn { get; set; }
}