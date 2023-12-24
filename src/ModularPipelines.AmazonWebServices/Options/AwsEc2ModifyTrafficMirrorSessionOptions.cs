using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-traffic-mirror-session")]
public record AwsEc2ModifyTrafficMirrorSessionOptions(
[property: CommandSwitch("--traffic-mirror-session-id")] string TrafficMirrorSessionId
) : AwsOptions
{
    [CommandSwitch("--traffic-mirror-target-id")]
    public string? TrafficMirrorTargetId { get; set; }

    [CommandSwitch("--traffic-mirror-filter-id")]
    public string? TrafficMirrorFilterId { get; set; }

    [CommandSwitch("--packet-length")]
    public int? PacketLength { get; set; }

    [CommandSwitch("--session-number")]
    public int? SessionNumber { get; set; }

    [CommandSwitch("--virtual-network-id")]
    public int? VirtualNetworkId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--remove-fields")]
    public string[]? RemoveFields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}