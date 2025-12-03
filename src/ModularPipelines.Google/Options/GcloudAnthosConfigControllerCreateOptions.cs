using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("anthos", "config", "controller", "create")]
public record GcloudAnthosConfigControllerCreateOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster-ipv4-cidr-block")]
    public string? ClusterIpv4CidrBlock { get; set; }

    [CliOption("--cluster-named-range")]
    public string? ClusterNamedRange { get; set; }

    [CliFlag("--full-management")]
    public bool? FullManagement { get; set; }

    [CliOption("--man-block")]
    public string? ManBlock { get; set; }

    [CliOption("--man-blocks")]
    public string[]? ManBlocks { get; set; }

    [CliOption("--master-ipv4-cidr-block")]
    public string? MasterIpv4CidrBlock { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--services-ipv4-cidr-block")]
    public string? ServicesIpv4CidrBlock { get; set; }

    [CliOption("--services-named-range")]
    public string? ServicesNamedRange { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliFlag("--use-private-endpoint")]
    public bool? UsePrivateEndpoint { get; set; }
}