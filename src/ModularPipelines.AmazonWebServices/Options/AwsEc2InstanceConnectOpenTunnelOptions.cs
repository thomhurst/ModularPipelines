using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2-instance-connect", "open-tunnel")]
public record AwsEc2InstanceConnectOpenTunnelOptions : AwsOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--instance-connect-endpoint-id")]
    public string? InstanceConnectEndpointId { get; set; }

    [CommandSwitch("--instance-connect-endpoint-dns-name")]
    public string? InstanceConnectEndpointDnsName { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--remote-port")]
    public int? RemotePort { get; set; }

    [CommandSwitch("--local-port")]
    public int? LocalPort { get; set; }

    [CommandSwitch("--max-tunnel-duration")]
    public int? MaxTunnelDuration { get; set; }

    [CommandSwitch("--max-websocket-connections")]
    public int? MaxWebsocketConnections { get; set; }
}