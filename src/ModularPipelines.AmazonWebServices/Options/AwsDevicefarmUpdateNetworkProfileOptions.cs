using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "update-network-profile")]
public record AwsDevicefarmUpdateNetworkProfileOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--uplink-bandwidth-bits")]
    public long? UplinkBandwidthBits { get; set; }

    [CommandSwitch("--downlink-bandwidth-bits")]
    public long? DownlinkBandwidthBits { get; set; }

    [CommandSwitch("--uplink-delay-ms")]
    public long? UplinkDelayMs { get; set; }

    [CommandSwitch("--downlink-delay-ms")]
    public long? DownlinkDelayMs { get; set; }

    [CommandSwitch("--uplink-jitter-ms")]
    public long? UplinkJitterMs { get; set; }

    [CommandSwitch("--downlink-jitter-ms")]
    public long? DownlinkJitterMs { get; set; }

    [CommandSwitch("--uplink-loss-percent")]
    public int? UplinkLossPercent { get; set; }

    [CommandSwitch("--downlink-loss-percent")]
    public int? DownlinkLossPercent { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}