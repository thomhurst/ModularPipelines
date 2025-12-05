using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "admin-clusters", "update")]
public record GcloudContainerBareMetalAdminClustersUpdateOptions(
[property: CliArgument] string AdminCluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--binauthz-evaluation-mode")]
    public string? BinauthzEvaluationMode { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-application-logs")]
    public bool? EnableApplicationLogs { get; set; }

    [CliOption("--island-mode-service-address-cidr-blocks")]
    public string[]? IslandModeServiceAddressCidrBlocks { get; set; }

    [CliOption("--login-user")]
    public string? LoginUser { get; set; }

    [CliOption("--maintenance-address-cidr-blocks")]
    public string[]? MaintenanceAddressCidrBlocks { get; set; }

    [CliOption("--max-pods-per-node")]
    public string? MaxPodsPerNode { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--api-server-args")]
    public IEnumerable<KeyValue>? ApiServerArgs { get; set; }

    [CliOption("--control-plane-node-configs")]
    public string[]? ControlPlaneNodeConfigs { get; set; }

    [CliOption("--control-plane-node-labels")]
    public IEnumerable<KeyValue>? ControlPlaneNodeLabels { get; set; }

    [CliOption("--control-plane-node-taints")]
    public IEnumerable<KeyValue>? ControlPlaneNodeTaints { get; set; }

    [CliOption("--no-proxy")]
    public string[]? NoProxy { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }
}