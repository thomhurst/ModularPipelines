using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "clusters", "update")]
public record GcloudContainerBareMetalClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--admin-users")]
    public string? AdminUsers { get; set; }

    [BooleanCommandSwitch("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-application-logs")]
    public bool? EnableApplicationLogs { get; set; }

    [CommandSwitch("--login-user")]
    public string? LoginUser { get; set; }

    [CommandSwitch("--maintenance-address-cidr-blocks")]
    public string[]? MaintenanceAddressCidrBlocks { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--add-annotations")]
    public string[]? AddAnnotations { get; set; }

    [CommandSwitch("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CommandSwitch("--api-server-args")]
    public IEnumerable<KeyValue>? ApiServerArgs { get; set; }

    [CommandSwitch("--control-plane-node-configs")]
    public string[]? ControlPlaneNodeConfigs { get; set; }

    [CommandSwitch("--control-plane-node-labels")]
    public IEnumerable<KeyValue>? ControlPlaneNodeLabels { get; set; }

    [CommandSwitch("--control-plane-node-taints")]
    public IEnumerable<KeyValue>? ControlPlaneNodeTaints { get; set; }

    [CommandSwitch("--control-plane-registry-burst")]
    public string? ControlPlaneRegistryBurst { get; set; }

    [CommandSwitch("--control-plane-registry-pull-qps")]
    public string? ControlPlaneRegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-control-plane-serialize-image-pulls")]
    public bool? DisableControlPlaneSerializeImagePulls { get; set; }

    [BooleanCommandSwitch("--enable-control-plane-serialize-image-pulls")]
    public bool? EnableControlPlaneSerializeImagePulls { get; set; }

    [CommandSwitch("--bgp-address-pools")]
    public string[]? BgpAddressPools { get; set; }

    [CommandSwitch("--bgp-asn")]
    public string? BgpAsn { get; set; }

    [CommandSwitch("--bgp-peer-configs")]
    public string[]? BgpPeerConfigs { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-configs")]
    public string[]? BgpLoadBalancerNodeConfigs { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-labels")]
    public IEnumerable<KeyValue>? BgpLoadBalancerNodeLabels { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-taints")]
    public IEnumerable<KeyValue>? BgpLoadBalancerNodeTaints { get; set; }

    [CommandSwitch("--bgp-load-balancer-registry-burst")]
    public string? BgpLoadBalancerRegistryBurst { get; set; }

    [CommandSwitch("--bgp-load-balancer-registry-pull-qps")]
    public string? BgpLoadBalancerRegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-bgp-load-balancer-serialize-image-pulls")]
    public bool? DisableBgpLoadBalancerSerializeImagePulls { get; set; }

    [BooleanCommandSwitch("--enable-bgp-load-balancer-serialize-image-pulls")]
    public bool? EnableBgpLoadBalancerSerializeImagePulls { get; set; }

    [CommandSwitch("--metal-lb-address-pools")]
    public string[]? MetalLbAddressPools { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-configs")]
    public string[]? MetalLbLoadBalancerNodeConfigs { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-labels")]
    public IEnumerable<KeyValue>? MetalLbLoadBalancerNodeLabels { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-taints")]
    public IEnumerable<KeyValue>? MetalLbLoadBalancerNodeTaints { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-registry-burst")]
    public string? MetalLbLoadBalancerRegistryBurst { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-registry-pull-qps")]
    public string? MetalLbLoadBalancerRegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool? DisableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [BooleanCommandSwitch("--enable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool? EnableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [CommandSwitch("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [BooleanCommandSwitch("--disable-sr-iov-config")]
    public bool? DisableSrIovConfig { get; set; }

    [BooleanCommandSwitch("--enable-sr-iov-config")]
    public bool? EnableSrIovConfig { get; set; }
}