using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "admin-clusters", "update")]
public record GcloudContainerBareMetalAdminClustersUpdateOptions(
[property: PositionalArgument] string AdminCluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-application-logs")]
    public bool? EnableApplicationLogs { get; set; }

    [CommandSwitch("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [CommandSwitch("--login-user")]
    public string? LoginUser { get; set; }

    [CommandSwitch("--maintenance-address-cidr-blocks")]
    public string[]? MaintenanceAddressCidrBlocks { get; set; }

    [CommandSwitch("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--api-server-args")]
    public IEnumerable<KeyValue>? ApiServerArgs { get; set; }

    [CommandSwitch("--control-plane-node-configs")]
    public string[]? ControlPlaneNodeConfigs { get; set; }

    [CommandSwitch("--control-plane-node-labels")]
    public IEnumerable<KeyValue>? ControlPlaneNodeLabels { get; set; }

    [CommandSwitch("--control-plane-node-taints")]
    public IEnumerable<KeyValue>? ControlPlaneNodeTaints { get; set; }

    [CommandSwitch("--no-proxy")]
    public string[]? NoProxy { get; set; }

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }
}