using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "forwarding-rules", "set-target")]
public record GcloudComputeForwardingRulesSetTargetOptions(
[property: CliArgument] string Name,
[property: CliOption("--backend-service")] string BackendService,
[property: CliOption("--target-grpc-proxy")] string TargetGrpcProxy,
[property: CliOption("--target-http-proxy")] string TargetHttpProxy,
[property: CliOption("--target-https-proxy")] string TargetHttpsProxy,
[property: CliOption("--target-instance")] string TargetInstance,
[property: CliOption("--target-pool")] string TargetPool,
[property: CliOption("--target-ssl-proxy")] string TargetSslProxy,
[property: CliOption("--target-tcp-proxy")] string TargetTcpProxy,
[property: CliOption("--target-vpn-gateway")] string TargetVpnGateway
) : GcloudOptions
{
    [CliOption("--load-balancing-scheme")]
    public string? LoadBalancingScheme { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-region")]
    public string? SubnetRegion { get; set; }

    [CliOption("--target-instance-zone")]
    public string? TargetInstanceZone { get; set; }

    [CliOption("--target-pool-region")]
    public string? TargetPoolRegion { get; set; }

    [CliOption("--target-vpn-gateway-region")]
    public string? TargetVpnGatewayRegion { get; set; }

    [CliOption("--backend-service-region")]
    public string? BackendServiceRegion { get; set; }

    [CliFlag("--global-backend-service")]
    public bool? GlobalBackendService { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--global-target-http-proxy")]
    public bool? GlobalTargetHttpProxy { get; set; }

    [CliOption("--target-http-proxy-region")]
    public string? TargetHttpProxyRegion { get; set; }

    [CliFlag("--global-target-https-proxy")]
    public bool? GlobalTargetHttpsProxy { get; set; }

    [CliOption("--target-https-proxy-region")]
    public string? TargetHttpsProxyRegion { get; set; }

    [CliFlag("--global-target-tcp-proxy")]
    public bool? GlobalTargetTcpProxy { get; set; }

    [CliOption("--target-tcp-proxy-region")]
    public string? TargetTcpProxyRegion { get; set; }
}