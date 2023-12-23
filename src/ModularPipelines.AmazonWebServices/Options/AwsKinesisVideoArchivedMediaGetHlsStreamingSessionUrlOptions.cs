using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-archived-media", "get-hls-streaming-session-url")]
public record AwsKinesisVideoArchivedMediaGetHlsStreamingSessionUrlOptions : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--playback-mode")]
    public string? PlaybackMode { get; set; }

    [CommandSwitch("--hls-fragment-selector")]
    public string? HlsFragmentSelector { get; set; }

    [CommandSwitch("--container-format")]
    public string? ContainerFormat { get; set; }

    [CommandSwitch("--discontinuity-mode")]
    public string? DiscontinuityMode { get; set; }

    [CommandSwitch("--display-fragment-timestamp")]
    public string? DisplayFragmentTimestamp { get; set; }

    [CommandSwitch("--expires")]
    public int? Expires { get; set; }

    [CommandSwitch("--max-media-playlist-fragment-results")]
    public long? MaxMediaPlaylistFragmentResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}