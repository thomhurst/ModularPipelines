using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("telco-automation", "orchestration-cluster", "create")]
public record GcloudTelcoAutomationOrchestrationClusterCreateOptions(
[property: PositionalArgument] string OrchestrationCluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cidr-blocks")]
    public string[]? CidrBlocks { get; set; }

    [CommandSwitch("--cluster-cidr-block")]
    public string? ClusterCidrBlock { get; set; }

    [CommandSwitch("--cluster-named-range")]
    public string? ClusterNamedRange { get; set; }

    [BooleanCommandSwitch("--full-management-config")]
    public bool? FullManagementConfig { get; set; }

    [CommandSwitch("--master-ipv4-cidr-block")]
    public string? MasterIpv4CidrBlock { get; set; }

    [CommandSwitch("--services-cidr-block")]
    public string? ServicesCidrBlock { get; set; }

    [CommandSwitch("--services-named-range")]
    public string? ServicesNamedRange { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }
}