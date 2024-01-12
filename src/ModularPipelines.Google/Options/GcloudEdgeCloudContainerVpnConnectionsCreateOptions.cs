using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "container", "vpn-connections", "create")]
public record GcloudEdgeCloudContainerVpnConnectionsCreateOptions(
[property: PositionalArgument] string VpnConnection,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--high-availability")]
    public bool? HighAvailability { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--nat-gateway-ip")]
    public string? NatGatewayIp { get; set; }

    [CommandSwitch("--router")]
    public string? Router { get; set; }

    [CommandSwitch("--vpc-project")]
    public string? VpcProject { get; set; }
}