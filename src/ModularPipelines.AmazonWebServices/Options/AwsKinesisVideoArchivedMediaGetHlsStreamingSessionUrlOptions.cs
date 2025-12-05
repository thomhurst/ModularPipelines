using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-archived-media", "get-hls-streaming-session-url")]
public record AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--playback-mode")]
    public string? PlaybackMode { get; set; }

    [CliOption("--hls-fragment-selector")]
    public string? HlsFragmentSelector { get; set; }

    [CliOption("--container-format")]
    public string? ContainerFormat { get; set; }

    [CliOption("--discontinuity-mode")]
    public string? DiscontinuityMode { get; set; }

    [CliOption("--display-fragment-timestamp")]
    public string? DisplayFragmentTimestamp { get; set; }

    [CliOption("--expires")]
    public int? Expires { get; set; }

    [CliOption("--max-media-playlist-fragment-results")]
    public long? MaxMediaPlaylistFragmentResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}