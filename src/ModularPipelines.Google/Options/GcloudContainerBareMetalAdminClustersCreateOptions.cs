using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "admin-clusters", "create")]
public record GcloudContainerBareMetalAdminClustersCreateOptions : GcloudOptions
{
    public GcloudContainerBareMetalAdminClustersCreateOptions(
        string adminCluster,
        string location,
        string version,
        string controlPlaneLoadBalancerPort,
        string controlPlaneVip,
        bool enableManualLb,
        IEnumerable<KeyValue> apiServerArgs,
        string[] controlPlaneNodeConfigs,
        IEnumerable<KeyValue> controlPlaneNodeLabels,
        IEnumerable<KeyValue> controlPlaneNodeTaints,
        string[] islandModePodAddressCidrBlocks,
        string[] islandModeServiceAddressCidrBlocks,
        string lvpNodeMountsConfigPath,
        string lvpNodeMountsConfigStorageClass,
        string sharedPathPvCount,
        string lvpSharePath,
        string lvpShareStorageClass
    )
    {
        AdminCluster = adminCluster;
        Location = location;
        Version = version;
        ControlPlaneLoadBalancerPort = controlPlaneLoadBalancerPort;
        ControlPlaneVip = controlPlaneVip;
        EnableManualLb = enableManualLb;
        ApiServerArgs = apiServerArgs;
        ControlPlaneNodeConfigs = controlPlaneNodeConfigs;
        ControlPlaneNodeLabels = controlPlaneNodeLabels;
        ControlPlaneNodeTaints = controlPlaneNodeTaints;
        IslandModePodAddressCidrBlocks = islandModePodAddressCidrBlocks;
        IslandModeServiceAddressCidrBlocks = islandModeServiceAddressCidrBlocks;
        LvpNodeMountsConfigPath = lvpNodeMountsConfigPath;
        LvpNodeMountsConfigStorageClass = lvpNodeMountsConfigStorageClass;
        SharedPathPvCount = sharedPathPvCount;
        LvpSharePath = lvpSharePath;
        LvpShareStorageClass = lvpShareStorageClass;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string AdminCluster { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--control-plane-load-balancer-port")]
    public string ControlPlaneLoadBalancerPort { get; set; }

    [CommandSwitch("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [BooleanCommandSwitch("--enable-manual-lb")]
    public bool EnableManualLb { get; set; }

    [CommandSwitch("--api-server-args")]
    public IEnumerable<KeyValue> ApiServerArgs { get; set; }

    [CommandSwitch("--control-plane-node-configs")]
    public string[] ControlPlaneNodeConfigs { get; set; }

    [CommandSwitch("--control-plane-node-labels")]
    public IEnumerable<KeyValue> ControlPlaneNodeLabels { get; set; }

    [CommandSwitch("--control-plane-node-taints")]
    public IEnumerable<KeyValue> ControlPlaneNodeTaints { get; set; }

    [CommandSwitch("--island-mode-pod-address-cidr-blocks")]
    public string[] IslandModePodAddressCidrBlocks { get; set; }

    [CommandSwitch("--island-mode-service-address-cidr-blocks")]
    public string[] IslandModeServiceAddressCidrBlocks { get; set; }

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

    [CommandSwitch("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--no-proxy")]
    public string[]? NoProxy { get; set; }
}
