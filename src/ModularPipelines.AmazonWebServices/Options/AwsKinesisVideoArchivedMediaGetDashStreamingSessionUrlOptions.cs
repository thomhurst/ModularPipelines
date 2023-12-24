using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesis-video-archived-media", "get-dash-streaming-session-url")]
public record AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions : AwsOptions
{
    [CommandSwitch("--stream-name")]
    public string? StreamName { get; set; }

    [CommandSwitch("--stream-arn")]
    public string? StreamArn { get; set; }

    [CommandSwitch("--playback-mode")]
    public string? PlaybackMode { get; set; }

    [CommandSwitch("--display-fragment-timestamp")]
    public string? DisplayFragmentTimestamp { get; set; }

    [CommandSwitch("--display-fragment-number")]
    public string? DisplayFragmentNumber { get; set; }

    [CommandSwitch("--dash-fragment-selector")]
    public string? DashFragmentSelector { get; set; }

    [CommandSwitch("--expires")]
    public int? Expires { get; set; }

    [CommandSwitch("--max-manifest-fragment-results")]
    public long? MaxManifestFragmentResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}