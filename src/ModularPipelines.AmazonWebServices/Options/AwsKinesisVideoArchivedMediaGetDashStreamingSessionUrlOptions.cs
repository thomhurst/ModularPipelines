using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesis-video-archived-media", "get-dash-streaming-session-url")]
public record AwsKinesisVideoArchivedMediaGetDashStreamingSessionUrlOptions : AwsOptions
{
    [CliOption("--stream-name")]
    public string? StreamName { get; set; }

    [CliOption("--stream-arn")]
    public string? StreamArn { get; set; }

    [CliOption("--playback-mode")]
    public string? PlaybackMode { get; set; }

    [CliOption("--display-fragment-timestamp")]
    public string? DisplayFragmentTimestamp { get; set; }

    [CliOption("--display-fragment-number")]
    public string? DisplayFragmentNumber { get; set; }

    [CliOption("--dash-fragment-selector")]
    public string? DashFragmentSelector { get; set; }

    [CliOption("--expires")]
    public int? Expires { get; set; }

    [CliOption("--max-manifest-fragment-results")]
    public long? MaxManifestFragmentResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}