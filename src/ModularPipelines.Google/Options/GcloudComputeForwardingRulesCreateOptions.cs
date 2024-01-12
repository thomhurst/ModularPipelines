using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "forwarding-rules", "create")]
public record GcloudComputeForwardingRulesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--backend-service")] string BackendService,
[property: CommandSwitch("--target-google-apis-bundle")] string TargetGoogleApisBundle,
[property: CommandSwitch("--target-grpc-proxy")] string TargetGrpcProxy,
[property: CommandSwitch("--target-http-proxy")] string TargetHttpProxy,
[property: CommandSwitch("--target-https-proxy")] string TargetHttpsProxy,
[property: CommandSwitch("--target-instance")] string TargetInstance,
[property: CommandSwitch("--target-pool")] string TargetPool,
[property: CommandSwitch("--target-service-attachment")] string TargetServiceAttachment,
[property: CommandSwitch("--target-ssl-proxy")] string TargetSslProxy,
[property: CommandSwitch("--target-tcp-proxy")] string TargetTcpProxy,
[property: CommandSwitch("--target-vpn-gateway")] string TargetVpnGateway
) : GcloudOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [BooleanCommandSwitch("--allow-global-access")]
    public bool? AllowGlobalAccess { get; set; }

    [BooleanCommandSwitch("--allow-psc-global-access")]
    public bool? AllowPscGlobalAccess { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable-automate-dns-zone")]
    public bool? DisableAutomateDnsZone { get; set; }

    [CommandSwitch("--ip-protocol")]
    public string? IpProtocol { get; set; }

    [CommandSwitch("--ip-version")]
    public string? IpVersion { get; set; }

    [BooleanCommandSwitch("--is-mirroring-collector")]
    public bool? IsMirroringCollector { get; set; }

    [CommandSwitch("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-tier")]
    public string? NetworkTier { get; set; }

    [CommandSwitch("--service-directory-registration")]
    public string? ServiceDirectoryRegistration { get; set; }

    [CommandSwitch("--service-label")]
    public string? ServiceLabel { get; set; }

    [CommandSwitch("--source-ip-ranges")]
    public string[]? SourceIpRanges { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CommandSwitch("--target-instance-zone")]
    public string? TargetInstanceZone { get; set; }

    [CommandSwitch("--target-pool-region")]
    public string? TargetPoolRegion { get; set; }

    [CommandSwitch("--target-service-attachment-region")]
    public string? TargetServiceAttachmentRegion { get; set; }

    [CommandSwitch("--target-vpn-gateway-region")]
    public string? TargetVpnGatewayRegion { get; set; }

    [CommandSwitch("--address-region")]
    public string? AddressRegion { get; set; }

    [BooleanCommandSwitch("--global-address")]
    public bool? GlobalAddress { get; set; }

    [CommandSwitch("--backend-service-region")]
    public string? BackendServiceRegion { get; set; }

    [BooleanCommandSwitch("--global-backend-service")]
    public bool? GlobalBackendService { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--global-target-http-proxy")]
    public bool? GlobalTargetHttpProxy { get; set; }

    [CommandSwitch("--target-http-proxy-region")]
    public string? TargetHttpProxyRegion { get; set; }

    [BooleanCommandSwitch("--global-target-https-proxy")]
    public bool? GlobalTargetHttpsProxy { get; set; }

    [CommandSwitch("--target-https-proxy-region")]
    public string? TargetHttpsProxyRegion { get; set; }

    [BooleanCommandSwitch("--global-target-tcp-proxy")]
    public bool? GlobalTargetTcpProxy { get; set; }

    [CommandSwitch("--target-tcp-proxy-region")]
    public string? TargetTcpProxyRegion { get; set; }

    [CommandSwitch("--port-range")]
    public string[]? PortRange { get; set; }

    [CommandSwitch("--ports")]
    public string[]? Ports { get; set; }
}