using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "clusters", "create")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Cluster { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliOption("--admin-cluster-membership")]
    public string AdminClusterMembership { get; set; }

    [CliOption("--admin-cluster-membership-location")]
    public string AdminClusterMembershipLocation { get; set; }

    [CliOption("--admin-cluster-membership-project")]
    public string AdminClusterMembershipProject { get; set; }

    [CliOption("--control-plane-load-balancer-port")]
    public string ControlPlaneLoadBalancerPort { get; set; }

    [CliOption("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [CliOption("--ingress-vip")]
    public string IngressVip { get; set; }

    [CliFlag("--enable-manual-lb")]
    public bool EnableManualLb { get; set; }

    [CliOption("--bgp-address-pools")]
    public string[] BgpAddressPools { get; set; }

    [CliOption("--bgp-asn")]
    public string BgpAsn { get; set; }

    [CliOption("--bgp-peer-configs")]
    public string[] BgpPeerConfigs { get; set; }

    [CliOption("--bgp-load-balancer-node-configs")]
    public string[] BgpLoadBalancerNodeConfigs { get; set; }

    [CliOption("--bgp-load-balancer-node-labels")]
    public IEnumerable<KeyValue> BgpLoadBalancerNodeLabels { get; set; }

    [CliOption("--bgp-load-balancer-node-taints")]
    public IEnumerable<KeyValue> BgpLoadBalancerNodeTaints { get; set; }

    [CliOption("--bgp-load-balancer-registry-burst")]
    public string BgpLoadBalancerRegistryBurst { get; set; }

    [CliOption("--bgp-load-balancer-registry-pull-qps")]
    public string BgpLoadBalancerRegistryPullQps { get; set; }

    [CliFlag("--disable-bgp-load-balancer-serialize-image-pulls")]
    public bool DisableBgpLoadBalancerSerializeImagePulls { get; set; }

    [CliOption("--metal-lb-address-pools")]
    public string[] MetalLbAddressPools { get; set; }

    [CliOption("--metal-lb-load-balancer-node-configs")]
    public string[] MetalLbLoadBalancerNodeConfigs { get; set; }

    [CliOption("--metal-lb-load-balancer-node-labels")]
    public IEnumerable<KeyValue> MetalLbLoadBalancerNodeLabels { get; set; }

    [CliOption("--metal-lb-load-balancer-node-taints")]
    public IEnumerable<KeyValue> MetalLbLoadBalancerNodeTaints { get; set; }

    [CliFlag("--disable-metal-lb-load-balancer-serialize-image-pulls")]
    public bool DisableMetalLbLoadBalancerSerializeImagePulls { get; set; }

    [CliOption("--metal-lb-load-balancer-registry-burst")]
    public string MetalLbLoadBalancerRegistryBurst { get; set; }

    [CliOption("--metal-lb-load-balancer-registry-pull-qps")]
    public string MetalLbLoadBalancerRegistryPullQps { get; set; }

    [CliOption("--api-server-args")]
    public IEnumerable<KeyValue> ApiServerArgs { get; set; }

    [CliOption("--control-plane-node-configs")]
    public string[] ControlPlaneNodeConfigs { get; set; }

    [CliOption("--control-plane-node-labels")]
    public IEnumerable<KeyValue> ControlPlaneNodeLabels { get; set; }

    [CliOption("--control-plane-node-taints")]
    public IEnumerable<KeyValue> ControlPlaneNodeTaints { get; set; }

    [CliOption("--control-plane-registry-burst")]
    public string ControlPlaneRegistryBurst { get; set; }

    [CliOption("--control-plane-registry-pull-qps")]
    public string ControlPlaneRegistryPullQps { get; set; }

    [CliFlag("--disable-control-plane-serialize-image-pulls")]
    public bool DisableControlPlaneSerializeImagePulls { get; set; }

    [CliOption("--lvp-node-mounts-config-path")]
    public string LvpNodeMountsConfigPath { get; set; }

    [CliOption("--lvp-node-mounts-config-storage-class")]
    public string LvpNodeMountsConfigStorageClass { get; set; }

    [CliOption("--shared-path-pv-count")]
    public string SharedPathPvCount { get; set; }

    [CliOption("--lvp-share-path")]
    public string LvpSharePath { get; set; }

    [CliOption("--lvp-share-storage-class")]
    public string LvpShareStorageClass { get; set; }

    [CliOption("--admin-users")]
    public string? AdminUsers { get; set; }

    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

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

    [CliOption("--container-runtime")]
    public string? ContainerRuntime { get; set; }

    [CliOption("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [CliFlag("--enable-advanced-networking")]
    public bool? EnableAdvancedNetworking { get; set; }

    [CliFlag("--enable-multi-nic-config")]
    public bool? EnableMultiNicConfig { get; set; }

    [CliFlag("--enable-sr-iov-config")]
    public bool? EnableSrIovConfig { get; set; }

    [CliOption("--island-mode-pod-address-cidr-blocks")]
    public string[]? IslandModePodAddressCidrBlocks { get; set; }

    [CliOption("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--no-proxy")]
    public string[]? NoProxy { get; set; }
}
