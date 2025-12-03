using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "update")]
public record GcloudContainerBareMetalClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--admin-users")]
    public string? AdminUsers { get; set; }

    [CliFlag("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-application-logs")]
    public bool? EnableApplicationLogs { get; set; }

    [CliOption("--login-user")]
    public string? LoginUser { get; set; }

    [CliOption("--maintenance-address-cidr-blocks")]
    public string[]? MaintenanceAddressCidrBlocks { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--add-annotations")]
    public string[]? AddAnnotations { get; set; }

    [CliOption("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CliOption("--api-server-args")]
    public IEnumerable<KeyValue>? ApiServerArgs { get; set; }

    [CliOption("--control-plane-node-configs")]
    public string[]? ControlPlaneNodeConfigs { get; set; }

    [CliOption("--control-plane-node-labels")]
    public IEnumerable<KeyValue>? ControlPlaneNodeLabels { get; set; }

    [CliOption("--control-plane-node-taints")]
    public IEnumerable<KeyValue>? ControlPlaneNodeTaints { get; set; }

    [CliOption("--control-plane-registry-burst")]
    public string? ControlPlaneRegistryBurst { get; set; }

    [CliOption("--control-plane-registry-pull-qps")]
    public string? ControlPlaneRegistryPullQps { get; set; }

    [CliFlag("--disable-control-plane-serialize-image-pulls")]
    public bool? DisableControlPlaneSerializeImagePulls { get; set; }

    [CliFlag("--enable-control-plane-serialize-image-pulls")]
    public bool? EnableControlPlaneSerializeImagePulls { get; set; }

    [CliOption("--bgp-address-pools")]
    public string[]? BgpAddressPools { get; set; }

    [CliOption("--bgp-asn")]
    public string? BgpAsn { get; set; }

    [CliOption("--bgp-peer-configs")]
    public string[]? BgpPeerConfigs { get; set; }

    [CliOption("--bgp-load-balancer-node-configs")]
    public string[]? BgpLoadBalancerNodeConfigs { get; set; }

    [CliOption("--bgp-load-balancer-node-labels")]
    public IEnumerable<KeyValue>? BgpLoadBalancerNodeLabels { get; set; }

    [CliOption("--bgp-load-balancer-node-taints")]
    public IEnumerable<KeyValue>? BgpLoadBalancerNodeTaints { get; set; }

    [CliOption("--bgp-load-balancer-registry-burst")]
    public string? BgpLoadBalancerRegistryBurst { get; set; }

    [CliOption("--bgp-load-balancer-registry-pull-qps")]
    public string? BgpLoadBalancerRegistryPullQps { get; set; }

    [CliFlag("--disable-bgp-load-balancer-serialize-image-pulls")]
    public bool? DisableBgpLoadBalancerSerializeImagePulls { get; set; }

    [CliFlag("--enable-bgp-load-balancer-serialize-image-pulls")]
    public bool? EnableBgpLoadBalancerSerializeImagePulls { get; set; }

    [CliOption("--metal-lb-address-pools")]
    public string[]? MetalLbAddressPools { get; set; }

    [CliOption("--metal-lb-load-balancer-node-configs")]
    public string[]? MetalLbLoadBalancerNodeConfigs { get; set; }

    [CliOption("--metal-lb-load-balancer-node-labels")]
    public IEnumerable<KeyValue>? MetalLbLoadBalancerNodeLabels { get; set; }

    [CliOption("--metal-lb-load-balancer-node-taints")]
    public IEnumerable<KeyValue>? MetalLbLoadBalancerNodeTaints { get; set; }

    [CliOption("--metal-lb-load-balancer-registry-burst")]
    public string? MetalLbLoadBalancerRegistryBurst { get; set; }

    [CliOption("--metal-lb-load-balancer-registry-pull-qps")]
    public string? MetalLbLoadBalancerRegistryPullQps { get; set; }

    [CliFlag("--disable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool? DisableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [CliFlag("--enable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool? EnableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [CliOption("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [CliFlag("--disable-sr-iov-config")]
    public bool? DisableSrIovConfig { get; set; }

    [CliFlag("--enable-sr-iov-config")]
    public bool? EnableSrIovConfig { get; set; }
}