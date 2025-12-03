using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-traffic-mirror-session")]
public record AwsEc2ModifyTrafficMirrorSessionOptions(
[property: CliOption("--traffic-mirror-session-id")] string TrafficMirrorSessionId
) : AwsOptions
{
    [CliOption("--traffic-mirror-target-id")]
    public string? TrafficMirrorTargetId { get; set; }

    [CliOption("--traffic-mirror-filter-id")]
    public string? TrafficMirrorFilterId { get; set; }

    [CliOption("--packet-length")]
    public int? PacketLength { get; set; }

    [CliOption("--session-number")]
    public int? SessionNumber { get; set; }

    [CliOption("--virtual-network-id")]
    public int? VirtualNetworkId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--remove-fields")]
    public string[]? RemoveFields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}