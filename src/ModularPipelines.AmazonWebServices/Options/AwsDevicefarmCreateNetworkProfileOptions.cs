using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-network-profile")]
public record AwsDevicefarmCreateNetworkProfileOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--uplink-bandwidth-bits")]
    public long? UplinkBandwidthBits { get; set; }

    [CliOption("--downlink-bandwidth-bits")]
    public long? DownlinkBandwidthBits { get; set; }

    [CliOption("--uplink-delay-ms")]
    public long? UplinkDelayMs { get; set; }

    [CliOption("--downlink-delay-ms")]
    public long? DownlinkDelayMs { get; set; }

    [CliOption("--uplink-jitter-ms")]
    public long? UplinkJitterMs { get; set; }

    [CliOption("--downlink-jitter-ms")]
    public long? DownlinkJitterMs { get; set; }

    [CliOption("--uplink-loss-percent")]
    public int? UplinkLossPercent { get; set; }

    [CliOption("--downlink-loss-percent")]
    public int? DownlinkLossPercent { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}