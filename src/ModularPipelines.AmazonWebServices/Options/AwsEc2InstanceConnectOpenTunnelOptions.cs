using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2-instance-connect", "open-tunnel")]
public record AwsEc2InstanceConnectOpenTunnelOptions : AwsOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--instance-connect-endpoint-id")]
    public string? InstanceConnectEndpointId { get; set; }

    [CliOption("--instance-connect-endpoint-dns-name")]
    public string? InstanceConnectEndpointDnsName { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--remote-port")]
    public int? RemotePort { get; set; }

    [CliOption("--local-port")]
    public int? LocalPort { get; set; }

    [CliOption("--max-tunnel-duration")]
    public int? MaxTunnelDuration { get; set; }

    [CliOption("--max-websocket-connections")]
    public int? MaxWebsocketConnections { get; set; }
}