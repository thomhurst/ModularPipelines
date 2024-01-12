using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos", "config", "controller", "create")]
public record GcloudAnthosConfigControllerCreateOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cluster-ipv4-cidr-block")]
    public string? ClusterIpv4CidrBlock { get; set; }

    [CommandSwitch("--cluster-named-range")]
    public string? ClusterNamedRange { get; set; }

    [BooleanCommandSwitch("--full-management")]
    public bool? FullManagement { get; set; }

    [CommandSwitch("--man-block")]
    public string? ManBlock { get; set; }

    [CommandSwitch("--man-blocks")]
    public string[]? ManBlocks { get; set; }

    [CommandSwitch("--master-ipv4-cidr-block")]
    public string? MasterIpv4CidrBlock { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--services-ipv4-cidr-block")]
    public string? ServicesIpv4CidrBlock { get; set; }

    [CommandSwitch("--services-named-range")]
    public string? ServicesNamedRange { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [BooleanCommandSwitch("--use-private-endpoint")]
    public bool? UsePrivateEndpoint { get; set; }
}