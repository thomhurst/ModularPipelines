using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "vpn-connections", "create")]
public record GcloudEdgeCloudContainerVpnConnectionsCreateOptions(
[property: CliArgument] string VpnConnection,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--high-availability")]
    public bool? HighAvailability { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--nat-gateway-ip")]
    public string? NatGatewayIp { get; set; }

    [CliOption("--router")]
    public string? Router { get; set; }

    [CliOption("--vpc-project")]
    public string? VpcProject { get; set; }
}