using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "clusters", "create")]
public record GcloudContainerBareMetalClustersCreateOptions : GcloudOptions
{
    public GcloudContainerBareMetalClustersCreateOptions(
        string cluster,
        string location,
        string version,
        string adminClusterMembership,
        string adminClusterMembershipLocation,
        string adminClusterMembershipProject,
        string controlPlaneLoadBalancerPort,
        string controlPlaneVip,
        string ingressVip,
        bool enableManualLb,
        string[] bgpAddressPools,
        string bgpAsn,
        string[] bgpPeerConfigs,
        string[] bgpLoadBalancerNodeConfigs,
        IEnumerable<KeyValue> bgpLoadBalancerNodeLabels,
        IEnumerable<KeyValue> bgpLoadBalancerNodeTaints,
        string bgpLoadBalancerRegistryBurst,
        string bgpLoadBalancerRegistryPullQps,
        bool disableBgpLoadBalancerSerializeImagePulls,
        string[] metalLbAddressPools,
        string[] metalLbLoadBalancerNodeConfigs,
        IEnumerable<KeyValue> metalLbLoadBalancerNodeLabels,
        IEnumerable<KeyValue> metalLbLoadBalancerNodeTaints,
        bool disableMetalLbLoadBalancerSerializeImagePulls,
        string metalLbLoadBalancerRegistryBurst,
        string metalLbLoadBalancerRegistryPullQps,
        IEnumerable<KeyValue> apiServerArgs,
        string[] controlPlaneNodeConfigs,
        IEnumerable<KeyValue> controlPlaneNodeLabels,
        IEnumerable<KeyValue> controlPlaneNodeTaints,
        string controlPlaneRegistryBurst,
        string controlPlaneRegistryPullQps,
        bool disableControlPlaneSerializeImagePulls,
        string lvpNodeMountsConfigPath,
        string lvpNodeMountsConfigStorageClass,
        string sharedPathPvCount,
        string lvpSharePath,
        string lvpShareStorageClass
    )
    {
        Cluster = cluster;
        Location = location;
        Version = version;
        AdminClusterMembership = adminClusterMembership;
        AdminClusterMembershipLocation = adminClusterMembershipLocation;
        AdminClusterMembershipProject = adminClusterMembershipProject;
        ControlPlaneLoadBalancerPort = controlPlaneLoadBalancerPort;
        ControlPlaneVip = controlPlaneVip;
        IngressVip = ingressVip;
        EnableManualLb = enableManualLb;
        BgpAddressPools = bgpAddressPools;
        BgpAsn = bgpAsn;
        BgpPeerConfigs = bgpPeerConfigs;
        BgpLoadBalancerNodeConfigs = bgpLoadBalancerNodeConfigs;
        BgpLoadBalancerNodeLabels = bgpLoadBalancerNodeLabels;
        BgpLoadBalancerNodeTaints = bgpLoadBalancerNodeTaints;
        BgpLoadBalancerRegistryBurst = bgpLoadBalancerRegistryBurst;
        BgpLoadBalancerRegistryPullQps = bgpLoadBalancerRegistryPullQps;
        DisableBgpLoadBalancerSerializeImagePulls = disableBgpLoadBalancerSerializeImagePulls;
        MetalLbAddressPools = metalLbAddressPools;
        MetalLbLoadBalancerNodeConfigs = metalLbLoadBalancerNodeConfigs;
        MetalLbLoadBalancerNodeLabels = metalLbLoadBalancerNodeLabels;
        MetalLbLoadBalancerNodeTaints = metalLbLoadBalancerNodeTaints;
        DisableMetalLbLoadBalancerSerializeImagePulls = disableMetalLbLoadBalancerSerializeImagePulls;
        MetalLbLoadBalancerRegistryBurst = metalLbLoadBalancerRegistryBurst;
        MetalLbLoadBalancerRegistryPullQps = metalLbLoadBalancerRegistryPullQps;
        ApiServerArgs = apiServerArgs;
        ControlPlaneNodeConfigs = controlPlaneNodeConfigs;
        ControlPlaneNodeLabels = controlPlaneNodeLabels;
        ControlPlaneNodeTaints = controlPlaneNodeTaints;
        ControlPlaneRegistryBurst = controlPlaneRegistryBurst;
        ControlPlaneRegistryPullQps = controlPlaneRegistryPullQps;
        DisableControlPlaneSerializeImagePulls = disableControlPlaneSerializeImagePulls;
        LvpNodeMountsConfigPath = lvpNodeMountsConfigPath;
        LvpNodeMountsConfigStorageClass = lvpNodeMountsConfigStorageClass;
        SharedPathPvCount = sharedPathPvCount;
        LvpSharePath = lvpSharePath;
        LvpShareStorageClass = lvpShareStorageClass;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Cluster { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--admin-cluster-membership")]
    public string AdminClusterMembership { get; set; }

    [CommandSwitch("--admin-cluster-membership-location")]
    public string AdminClusterMembershipLocation { get; set; }

    [CommandSwitch("--admin-cluster-membership-project")]
    public string AdminClusterMembershipProject { get; set; }

    [CommandSwitch("--control-plane-load-balancer-port")]
    public string ControlPlaneLoadBalancerPort { get; set; }

    [CommandSwitch("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [CommandSwitch("--ingress-vip")]
    public string IngressVip { get; set; }

    [BooleanCommandSwitch("--enable-manual-lb")]
    public bool EnableManualLb { get; set; }

    [CommandSwitch("--bgp-address-pools")]
    public string[] BgpAddressPools { get; set; }

    [CommandSwitch("--bgp-asn")]
    public string BgpAsn { get; set; }

    [CommandSwitch("--bgp-peer-configs")]
    public string[] BgpPeerConfigs { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-configs")]
    public string[] BgpLoadBalancerNodeConfigs { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-labels")]
    public IEnumerable<KeyValue> BgpLoadBalancerNodeLabels { get; set; }

    [CommandSwitch("--bgp-load-balancer-node-taints")]
    public IEnumerable<KeyValue> BgpLoadBalancerNodeTaints { get; set; }

    [CommandSwitch("--bgp-load-balancer-registry-burst")]
    public string BgpLoadBalancerRegistryBurst { get; set; }

    [CommandSwitch("--bgp-load-balancer-registry-pull-qps")]
    public string BgpLoadBalancerRegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-bgp-load-balancer-serialize-image-pulls")]
    public bool DisableBgpLoadBalancerSerializeImagePulls { get; set; }

    [CommandSwitch("--metal-lb-address-pools")]
    public string[] MetalLbAddressPools { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-configs")]
    public string[] MetalLbLoadBalancerNodeConfigs { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-labels")]
    public IEnumerable<KeyValue> MetalLbLoadBalancerNodeLabels { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-node-taints")]
    public IEnumerable<KeyValue> MetalLbLoadBalancerNodeTaints { get; set; }

    [BooleanCommandSwitch("--disable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool DisableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-registry-burst")]
    public string MetalLbLoadBalancerRegistryBurst { get; set; }

    [CommandSwitch("--metal-lb-load-balancer-registry-pull-qps")]
    public string MetalLbLoadBalancerRegistryPullQps { get; set; }

    [CommandSwitch("--api-server-args")]
    public IEnumerable<KeyValue> ApiServerArgs { get; set; }

    [CommandSwitch("--control-plane-node-configs")]
    public string[] ControlPlaneNodeConfigs { get; set; }

    [CommandSwitch("--control-plane-node-labels")]
    public IEnumerable<KeyValue> ControlPlaneNodeLabels { get; set; }

    [CommandSwitch("--control-plane-node-taints")]
    public IEnumerable<KeyValue> ControlPlaneNodeTaints { get; set; }

    [CommandSwitch("--control-plane-registry-burst")]
    public string ControlPlaneRegistryBurst { get; set; }

    [CommandSwitch("--control-plane-registry-pull-qps")]
    public string ControlPlaneRegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-control-plane-serialize-image-pulls")]
    public bool DisableControlPlaneSerializeImagePulls { get; set; }

    [CommandSwitch("--lvp-node-mounts-config-path")]
    public string LvpNodeMountsConfigPath { get; set; }

    [CommandSwitch("--lvp-node-mounts-config-storage-class")]
    public string LvpNodeMountsConfigStorageClass { get; set; }

    [CommandSwitch("--shared-path-pv-count")]
    public string SharedPathPvCount { get; set; }

    [CommandSwitch("--lvp-share-path")]
    public string LvpSharePath { get; set; }

    [CommandSwitch("--lvp-share-storage-class")]
    public string LvpShareStorageClass { get; set; }

    [CommandSwitch("--admin-users")]
    public string? AdminUsers { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

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

    [CommandSwitch("--container-runtime")]
    public string? ContainerRuntime { get; set; }

    [CommandSwitch("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [BooleanCommandSwitch("--enable-advanced-networking")]
    public bool? EnableAdvancedNetworking { get; set; }

    [BooleanCommandSwitch("--enable-multi-nic-config")]
    public bool? EnableMultiNicConfig { get; set; }

    [BooleanCommandSwitch("--enable-sr-iov-config")]
    public bool? EnableSrIovConfig { get; set; }

    [CommandSwitch("--island-mode-pod-address-cidr-blocks")]
    public string[]? IslandModePodAddressCidrBlocks { get; set; }

    [CommandSwitch("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--no-proxy")]
    public string[]? NoProxy { get; set; }
}
