using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("telco-automation", "orchestration-cluster", "create")]
public record GcloudTelcoAutomationOrchestrationClusterCreateOptions(
[property: CliArgument] string OrchestrationCluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cidr-blocks")]
    public string[]? CidrBlocks { get; set; }

    [CliOption("--cluster-cidr-block")]
    public string? ClusterCidrBlock { get; set; }

    [CliOption("--cluster-named-range")]
    public string? ClusterNamedRange { get; set; }

    [CliFlag("--full-management-config")]
    public bool? FullManagementConfig { get; set; }

    [CliOption("--master-ipv4-cidr-block")]
    public string? MasterIpv4CidrBlock { get; set; }

    [CliOption("--services-cidr-block")]
    public string? ServicesCidrBlock { get; set; }

    [CliOption("--services-named-range")]
    public string? ServicesNamedRange { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }
}