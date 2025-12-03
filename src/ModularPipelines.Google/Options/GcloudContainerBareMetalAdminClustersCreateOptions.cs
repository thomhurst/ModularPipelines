using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "create")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string AdminCluster { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliOption("--control-plane-load-balancer-port")]
    public string ControlPlaneLoadBalancerPort { get; set; }

    [CliOption("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [CliFlag("--enable-manual-lb")]
    public bool EnableManualLb { get; set; }

    [CliOption("--api-server-args")]
    public IEnumerable<KeyValue> ApiServerArgs { get; set; }

    [CliOption("--control-plane-node-configs")]
    public string[] ControlPlaneNodeConfigs { get; set; }

    [CliOption("--control-plane-node-labels")]
    public IEnumerable<KeyValue> ControlPlaneNodeLabels { get; set; }

    [CliOption("--control-plane-node-taints")]
    public IEnumerable<KeyValue> ControlPlaneNodeTaints { get; set; }

    [CliOption("--island-mode-pod-address-cidr-blocks")]
    public string[] IslandModePodAddressCidrBlocks { get; set; }

    [CliOption("--island-mode-service-address-cidr-blocks")]
    public string[] IslandModeServiceAddressCidrBlocks { get; set; }

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

    [CliOption("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--no-proxy")]
    public string[]? NoProxy { get; set; }
}
