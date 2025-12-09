using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-traffic-mirror-session")]
public record AwsEc2CreateTrafficMirrorSessionOptions(
[property: CliOption("--network-interface-id")] string NetworkInterfaceId,
[property: CliOption("--traffic-mirror-target-id")] string TrafficMirrorTargetId,
[property: CliOption("--traffic-mirror-filter-id")] string TrafficMirrorFilterId,
[property: CliOption("--session-number")] int SessionNumber
) : AwsOptions
{
    [CliOption("--packet-length")]
    public int? PacketLength { get; set; }

    [CliOption("--virtual-network-id")]
    public int? VirtualNetworkId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}