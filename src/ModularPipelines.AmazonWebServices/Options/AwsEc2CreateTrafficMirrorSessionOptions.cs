using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-traffic-mirror-session")]
public record AwsEc2CreateTrafficMirrorSessionOptions(
[property: CommandSwitch("--network-interface-id")] string NetworkInterfaceId,
[property: CommandSwitch("--traffic-mirror-target-id")] string TrafficMirrorTargetId,
[property: CommandSwitch("--traffic-mirror-filter-id")] string TrafficMirrorFilterId,
[property: CommandSwitch("--session-number")] int SessionNumber
) : AwsOptions
{
    [CommandSwitch("--packet-length")]
    public int? PacketLength { get; set; }

    [CommandSwitch("--virtual-network-id")]
    public int? VirtualNetworkId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}