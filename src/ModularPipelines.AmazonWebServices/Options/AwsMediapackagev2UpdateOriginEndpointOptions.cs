using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackagev2", "update-origin-endpoint")]
public record AwsMediapackagev2UpdateOriginEndpointOptions(
[property: CliOption("--channel-group-name")] string ChannelGroupName,
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--origin-endpoint-name")] string OriginEndpointName,
[property: CliOption("--container-type")] string ContainerType
) : AwsOptions
{
    [CliOption("--segment")]
    public string? Segment { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--startover-window-seconds")]
    public int? StartoverWindowSeconds { get; set; }

    [CliOption("--hls-manifests")]
    public string[]? HlsManifests { get; set; }

    [CliOption("--low-latency-hls-manifests")]
    public string[]? LowLatencyHlsManifests { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}